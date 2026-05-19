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
            var matieres = db.Matieres.Include(m => m.Filiere).Include(m => m.RefSemestre).ToList();
            return Ok(matieres);
        }

        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetMatiere(int id)
        {
            var matiere = db.Matieres.Include(m => m.Filiere).Include(m => m.RefSemestre)
                            .FirstOrDefault(m => m.IdMatiere == id);
            if (matiere == null) return NotFound();
            return Ok(matiere);
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult CreateMatiere(Matiere matiere)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            db.Matieres.Add(matiere);
            db.SaveChanges();
            return CreatedAtRoute("DefaultApi", new { id = matiere.IdMatiere }, matiere);
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