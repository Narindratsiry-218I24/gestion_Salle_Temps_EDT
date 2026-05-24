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

        // GET: api/Salle?batiment=A&type=cours&etage=1
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetSalles(string batiment = null, string type = null, int? etage = null)
        {
            var query = db.Salles.AsQueryable();
            if (!string.IsNullOrWhiteSpace(batiment) && batiment.ToLower() != "all")
                query = query.Where(s => s.CodeBatiment == batiment);
            if (!string.IsNullOrWhiteSpace(type) && type.ToLower() != "all")
                query = query.Where(s => s.TypeSalle == type);
            if (etage.HasValue)
                query = query.Where(s => s.Etage == etage.Value);
            return Ok(query.ToList());
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

            if (salle.IdSalle <= 0)
            {
                salle.IdSalle = db.Salles.Any() ? db.Salles.Max(s => s.IdSalle) + 1 : 1;
            }

            db.Salles.Add(salle);
            db.SaveChanges();

            return Ok(salle);
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