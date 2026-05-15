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
            var user = db.Utilisateurs.FirstOrDefault(u => u.Email == login.Email);
            if (user != null)
            {
                // In a real API, return a JWT token here
                return Ok(new { user.IdUtilisateur, user.Email, user.Role });
            }
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
