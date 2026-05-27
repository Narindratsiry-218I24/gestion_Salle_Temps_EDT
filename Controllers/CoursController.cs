using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Diagnostics;
using System.Web.Http;
using Gestion_Salle_classe.Models;

namespace Gestion_Salle_classe.Controllers
{
    [RoutePrefix("api/Cours")]
    public class CoursController : ApiController
    {
        private EMITDbContext db = new EMITDbContext();

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetCours()
        {
            try
            {
                var cours = db.Cours.Include(c => c.Matiere).ToList();
                return Ok(cours);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Exception in GetCours: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetCours(int id)
        {
            var cours = db.Cours.Include(c => c.Matiere).Include(c => c.Professeur)
                        .Include(c => c.Classe).Include(c => c.Salle)
                        .FirstOrDefault(c => c.IdCours == id);
            if (cours == null) return NotFound();
            return Ok(cours);
        }

        [HttpPost]
        [Route("Planifier")]
        public IHttpActionResult PlanifierCours(Cours cours)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Vérification exhaustive de conflits
            if (cours.Creneaux != null && cours.Creneaux.Any())
            {
                foreach (var creneau in cours.Creneaux)
                {
                    // 1. Conflit de salle
                    bool conflitSalle = db.Creneaux.Any(c => 
                        c.Cours.IdSalle == cours.IdSalle &&
                        c.JourSemaine == creneau.JourSemaine &&
                        ((creneau.HeureDebut >= c.HeureDebut && creneau.HeureDebut < c.HeureFin) ||
                         (creneau.HeureFin > c.HeureDebut && creneau.HeureFin <= c.HeureFin) ||
                         (creneau.HeureDebut <= c.HeureDebut && creneau.HeureFin >= c.HeureFin))
                    );
                    if (conflitSalle) return BadRequest("Conflit : La salle est déjà occupée sur ce créneau.");

                    // 2. Conflit de professeur
                    bool conflitProf = db.Creneaux.Any(c => 
                        c.Cours.IdProfesseur == cours.IdProfesseur &&
                        c.JourSemaine == creneau.JourSemaine &&
                        ((creneau.HeureDebut >= c.HeureDebut && creneau.HeureDebut < c.HeureFin) ||
                         (creneau.HeureFin > c.HeureDebut && creneau.HeureFin <= c.HeureFin) ||
                         (creneau.HeureDebut <= c.HeureDebut && creneau.HeureFin >= c.HeureFin))
                    );
                    if (conflitProf) return BadRequest("Conflit : Le professeur a déjà un autre cours sur ce créneau.");

                    // 3. Conflit de classe
                    bool conflitClasse = db.Creneaux.Any(c => 
                        c.Cours.IdClasse == cours.IdClasse &&
                        c.JourSemaine == creneau.JourSemaine &&
                        ((creneau.HeureDebut >= c.HeureDebut && creneau.HeureDebut < c.HeureFin) ||
                         (creneau.HeureFin > c.HeureDebut && creneau.HeureFin <= c.HeureFin) ||
                         (creneau.HeureDebut <= c.HeureDebut && creneau.HeureFin >= c.HeureFin))
                    );
                    if (conflitClasse) return BadRequest("Conflit : La classe a déjà un autre cours sur ce créneau.");
                }
            }

            db.Cours.Add(cours);
            db.SaveChanges();
            return CreatedAtRoute("DefaultApi", new { id = cours.IdCours }, cours);
        }

        [HttpPost]
        [Route("VerifierConflit")]
        public IHttpActionResult VerifierConflit(Cours cours)
        {
            if (cours.Creneaux == null || !cours.Creneaux.Any()) return Ok(new { HasConflict = false });

            foreach (var creneau in cours.Creneaux)
            {
                // Conflit de salle
                var cSalle = db.Creneaux.Include(c => c.Cours.Matiere).FirstOrDefault(c => 
                    c.Cours.IdSalle == cours.IdSalle &&
                    c.JourSemaine == creneau.JourSemaine &&
                    c.IdCours != cours.IdCours &&
                    ((creneau.HeureDebut >= c.HeureDebut && creneau.HeureDebut < c.HeureFin) ||
                     (creneau.HeureFin > c.HeureDebut && creneau.HeureFin <= c.HeureFin) ||
                     (creneau.HeureDebut <= c.HeureDebut && creneau.HeureFin >= c.HeureFin))
                );
                if (cSalle != null) return Ok(new { HasConflict = true, Message = $"La salle est occupée par {cSalle.Cours.Matiere.NomMatiere}." });

                // Conflit de professeur
                var cProf = db.Creneaux.Include(c => c.Cours.Matiere).FirstOrDefault(c => 
                    c.Cours.IdProfesseur == cours.IdProfesseur &&
                    c.JourSemaine == creneau.JourSemaine &&
                    c.IdCours != cours.IdCours &&
                    ((creneau.HeureDebut >= c.HeureDebut && creneau.HeureDebut < c.HeureFin) ||
                     (creneau.HeureFin > c.HeureDebut && creneau.HeureFin <= c.HeureFin) ||
                     (creneau.HeureDebut <= c.HeureDebut && creneau.HeureFin >= c.HeureFin))
                );
                if (cProf != null) return Ok(new { HasConflict = true, Message = $"Le professeur enseigne déjà {cProf.Cours.Matiere.NomMatiere}." });
            }

            return Ok(new { HasConflict = false });
        }

        [HttpPost]
        [Route("{id:int}/Valider")]
        public IHttpActionResult ValiderCours(int id)
        {
            var cours = db.Cours.Find(id);
            if (cours == null) return NotFound();
            cours.Statut = "validé";
            db.SaveChanges();
            return Ok(cours);
        }

        [HttpPost]
        [Route("{id:int}/Annuler")]
        public IHttpActionResult AnnulerCours(int id)
        {
            var cours = db.Cours.Find(id);
            if (cours == null) return NotFound();
            cours.Statut = "annulé";
            db.SaveChanges();
            return Ok(cours);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}