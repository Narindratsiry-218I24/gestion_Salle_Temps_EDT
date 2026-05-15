using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Http;
using Gestion_Salle_classe.Models;

namespace Gestion_Salle_classe.Controllers
{
    [RoutePrefix("api/Salle")]
    public class SalleController : ApiController
    {
        private EMITDbContext db = new EMITDbContext();

        // GET: api/Salle
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetSalles()
        {
            return Ok(db.Salles.ToList());
        }

        // GET: api/Salle/5
        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetSalle(int id)
        {
            Salle salle = db.Salles.Find(id);
            if (salle == null)
            {
                return NotFound();
            }
            return Ok(salle);
        }

        // POST: api/Salle
        [HttpPost]
        [Route("")]
        public IHttpActionResult CreateSalle(Salle salle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Salles.Add(salle);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = salle.IdSalle }, salle);
        }

        // PUT: api/Salle/5
        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult UpdateSalle(int id, Salle salle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != salle.IdSalle)
            {
                return BadRequest();
            }

            db.Entry(salle).State = EntityState.Modified;
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // DELETE: api/Salle/5
        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult DeleteSalle(int id)
        {
            Salle salle = db.Salles.Find(id);
            if (salle == null)
            {
                return NotFound();
            }

            db.Salles.Remove(salle);
            db.SaveChanges();

            return Ok(salle);
        }

        // GET: api/Salle/Disponibles
        [HttpGet]
        [Route("Disponibles")]
        public IHttpActionResult GetSallesDisponibles()
        {
            return Ok(db.Salles.ToList());
        }

        // GET: api/Salle/ParType/{type}
        [HttpGet]
        [Route("ParType/{type}")]
        public IHttpActionResult GetSallesParType(string type)
        {
            var salles = db.Salles.Where(s => s.TypeSalle == type).ToList();
            return Ok(salles);
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