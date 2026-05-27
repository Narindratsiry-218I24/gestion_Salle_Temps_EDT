using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Http;
using Gestion_Salle_classe.Models;

namespace Gestion_Salle_classe.Controllers
{
    [RoutePrefix("api/Parcours")]
    public class ParcoursController : ApiController
    {
        private EMITDbContext db = new EMITDbContext();

        // GET api/Parcours  — tous les parcours (avec leur mention)
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetParcours()
        {
            try
            {
                var parcours = db.Parcours
                    .Select(p => new
                    {
                        p.IdParcours,
                        p.CodeParcours,
                        p.NomParcours,
                        p.Cycle,
                        p.IdMention,
                        Mention = p.Mention == null ? null : new
                        {
                            p.Mention.IdMention,
                            p.Mention.CodeMention,
                            p.Mention.NomMention
                        },
                        Filieres = p.Filieres.Select(f => new
                        {
                            f.IdFiliere,
                            f.CodeFiliere,
                            f.NomFiliere,
                            f.IdMention,
                            f.IdParcours
                        })
                    })
                    .ToList();
                return Ok(parcours);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // GET api/Parcours/{id}
        [HttpGet]
        [Route("{id:int}", Name = "GetParcoursById")]
        public IHttpActionResult GetParcoursById(int id)
        {
            var parcours = db.Parcours
                .Where(p => p.IdParcours == id)
                .Select(p => new
                {
                    p.IdParcours,
                    p.CodeParcours,
                    p.NomParcours,
                    p.Cycle,
                    p.IdMention,
                    Mention = p.Mention == null ? null : new
                    {
                        p.Mention.IdMention,
                        p.Mention.CodeMention,
                        p.Mention.NomMention
                    },
                    Filieres = p.Filieres.Select(f => new
                    {
                        f.IdFiliere,
                        f.CodeFiliere,
                        f.NomFiliere,
                        f.IdMention,
                        f.IdParcours
                    })
                })
                .FirstOrDefault();
            if (parcours == null) return NotFound();
            return Ok(parcours);
        }

        // GET api/Parcours/ByMention/{idMention}  — parcours d'une mention
        [HttpGet]
        [Route("ByMention/{idMention:int}")]
        public IHttpActionResult GetParcoursByMention(int idMention)
        {
            var parcours = db.Parcours
                .Where(p => p.IdMention == idMention)
                .Select(p => new
                {
                    p.IdParcours,
                    p.CodeParcours,
                    p.NomParcours,
                    p.Cycle,
                    p.IdMention,
                    Mention = p.Mention == null ? null : new
                    {
                        p.Mention.IdMention,
                        p.Mention.CodeMention,
                        p.Mention.NomMention
                    },
                    Filieres = p.Filieres.Select(f => new
                    {
                        f.IdFiliere,
                        f.CodeFiliere,
                        f.NomFiliere,
                        f.IdMention,
                        f.IdParcours
                    })
                })
                .ToList();
            return Ok(parcours);
        }

        // GET api/Parcours/ByCycle/{cycle}  — ex: "Licence" ou "Master"
        [HttpGet]
        [Route("ByCycle/{cycle}")]
        public IHttpActionResult GetParcoursByCycle(string cycle)
        {
            var parcours = db.Parcours
                .Where(p => p.Cycle.ToLower() == cycle.ToLower())
                .Select(p => new
                {
                    p.IdParcours,
                    p.CodeParcours,
                    p.NomParcours,
                    p.Cycle,
                    p.IdMention,
                    Mention = p.Mention == null ? null : new
                    {
                        p.Mention.IdMention,
                        p.Mention.CodeMention,
                        p.Mention.NomMention
                    },
                    Filieres = p.Filieres.Select(f => new
                    {
                        f.IdFiliere,
                        f.CodeFiliere,
                        f.NomFiliere,
                        f.IdMention,
                        f.IdParcours
                    })
                })
                .ToList();
            return Ok(parcours);
        }

        // POST api/Parcours  — créer un parcours
        [HttpPost]
        [Route("")]
        public IHttpActionResult CreateParcours(Parcours parcours)
        {
            if (parcours == null) return BadRequest("Données manquantes.");
            if (!ModelState.IsValid)
            {
                var errors = string.Join("; ", ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage));
                return BadRequest($"Erreur de validation: {errors}");
            }

            try
            {
                if (parcours.IdParcours == 0)
                {
                    var maxId = db.Parcours.Select(p => (int?)p.IdParcours).Max();
                    parcours.IdParcours = (maxId ?? 0) + 1;
                }

                db.Parcours.Add(parcours);
                db.SaveChanges();
                return CreatedAtRoute("GetParcoursById", new { id = parcours.IdParcours }, parcours);
            }
            catch (Exception ex)
            {
                var innerMsg = ex.InnerException != null ? ex.InnerException.Message : "";
                return BadRequest($"Erreur DB: {ex.Message} | Inner: {innerMsg}");
            }
        }

        // PUT api/Parcours/{id}  — modifier un parcours
        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult UpdateParcours(int id, Parcours parcours)
        {
            if (id != parcours.IdParcours) return BadRequest("ID incompatible.");
            db.Entry(parcours).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return BadRequest($"Erreur: {ex.Message}");
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        // DELETE api/Parcours/{id}
        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult DeleteParcours(int id)
        {
            var parcours = db.Parcours.Find(id);
            if (parcours == null) return NotFound();

            // Délier les filières rattachées à ce parcours avant suppression
            var filieres = db.Filieres.Where(f => f.IdParcours == id).ToList();
            foreach (var f in filieres)
            {
                f.IdParcours = null;
            }

            db.Parcours.Remove(parcours);
            db.SaveChanges();
            return Ok(parcours);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}
