using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Http;
using Gestion_Salle_classe.Models;

namespace Gestion_Salle_classe.Controllers
{
    [RoutePrefix("api/DemandeEDT")]
    public class DemandeEDTController : ApiController
    {
        private EMITDbContext db = new EMITDbContext();

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetDemandes()
        {
            var demandes = db.DemandesEdt.Include(d => d.Demandeur).Include(d => d.Cours).ToList();
            return Ok(demandes);
        }

        [HttpPost]
        [Route("Creer")]
        public IHttpActionResult CreerDemande(DemandeEdt demande)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            db.DemandesEdt.Add(demande);
            db.SaveChanges();
            return CreatedAtRoute("DefaultApi", new { id = demande.IdDemande }, demande);
        }

        [HttpPost]
        [Route("{id:int}/Valider")]
        public IHttpActionResult ValiderDemande(int id)
        {
            var demande = db.DemandesEdt.Find(id);
            if (demande == null) return NotFound();
            demande.Statut = "validée";
            db.SaveChanges();
            return Ok(demande);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}