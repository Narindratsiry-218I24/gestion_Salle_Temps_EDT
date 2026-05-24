using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Gestion_Salle_classe.Models;

namespace Gestion_Salle_classe.Controllers
{
    [RoutePrefix("api/Dashboard")]
    public class DashboardController : ApiController
    {
        private EMITDbContext db = new EMITDbContext();

        [HttpGet]
        [Route("Stats")]
        public IHttpActionResult GetStats()
        {
            var stats = new
            {
                TotalSalles = db.Salles.Count(),
                TotalProfesseurs = db.Professeurs.Count(),
                TotalClasses = db.Classes.Count(),
                TotalCours = db.Cours.Count(),
                DemandesAttente = db.DemandesEdt.Count(d =>
                    d.Statut == "en_attente" || d.Statut == "en attente" || d.Statut == "pending")
            };

            return Ok(stats);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}