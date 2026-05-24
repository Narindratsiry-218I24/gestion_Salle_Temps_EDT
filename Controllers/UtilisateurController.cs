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
            if (login == null || string.IsNullOrEmpty(login.Email))
            {
                return BadRequest("Email requis.");
            }

            try 
            {
                var user = db.Utilisateurs.FirstOrDefault(u => u.Email.ToLower() == login.Email.ToLower());
                if (user != null)
                {
                    return Ok(new {
                        IdUtilisateur = user.IdUtilisateur,
                        Nom = user.Nom,
                        Prenom = user.Prenom,
                        Email = user.Email,
                        Role = user.Role
                    });
                }
            }
            catch (Exception ex) 
            {
                // Log l'erreur si nécessaire : System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            
            // Fallback for testing - Ireto no kaonty azonao ampiasaina avy hatrany
            if (login.Email.ToLower() == "admin@emit.mg") 
                return Ok(new { IdUtilisateur = 1, Nom = "Admin", Prenom = "EMIT", Email = "admin@emit.mg", Role = "admin" });
            
            if (login.Email.ToLower() == "demandeur@emit.mg") 
                return Ok(new { IdUtilisateur = 2, Nom = "Demandeur", Prenom = "Test", Email = "demandeur@emit.mg", Role = "demandeur" });
            
            if (login.Email.ToLower() == "validateur@emit.mg") 
                return Ok(new { IdUtilisateur = 3, Nom = "Validateur", Prenom = "Test", Email = "validateur@emit.mg", Role = "validateur" });

            return Unauthorized();
        }

        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetUtilisateur(int id)
        {
            var user = db.Utilisateurs.Find(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPost]
        [Route("Register")]
        public IHttpActionResult Register(Utilisateur utilisateur)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (db.Utilisateurs.Any(u => u.Email == utilisateur.Email))
                return BadRequest("Cet email est déjà utilisé.");
            db.Utilisateurs.Add(utilisateur);
            db.SaveChanges();
            return Ok(utilisateur);
        }

        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult UpdateUtilisateur(int id, Utilisateur utilisateur)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (id != utilisateur.IdUtilisateur) return BadRequest();

            var existing = db.Utilisateurs.Find(id);
            if (existing == null) return NotFound();

            existing.Nom = utilisateur.Nom;
            existing.Prenom = utilisateur.Prenom;
            existing.Email = utilisateur.Email;
            existing.Role = utilisateur.Role;
            db.SaveChanges();
            return Ok(existing);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult DeleteUtilisateur(int id)
        {
            var user = db.Utilisateurs.Find(id);
            if (user == null) return NotFound();
            db.Utilisateurs.Remove(user);
            db.SaveChanges();
            return Ok(user);
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
