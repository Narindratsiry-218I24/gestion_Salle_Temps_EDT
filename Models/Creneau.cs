using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gestion_Salle_classe.Models
{
    [Table("creneau")]
    public class Creneau
    {
        [Key]
        [Column("id_creneau")]
        public int IdCreneau { get; set; }

        [Column("id_cours")]
        public int IdCours { get; set; }

        [Required]
        [StringLength(15)]
        [Column("jour_semaine")]
        public string JourSemaine { get; set; }

        [Required]
        [Column("heure_debut")]
        public TimeSpan HeureDebut { get; set; }

        [Required]
        [Column("heure_fin")]
        public TimeSpan HeureFin { get; set; }

        [Column("semaine_type", TypeName = "char")]
        [StringLength(1)]
        public string SemaineType { get; set; }

        // Navigation properties
        [ForeignKey("IdCours")]
        public virtual Cours Cours { get; set; }
    }
}
