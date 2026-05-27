using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Http;
using Gestion_Salle_classe.Models;

namespace Gestion_Salle_classe.Controllers
{
    [RoutePrefix("api/Niveau")]
    public class NiveauController : ApiController
    {
        private EMITDbContext db = new EMITDbContext();

        // GET api/Niveau
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetNiveaux()
        {
            try
            {
                var niveaux = db.Niveaux
                    .OrderBy(n => n.Ordre)
                    .Select(n => new
                    {
                        n.IdNiveau,
                        n.CodeNiveau,
                        n.Ordre,
                        n.IdMention,
                        Mention = n.Mention == null ? null : new
                        {
                            n.Mention.IdMention,
                            n.Mention.CodeMention,
                            n.Mention.NomMention
                        }
                    })
                    .ToList();
                return Ok(niveaux);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // GET api/Niveau/{id}
        [HttpGet]
        [Route("{id:int}", Name = "GetNiveauById")]
        public IHttpActionResult GetNiveau(int id)
        {
            var niveau = db.Niveaux
                .Where(n => n.IdNiveau == id)
                .Select(n => new
                {
                    n.IdNiveau,
                    n.CodeNiveau,
                    n.Ordre,
                    n.IdMention,
                    Mention = n.Mention == null ? null : new
                    {
                        n.Mention.IdMention,
                        n.Mention.CodeMention,
                        n.Mention.NomMention
                    }
                })
                .FirstOrDefault();
            if (niveau == null) return NotFound();
            return Ok(niveau);
        }

        // GET api/Niveau/ByMention/{idMention}
        [HttpGet]
        [Route("ByMention/{idMention:int}")]
        public IHttpActionResult GetNiveauxByMention(int idMention)
        {
            var niveaux = db.Niveaux
                .Where(n => n.IdMention == idMention)
                .OrderBy(n => n.Ordre)
                .Select(n => new
                {
                    n.IdNiveau,
                    n.CodeNiveau,
                    n.Ordre,
                    n.IdMention,
                    Mention = n.Mention == null ? null : new
                    {
                        n.Mention.IdMention,
                        n.Mention.CodeMention,
                        n.Mention.NomMention
                    }
                })
                .ToList();
            return Ok(niveaux);
        }

        // POST api/Niveau
        [HttpPost]
        [Route("")]
        public IHttpActionResult CreateNiveau(Niveau niveau)
        {
            if (niveau == null) return BadRequest("Données manquantes.");
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                db.Niveaux.Add(niveau);
                db.SaveChanges();
                return CreatedAtRoute("GetNiveauById", new { id = niveau.IdNiveau }, niveau);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erreur DB: {ex.Message}");
            }
        }

        // PUT api/Niveau/{id}
        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult UpdateNiveau(int id, Niveau niveau)
        {
            if (id != niveau.IdNiveau) return BadRequest();
            db.Entry(niveau).State = EntityState.Modified;
            db.SaveChanges();
            return StatusCode(HttpStatusCode.NoContent);
        }

        // DELETE api/Niveau/{id}
        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult DeleteNiveau(int id)
        {
            var niveau = db.Niveaux.Find(id);
            if (niveau == null) return NotFound();
            db.Niveaux.Remove(niveau);
            db.SaveChanges();
            return Ok(niveau);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}
