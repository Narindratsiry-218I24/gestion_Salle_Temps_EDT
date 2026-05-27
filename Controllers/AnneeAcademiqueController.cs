using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Http;
using Gestion_Salle_classe.Models;

namespace Gestion_Salle_classe.Controllers
{
    [RoutePrefix("api/AnneeAcademique")]
    public class AnneeAcademiqueController : ApiController
    {
        private EMITDbContext db = new EMITDbContext();

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAnneeAcademiques()
        {
            var list = db.AnneesAcademiques.OrderByDescending(a => a.IdAnnee).ToList();
            return Ok(list);
        }

        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetAnneeAcademique(int id)
        {
            var item = db.AnneesAcademiques.FirstOrDefault(a => a.IdAnnee == id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult CreateAnneeAcademique(AnneeAcademique annee)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Set state to default if empty
            if (string.IsNullOrEmpty(annee.Etat))
            {
                annee.Etat = "en_attente";
            }

            // Ensure unique active year if requested
            if (annee.Etat == "actif")
            {
                var activeYears = db.AnneesAcademiques.Where(a => a.Etat == "actif").ToList();
                foreach (var y in activeYears)
                {
                    y.Etat = "cloture";
                }
            }

            // Generate unique ID since it's not identity in SQL schema
            int nextId = db.AnneesAcademiques.Any() ? db.AnneesAcademiques.Max(a => a.IdAnnee) + 1 : 1;
            annee.IdAnnee = nextId;

            db.AnneesAcademiques.Add(annee);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = annee.IdAnnee }, annee);
        }

        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult UpdateAnneeAcademique(int id, AnneeAcademique annee)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (id != annee.IdAnnee) return BadRequest();

            // Handle uniqueness of active year
            if (annee.Etat == "actif")
            {
                var activeYears = db.AnneesAcademiques.Where(a => a.Etat == "actif" && a.IdAnnee != id).ToList();
                foreach (var y in activeYears)
                {
                    y.Etat = "cloture";
                }
            }

            db.Entry(annee).State = EntityState.Modified;
            db.SaveChanges();
            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult DeleteAnneeAcademique(int id)
        {
            var item = db.AnneesAcademiques.Find(id);
            if (item == null) return NotFound();

            // Remove associated semestres first to avoid foreign key violations
            var semestres = db.Semestres.Where(s => s.IdAnnee == id).ToList();
            foreach (var s in semestres)
            {
                db.Semestres.Remove(s);
            }

            db.AnneesAcademiques.Remove(item);
            db.SaveChanges();
            return Ok(item);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}
