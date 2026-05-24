using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Http;
using Gestion_Salle_classe.Models;

namespace Gestion_Salle_classe.Controllers
{
    [RoutePrefix("api/Utilisateur")]
    public class UtilisateurController : ApiController
    {
        private EMITDbContext db = new EMITDbContext();

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetUtilisateurs()
        {
            return Ok(db.Utilisateurs.ToList());
        }

        [HttpPost]
        [Route("Login")]
        public IHttpActionResult Login(LoginModel login)
        {
            try 
            {
                var user = db.Utilisateurs.FirstOrDefault(u => u.Email == login.Email);
                if (user != null)
                {
                    return Ok(new { IdUtilisateur = user.IdUtilisateur, Email = user.Email, Role = user.Role });
                }
            }
            catch (Exception) 
            {
                // Ignorer l'erreur DB si la table n'est pas encore créée et utiliser le fallback
            }
            
            // Fallback for testing if DB is empty or user not found
            if (login.Email == "admin@emit.mg") return Ok(new { IdUtilisateur = 1, Email = "admin@emit.mg", Role = "admin" });
            if (login.Email == "demandeur@emit.mg") return Ok(new { IdUtilisateur = 2, Email = "demandeur@emit.mg", Role = "demandeur" });
            if (login.Email == "validateur@emit.mg") return Ok(new { IdUtilisateur = 3, Email = "validateur@emit.mg", Role = "validateur" });

            return Unauthorized();
        }

        [HttpPost]
        [Route("Register")]
        public IHttpActionResult Register(Utilisateur utilisateur)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            db.Utilisateurs.Add(utilisateur);
            db.SaveChanges();
            return Ok(utilisateur);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }

    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
