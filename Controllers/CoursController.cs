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

        /// <summary>
        /// GET api/Cours — tous les cours avec filtres optionnels
        /// ?idNiveau=1  &idParcours=2  &idMention=3  &idFiliere=4
        /// </summary>
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetCours(
            [FromUri] int? idNiveau = null,
            [FromUri] int? idParcours = null,
            [FromUri] int? idMention = null,
            [FromUri] int? idFiliere = null)
        {
            try
            {
                var query = db.Cours
                    .Include(c => c.Matiere)
                    .Include(c => c.Matiere.Filiere)
                    .Include(c => c.Matiere.Filiere.Parcours)
                    .Include(c => c.Matiere.Filiere.Mention)
                    .Include(c => c.Matiere.RefSemestre)
                    .Include(c => c.Matiere.RefSemestre.Niveau)
                    .Include(c => c.Professeur)
                    .Include(c => c.Classe)
                    .Include(c => c.Salle)
                    .Include(c => c.Semestre)
                    .AsQueryable();

                // Filtre par Filière directe
                if (idFiliere.HasValue)
                    query = query.Where(c => c.Matiere.IdFiliere == idFiliere.Value);

                // Filtre par Parcours
                if (idParcours.HasValue)
                    query = query.Where(c => c.Matiere.Filiere.IdParcours == idParcours.Value);

                // Filtre par Mention
                if (idMention.HasValue)
                    query = query.Where(c => c.Matiere.Filiere.IdMention == idMention.Value);

                // Filtre par Niveau (via RefSemestre -> Niveau)
                if (idNiveau.HasValue)
                    query = query.Where(c => c.Matiere.RefSemestre.IdNiveau == idNiveau.Value);

                var cours = query.ToList();

                // Project to anonymous DTO to avoid EF circular reference serialization issues
                var result = cours.Select(c => new {
                    c.IdCours,
                    c.IdMatiere,
                    c.IdProfesseur,
                    c.IdClasse,
                    c.IdSalle,
                    c.IdSemestre,
                    c.TypeCours,
                    c.Statut,
                    Matiere = c.Matiere == null ? null : new {
                        c.Matiere.IdMatiere,
                        c.Matiere.CodeMatiere,
                        c.Matiere.NomMatiere,
                        c.Matiere.Credit,
                        c.Matiere.IdFiliere,
                        c.Matiere.IdRefSemestre,
                        Filiere = c.Matiere.Filiere == null ? null : new {
                            c.Matiere.Filiere.IdFiliere,
                            c.Matiere.Filiere.NomFiliere,
                            c.Matiere.Filiere.IdMention,
                            c.Matiere.Filiere.IdParcours,
                            Mention = c.Matiere.Filiere.Mention == null ? null : new {
                                c.Matiere.Filiere.Mention.IdMention,
                                c.Matiere.Filiere.Mention.NomMention,
                                c.Matiere.Filiere.Mention.CodeMention
                            }
                        },
                        RefSemestre = c.Matiere.RefSemestre == null ? null : new {
                            c.Matiere.RefSemestre.IdRefSemestre,
                            c.Matiere.RefSemestre.CodeSemestre,
                            Niveau = c.Matiere.RefSemestre.Niveau == null ? null : new {
                                c.Matiere.RefSemestre.Niveau.IdNiveau,
                                c.Matiere.RefSemestre.Niveau.CodeNiveau
                            }
                        }
                    },
                    Professeur = c.Professeur == null ? null : new {
                        c.Professeur.IdProfesseur,
                        c.Professeur.Nom,
                        c.Professeur.Prenom,
                        c.Professeur.Email
                    },
                    Classe = c.Classe == null ? null : new {
                        c.Classe.IdClasse,
                        c.Classe.NomClasse
                    },
                    Salle = c.Salle == null ? null : new {
                        c.Salle.IdSalle,
                        c.Salle.NomSalle,
                        c.Salle.CodeBatiment,
                        c.Salle.NumeroPorte
                    },
                    Semestre = c.Semestre == null ? null : new {
                        c.Semestre.IdSemestre,
                        c.Semestre.IdRefSemestre,
                        RefSemestre = c.Semestre.RefSemestre == null ? null : new {
                            c.Semestre.RefSemestre.CodeSemestre
                        }
                    }
                });

                return Ok(result);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception in GetCours: {ex.Message}");
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetCours(int id)
        {
            var cours = db.Cours
                .Include(c => c.Matiere)
                .Include(c => c.Matiere.Filiere)
                .Include(c => c.Matiere.Filiere.Mention)
                .Include(c => c.Matiere.Filiere.Parcours)
                .Include(c => c.Professeur)
                .Include(c => c.Classe)
                .Include(c => c.Salle)
                .Include(c => c.Creneaux)
                .FirstOrDefault(c => c.IdCours == id);
            if (cours == null) return NotFound();
            return Ok(cours);
        }

        /// <summary>
        /// GET api/Cours/Semestres?idNiveau=1 — semestres disponibles pour le planning
        /// </summary>
        [HttpGet]
        [Route("Semestres")]
        public IHttpActionResult GetSemestres([FromUri] int? idNiveau = null)
        {
            try
            {
                var query = db.Semestres
                    .Include(s => s.RefSemestre)
                    .Include(s => s.RefSemestre.Niveau)
                    .AsQueryable();

                if (idNiveau.HasValue)
                    query = query.Where(s => s.RefSemestre.IdNiveau == idNiveau.Value);

                var result = query.ToList().Select(s => new {
                    s.IdSemestre,
                    s.IdRefSemestre,
                    s.DateDebut,
                    s.DateFin,
                    RefSemestre = s.RefSemestre == null ? null : new {
                        s.RefSemestre.IdRefSemestre,
                        s.RefSemestre.CodeSemestre,
                        s.RefSemestre.Ordre,
                        Niveau = s.RefSemestre.Niveau == null ? null : new {
                            s.RefSemestre.Niveau.IdNiveau,
                            s.RefSemestre.Niveau.CodeNiveau
                        }
                    }
                });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("Planifier")]
        public IHttpActionResult PlanifierCours(Cours cours)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (cours.IdMatiere <= 0) return BadRequest("Veuillez sélectionner une matière valide.");
            if (cours.IdProfesseur <= 0) return BadRequest("Veuillez sélectionner un professeur valide.");
            if (cours.IdClasse <= 0) return BadRequest("Veuillez sélectionner une classe valide.");
            if (cours.IdSalle <= 0) return BadRequest("Veuillez sélectionner une salle valide.");
            if (cours.IdSemestre <= 0) return BadRequest("Veuillez sélectionner un semestre valide.");

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
                        return BadRequest("Conflit de salle détecté pour l'un des créneaux.");
                }
            }

            // Detach navigation objects to avoid EF tracking/insert conflicts
            cours.Matiere = null;
            cours.Professeur = null;
            cours.Classe = null;
            cours.Salle = null;
            cours.Semestre = null;
            cours.DemandesEdt = null;
            if (cours.Creneaux != null)
                foreach (var c in cours.Creneaux) c.Cours = null;

            if (string.IsNullOrEmpty(cours.Statut))
                cours.Statut = "en attente";

            try
            {
                db.Cours.Add(cours);
                db.SaveChanges();
                return Ok(new {
                    cours.IdCours, cours.IdMatiere, cours.IdProfesseur,
                    cours.IdClasse, cours.IdSalle, cours.IdSemestre,
                    cours.TypeCours, cours.Statut
                });
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                var errors = string.Join("; ", ex.EntityValidationErrors
                    .SelectMany(e => e.ValidationErrors)
                    .Select(e => e.ErrorMessage));
                return BadRequest($"Erreur de validation: {errors}");
            }
            catch (Exception ex)
            {
                var inner = ex.InnerException?.InnerException?.Message
                         ?? ex.InnerException?.Message
                         ?? ex.Message;
                return BadRequest($"Erreur DB: {inner}");
            }
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