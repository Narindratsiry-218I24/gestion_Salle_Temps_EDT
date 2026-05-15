using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
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
            var cours = db.Cours.Include(c => c.Matiere).Include(c => c.Professeur)
                        .Include(c => c.Classe).Include(c => c.Salle).ToList();
            return Ok(cours);
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