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
            var dayOfWeek = DateTime.Today.DayOfWeek;
            string today = "MON";
            switch(dayOfWeek) {
                case DayOfWeek.Monday: today = "MON"; break;
                case DayOfWeek.Tuesday: today = "TUE"; break;
                case DayOfWeek.Wednesday: today = "WED"; break;
                case DayOfWeek.Thursday: today = "THU"; break;
                case DayOfWeek.Friday: today = "FRI"; break;
                default: today = "MON"; break;
            }

            var stats = new
            {
                TotalSalles = db.Salles.Count(),
                TotalProfesseurs = db.Professeurs.Count(),
                TotalClasses = db.Classes.Count(),
                TotalCours = db.Cours.Count(),
                DemandesAttente = db.DemandesEdt.Count(d =>
                    d.Statut == "en_attente" || d.Statut == "en attente" || d.Statut == "pending"),
                OccupancyRate = db.Salles.Any() ? (double)db.Creneaux.Where(c => c.JourSemaine == today).Select(c => c.Cours.IdSalle).Distinct().Count() / db.Salles.Count() * 100 : 0
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