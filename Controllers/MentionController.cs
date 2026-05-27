using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Http;
using Gestion_Salle_classe.Models;

namespace Gestion_Salle_classe.Controllers
{
    [RoutePrefix("api/Mention")]
    public class MentionController : ApiController
    {
        private EMITDbContext db = new EMITDbContext();

        // GET api/Mention
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetMentions()
        {
            try
            {
                var mentions = db.Mentions
                    .Select(m => new
                    {
                        m.IdMention,
                        m.CodeMention,
                        m.NomMention,
                        Niveaux = m.Niveaux.Select(n => new
                        {
                            n.IdNiveau,
                            n.CodeNiveau,
                            n.Ordre,
                            n.IdMention
                        }),
                        Parcours = m.Parcours.Select(p => new
                        {
                            p.IdParcours,
                            p.CodeParcours,
                            p.NomParcours,
                            p.Cycle,
                            p.IdMention
                        })
                    })
                    .ToList();

                return Ok(mentions);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erreur: {ex.Message}");
            }
        }

        // GET api/Mention/{id}
        [HttpGet]
        [Route("{id:int}", Name = "GetMentionById")]
        public IHttpActionResult GetMention(int id)
        {
            var mention = db.Mentions
                .Where(m => m.IdMention == id)
                .Select(m => new
                {
                    m.IdMention,
                    m.CodeMention,
                    m.NomMention,
                    Niveaux = m.Niveaux.Select(n => new
                    {
                        n.IdNiveau,
                        n.CodeNiveau,
                        n.Ordre,
                        n.IdMention
                    }),
                    Parcours = m.Parcours.Select(p => new
                    {
                        p.IdParcours,
                        p.CodeParcours,
                        p.NomParcours,
                        p.Cycle,
                        p.IdMention
                    })
                })
                .FirstOrDefault();
            if (mention == null) return NotFound();
            return Ok(mention);
        }

        // POST api/Mention
        [HttpPost]
        [Route("")]
        public IHttpActionResult CreateMention(Mention mention)
        {
            if (mention == null) return BadRequest("Données manquantes.");
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                if (mention.IdMention == 0)
                {
                    var maxId = db.Mentions.Select(m => (int?)m.IdMention).Max();
                    mention.IdMention = (maxId ?? 0) + 1;
                }
                db.Mentions.Add(mention);
                db.SaveChanges();
                return CreatedAtRoute("GetMentionById", new { id = mention.IdMention }, mention);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erreur DB: {ex.Message}");
            }
        }

        // PUT api/Mention/{id}
        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult UpdateMention(int id, Mention mention)
        {
            if (id != mention.IdMention) return BadRequest();
            db.Entry(mention).State = EntityState.Modified;
            db.SaveChanges();
            return StatusCode(HttpStatusCode.NoContent);
        }

        // DELETE api/Mention/{id}
        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult DeleteMention(int id)
        {
            var mention = db.Mentions.Find(id);
            if (mention == null) return NotFound();
            db.Mentions.Remove(mention);
            db.SaveChanges();
            return Ok(mention);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}
