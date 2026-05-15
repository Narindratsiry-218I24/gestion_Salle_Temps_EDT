using System;
using System.Data.Entity;
using System.Linq;

namespace Gestion_Salle_classe.Models
{
    public class EMITDbContext : DbContext
    {
        public EMITDbContext() : base("name=EMITDbContext")
        {
        }

        public DbSet<Mention> Mentions { get; set; }
        public DbSet<Filiere> Filieres { get; set; }
        public DbSet<Niveau> Niveaux { get; set; }
        public DbSet<RefSemestre> RefSemestres { get; set; }
        public DbSet<AnneeAcademique> AnneesAcademiques { get; set; }
        public DbSet<Semestre> Semestres { get; set; }
        public DbSet<Classe> Classes { get; set; }
        public DbSet<Matiere> Matieres { get; set; }
        public DbSet<Professeur> Professeurs { get; set; }
        public DbSet<Salle> Salles { get; set; }
        public DbSet<DisponibiliteProf> DisponibilitesProf { get; set; }
        public DbSet<Cours> Cours { get; set; }
        public DbSet<Creneau> Creneaux { get; set; }
        public DbSet<Utilisateur> Utilisateurs { get; set; }
        public DbSet<DemandeEdt> DemandesEdt { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Default schema for PostgreSQL is usually "public"
            modelBuilder.HasDefaultSchema("public");

            // Additional configurations if needed
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
