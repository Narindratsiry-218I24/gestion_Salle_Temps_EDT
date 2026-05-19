using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Http;
using Gestion_Salle_classe.Models;

namespace Gestion_Salle_classe.Controllers
{
    [RoutePrefix("api/Classe")]
    public class ClasseController : ApiController
    {
        private EMITDbContext db = new EMITDbContext();

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetClasses()
        {
            var classes = db.Classes.Include(c => c.Filiere).Include(c => c.Semestre).ToList();
            return Ok(classes);
        }

        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetClasse(int id)
        {
            var classe = db.Classes.Include(c => c.Filiere).Include(c => c.Semestre)
                        .FirstOrDefault(c => c.IdClasse == id);
            if (classe == null) return NotFound();
            return Ok(classe);
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult CreateClasse(Classe classe)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            db.Classes.Add(classe);
            db.SaveChanges();
            return CreatedAtRoute("DefaultApi", new { id = classe.IdClasse }, classe);
        }

        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult UpdateClasse(int id, Classe classe)
        {
            if (id != classe.IdClasse) return BadRequest();
            db.Entry(classe).State = EntityState.Modified;
            db.SaveChanges();
            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult DeleteClasse(int id)
        {
            var classe = db.Classes.Find(id);
            if (classe == null) return NotFound();
            db.Classes.Remove(classe);
            db.SaveChanges();
            return Ok(classe);
        }

        [HttpGet]
        [Route("{id:int}/EmploiDuTemps")]
        public IHttpActionResult GetEmploiDuTemps(int id)
        {
            var cours = db.Cours.Where(c => c.IdClasse == id)
                        .Include(c => c.Matiere)
                        .Include(c => c.Professeur)
                        .Include(c => c.Salle)
                        .ToList();
            return Ok(cours);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}