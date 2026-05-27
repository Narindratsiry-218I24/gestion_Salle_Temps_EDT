using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Http;
using Gestion_Salle_classe.Models;

namespace Gestion_Salle_classe.Controllers
{
    [RoutePrefix("api/Matiere")]
    public class MatiereController : ApiController
    {
        private EMITDbContext db = new EMITDbContext();

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetMatieres()
        {
            try
            {
                var matieres = db.Matieres
                    .Select(m => new {
                    m.IdMatiere,
                    m.IdFiliere,
                    m.CodeMatiere,
                    m.NomMatiere,
                    m.IdRefSemestre,
                    m.Credit,
                    Filiere = m.Filiere == null ? null : new {
                        m.Filiere.IdFiliere,
                        m.Filiere.NomFiliere,
                        m.Filiere.CodeFiliere,
                        m.Filiere.IdMention,
                        m.Filiere.IdParcours,
                        Mention = m.Filiere.Mention == null ? null : new {
                            m.Filiere.Mention.IdMention,
                            m.Filiere.Mention.CodeMention,
                            m.Filiere.Mention.NomMention
                        },
                        Parcours = m.Filiere.Parcours == null ? null : new {
                            m.Filiere.Parcours.IdParcours,
                            m.Filiere.Parcours.CodeParcours,
                            m.Filiere.Parcours.NomParcours,
                            m.Filiere.Parcours.Cycle
                        }
                    },
                    RefSemestre = m.RefSemestre == null ? null : new {
                        m.RefSemestre.IdRefSemestre,
                        m.RefSemestre.CodeSemestre,
                        m.RefSemestre.Ordre,
                        Niveau = m.RefSemestre.Niveau == null ? null : new {
                            m.RefSemestre.Niveau.IdNiveau,
                            m.RefSemestre.Niveau.CodeNiveau
                        }
                    }
                })
                .ToList();

                return Ok(matieres);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erreur: {ex.Message}");
            }
        }


        [HttpGet]
        [Route("{id:int}", Name = "GetMatiereById")]
        public IHttpActionResult GetMatiere(int id)
        {
            var matiere = db.Matieres
                .Where(m => m.IdMatiere == id)
                .Select(m => new
                {
                    m.IdMatiere,
                    m.IdFiliere,
                    m.CodeMatiere,
                    m.NomMatiere,
                    m.IdRefSemestre,
                    m.Credit,
                    Filiere = m.Filiere == null ? null : new
                    {
                        m.Filiere.IdFiliere,
                        m.Filiere.NomFiliere,
                        m.Filiere.CodeFiliere,
                        m.Filiere.IdMention,
                        m.Filiere.IdParcours,
                        Mention = m.Filiere.Mention == null ? null : new
                        {
                            m.Filiere.Mention.IdMention,
                            m.Filiere.Mention.CodeMention,
                            m.Filiere.Mention.NomMention
                        },
                        Parcours = m.Filiere.Parcours == null ? null : new
                        {
                            m.Filiere.Parcours.IdParcours,
                            m.Filiere.Parcours.CodeParcours,
                            m.Filiere.Parcours.NomParcours,
                            m.Filiere.Parcours.Cycle
                        }
                    },
                    RefSemestre = m.RefSemestre == null ? null : new
                    {
                        m.RefSemestre.IdRefSemestre,
                        m.RefSemestre.CodeSemestre,
                        m.RefSemestre.Ordre,
                        Niveau = m.RefSemestre.Niveau == null ? null : new
                        {
                            m.RefSemestre.Niveau.IdNiveau,
                            m.RefSemestre.Niveau.CodeNiveau
                        }
                    }
                })
                .FirstOrDefault();
            if (matiere == null) return NotFound();
            return Ok(matiere);
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult CreateMatiere(Matiere matiere)
        {
            if (matiere == null) return BadRequest("Les données de la matière sont vides.");
            if (!ModelState.IsValid)
            {
                var errors = string.Join("; ", ModelState.Values
                                            .SelectMany(x => x.Errors)
                                            .Select(x => x.ErrorMessage));
                return BadRequest($"Erreur de validation: {errors}");
            }

            try
            {
                if (matiere.IdMatiere == 0)
                {
                    var maxId = db.Matieres.Select(m => (int?)m.IdMatiere).Max();
                    matiere.IdMatiere = (maxId ?? 0) + 1;
                }

                db.Matieres.Add(matiere);
                db.SaveChanges();
                return CreatedAtRoute("GetMatiereById", new { id = matiere.IdMatiere }, matiere);
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);
                var fullErrorMessage = string.Join("; ", errorMessages);
                var exceptionMessage = string.Concat(ex.Message, " Les erreurs de validation sont : ", fullErrorMessage);
                return BadRequest(exceptionMessage);
            }
            catch (Exception ex)
            {
                var innerMsg = ex.InnerException != null ? ex.InnerException.Message : "";
                var innerInnerMsg = ex.InnerException?.InnerException != null ? ex.InnerException.InnerException.Message : "";
                return BadRequest($"Erreur DB: {ex.Message} | Inner: {innerMsg} | Details: {innerInnerMsg}");
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult UpdateMatiere(int id, Matiere matiere)
        {
            if (id != matiere.IdMatiere) return BadRequest();
            db.Entry(matiere).State = EntityState.Modified;
            db.SaveChanges();
            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult DeleteMatiere(int id)
        {
            var matiere = db.Matieres.Find(id);
            if (matiere == null) return NotFound();
            db.Matieres.Remove(matiere);
            db.SaveChanges();
            return Ok(matiere);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}