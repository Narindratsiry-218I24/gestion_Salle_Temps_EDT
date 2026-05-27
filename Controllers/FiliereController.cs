using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Http;
using Gestion_Salle_classe.Models;

namespace Gestion_Salle_classe.Controllers
{
    [RoutePrefix("api/Filiere")]
    public class FiliereController : ApiController
    {
        private EMITDbContext db = new EMITDbContext();

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetFilieres()
        {
            var filieres = db.Filieres.Include(f => f.Mention).OrderBy(f => f.IdMention).ToList();
            return Ok(filieres);
        }

        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetFiliere(int id)
        {
            var filiere = db.Filieres.Include(f => f.Mention).FirstOrDefault(f => f.IdFiliere == id);
            if (filiere == null) return NotFound();
            return Ok(filiere);
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult CreateFiliere(Filiere filiere)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Generate unique ID
            int nextId = db.Filieres.Any() ? db.Filieres.Max(f => f.IdFiliere) + 1 : 1;
            filiere.IdFiliere = nextId;

            db.Filieres.Add(filiere);
            db.SaveChanges();

            // Re-fetch with Mention
            var created = db.Filieres.Include(f => f.Mention).FirstOrDefault(f => f.IdFiliere == filiere.IdFiliere);
            return CreatedAtRoute("DefaultApi", new { id = filiere.IdFiliere }, created);
        }

        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult UpdateFiliere(int id, Filiere filiere)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (id != filiere.IdFiliere) return BadRequest();

            db.Entry(filiere).State = EntityState.Modified;
            db.SaveChanges();
            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult DeleteFiliere(int id)
        {
            var filiere = db.Filieres.Find(id);
            if (filiere == null) return NotFound();

            db.Filieres.Remove(filiere);
            db.SaveChanges();
            return Ok(filiere);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}
