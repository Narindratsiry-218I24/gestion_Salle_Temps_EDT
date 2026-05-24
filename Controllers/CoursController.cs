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

            // Vérification de conflit de salle
            if (cours.Creneaux != null && cours.Creneaux.Any())
            {
                foreach (var creneau in cours.Creneaux)
                {
                    bool conflit = db.Creneaux.Any(c => 
                        c.Cours.IdSalle == cours.IdSalle &&
                        c.JourSemaine == creneau.JourSemaine &&
                        ((creneau.HeureDebut >= c.HeureDebut && creneau.HeureDebut < c.HeureFin) ||
                         (creneau.HeureFin > c.HeureDebut && creneau.HeureFin <= c.HeureFin) ||
                         (creneau.HeureDebut <= c.HeureDebut && creneau.HeureFin >= c.HeureFin))
                    );

                    if (conflit)
                    {
                        return BadRequest("Conflit de salle détecté pour l'un des créneaux.");
                    }
                }
            }

            db.Cours.Add(cours);
            db.SaveChanges();
            return CreatedAtRoute("DefaultApi", new { id = cours.IdCours }, cours);
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