using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Http;
using Gestion_Salle_classe.Models;

namespace Gestion_Salle_classe.Controllers
{
    [RoutePrefix("api/Professeur")]
    public class ProfesseurController : ApiController
    {
        private EMITDbContext db = new EMITDbContext();

        // GET: api/Professeur
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetProfesseurs()
        {
            return Ok(db.Professeurs.ToList());
        }

        // GET: api/Professeur/5
        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetProfesseur(int id)
        {
            Professeur professeur = db.Professeurs.Find(id);
            if (professeur == null)
            {
                return NotFound();
            }
            return Ok(professeur);
        }

        // POST: api/Professeur
        [HttpPost]
        [Route("")]
        public IHttpActionResult CreateProfesseur(Professeur professeur)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Professeurs.Add(professeur);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = professeur.IdProfesseur }, professeur);
        }

        // PUT: api/Professeur/5
        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult UpdateProfesseur(int id, Professeur professeur)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != professeur.IdProfesseur)
            {
                return BadRequest();
            }

            db.Entry(professeur).State = EntityState.Modified;
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // DELETE: api/Professeur/5
        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult DeleteProfesseur(int id)
        {
            Professeur professeur = db.Professeurs.Find(id);
            if (professeur == null)
            {
                return NotFound();
            }

            db.Professeurs.Remove(professeur);
            db.SaveChanges();

            return Ok(professeur);
        }

        // GET: api/Professeur/Cours/5
        [HttpGet]
        [Route("{id:int}/Cours")]
        public IHttpActionResult GetCoursDuProfesseur(int id)
        {
            var cours = db.Cours.Where(c => c.IdProfesseur == id)
                        .Include(c => c.Matiere)
                        .Include(c => c.Classe)
                        .ToList();
            return Ok(cours);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
