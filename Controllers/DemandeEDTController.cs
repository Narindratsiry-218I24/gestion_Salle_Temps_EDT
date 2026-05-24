using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Http;
using Gestion_Salle_classe.Models;
using Gestion_Salle_classe.Helpers;

namespace Gestion_Salle_classe.Controllers
{
    [RoutePrefix("api/DemandeEDT")]
    public class DemandeEDTController : ApiController
    {
        private EMITDbContext db = new EMITDbContext();

        private static bool IsPending(string statut)
        {
            if (string.IsNullOrEmpty(statut)) return false;
            var s = statut.Replace(" ", "_").ToLowerInvariant();
            return s == "en_attente" || s == "pending";
        }

        private static string NormalizeStatut(string statut)
        {
            if (string.IsNullOrEmpty(statut)) return "en_attente";
            var s = statut.Replace(" ", "_").ToLowerInvariant();
            if (s == "en_attente" || s == "pending") return "en_attente";
            if (s == "validée" || s == "validee" || s == "approved") return "validée";
            if (s == "refusée" || s == "refusee" || s == "rejected") return "refusée";
            return statut;
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetDemandes()
        {
            var ctx = UserContextHelper.FromRequest(ActionContext);
            var query = db.DemandesEdt
                .Include(d => d.Demandeur)
                .Include(d => d.Cours.Matiere)
                .Include(d => d.Salle)
                .AsQueryable();

            if (ctx.IsAuthenticated)
            {
                if (ctx.Role == "demandeur")
                    query = query.Where(d => d.IdDemandeur == ctx.UserId.Value);
                else if (ctx.Role == "validateur")
                    query = query.Where(d => IsPending(d.Statut));
            }

            return Ok(query.ToList());
        }

        [HttpPost]
        [Route("Creer")]
        public IHttpActionResult CreerDemande(DemandeEdt demande)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            
            try
            {
                // Force a valid status if somehow it's null
                demande.Statut = NormalizeStatut(demande.Statut);

                db.DemandesEdt.Add(demande);
                db.SaveChanges();
                return CreatedAtRoute("DefaultApi", new { id = demande.IdDemande }, demande);
            }
            catch (Exception ex)
            {
                var innerMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                if (ex.InnerException?.InnerException != null) {
                    innerMessage += " | " + ex.InnerException.InnerException.Message;
                }
                // Return 400 with the exact error so axios doesn't crash silently
                return Content(HttpStatusCode.BadRequest, new { Message = "DB Error: " + innerMessage });
            }
        }

        [HttpPost]
        [Route("{id:int}/Valider")]
        public IHttpActionResult ValiderDemande(int id)
        {
            var demande = db.DemandesEdt.Include(d => d.Cours.Creneaux).FirstOrDefault(d => d.IdDemande == id);
            if (demande == null) return NotFound();

            // Vérifier les conflits de salle
            if (demande.IdSalle.HasValue && demande.Cours != null && demande.Cours.Creneaux != null)
            {
                foreach (var creneau in demande.Cours.Creneaux)
                {
                    bool conflit = db.Creneaux.Any(c => 
                        c.Cours.IdSalle == demande.IdSalle.Value &&
                        c.JourSemaine == creneau.JourSemaine &&
                        c.IdCours != demande.IdCours &&
                        ((creneau.HeureDebut >= c.HeureDebut && creneau.HeureDebut < c.HeureFin) ||
                         (creneau.HeureFin > c.HeureDebut && creneau.HeureFin <= c.HeureFin) ||
                         (creneau.HeureDebut <= c.HeureDebut && creneau.HeureFin >= c.HeureFin))
                    );

                    if (conflit)
                    {
                        return BadRequest("Conflit de salle détecté pour ce créneau.");
                    }
                }
                
                // Mettre à jour la salle du cours
                demande.Cours.IdSalle = demande.IdSalle.Value;
            }

            var ctx = UserContextHelper.FromRequest(ActionContext);
            if (ctx.IsAuthenticated && ctx.Role == "validateur")
                demande.IdValidateur = ctx.UserId;

            demande.Statut = "validée";
            db.SaveChanges();
            return Ok(demande);
        }

        [HttpPost]
        [Route("{id:int}/Refuser")]
        public IHttpActionResult RefuserDemande(int id)
        {
            var demande = db.DemandesEdt.Find(id);
            if (demande == null) return NotFound();

            var ctx = UserContextHelper.FromRequest(ActionContext);
            if (ctx.IsAuthenticated && ctx.Role == "validateur")
                demande.IdValidateur = ctx.UserId;

            demande.Statut = "refusée";
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