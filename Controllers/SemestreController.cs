using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Http;
using Gestion_Salle_classe.Models;

namespace Gestion_Salle_classe.Controllers
{
    [RoutePrefix("api/Semestre")]
    public class SemestreController : ApiController
    {
        private EMITDbContext db = new EMITDbContext();

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetSemestres()
        {
            var semestres = db.Semestres
                .Include(s => s.RefSemestre)
                .Include(s => s.AnneeAcademique)
                .OrderBy(s => s.IdAnnee)
                .ThenBy(s => s.RefSemestre.Ordre)
                .ToList();
            return Ok(semestres);
        }

        [HttpGet]
        [Route("Annee/{anneeId:int}")]
        public IHttpActionResult GetSemestresByAnnee(int anneeId)
        {
            var semestres = db.Semestres
                .Where(s => s.IdAnnee == anneeId)
                .Include(s => s.RefSemestre)
                .OrderBy(s => s.RefSemestre.Ordre)
                .ToList();
            return Ok(semestres);
        }

        [HttpGet]
        [Route("Reference")]
        public IHttpActionResult GetReferenceSemestres()
        {
            var refSemestres = db.RefSemestres
                .Include(r => r.Niveau)
                .OrderBy(r => r.Ordre)
                .ToList();
            return Ok(refSemestres);
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult CreateSemestre(Semestre semestre)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Check if this reference semester is already configured for the given academic year
            bool exists = db.Semestres.Any(s => s.IdRefSemestre == semestre.IdRefSemestre && s.IdAnnee == semestre.IdAnnee);
            if (exists)
            {
                return BadRequest("Ce semestre est déjà configuré pour cette année académique.");
            }

            // Generate unique ID
            int nextId = db.Semestres.Any() ? db.Semestres.Max(s => s.IdSemestre) + 1 : 1;
            semestre.IdSemestre = nextId;

            db.Semestres.Add(semestre);
            db.SaveChanges();

            // Re-fetch with references included
            var created = db.Semestres
                .Include(s => s.RefSemestre)
                .Include(s => s.AnneeAcademique)
                .FirstOrDefault(s => s.IdSemestre == semestre.IdSemestre);

            return CreatedAtRoute("DefaultApi", new { id = semestre.IdSemestre }, created);
        }

        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult UpdateSemestre(int id, Semestre semestre)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (id != semestre.IdSemestre) return BadRequest();

            db.Entry(semestre).State = EntityState.Modified;
            db.SaveChanges();
            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult DeleteSemestre(int id)
        {
            var semestre = db.Semestres.Find(id);
            if (semestre == null) return NotFound();

            db.Semestres.Remove(semestre);
            db.SaveChanges();
            return Ok(semestre);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}
