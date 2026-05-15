using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gestion_Salle_classe.Models
{
    [Table("salle")]
    public class Salle
    {
        [Key]
        [Column("id_salle")]
        public int IdSalle { get; set; }

        [Required]
        [Column("code_batiment", TypeName = "char")]
        [StringLength(1)]
        public string CodeBatiment { get; set; }

        [Required]
        [Column("etage")]
        public int Etage { get; set; }

        [Required]
        [StringLength(10)]
        [Column("numero_porte")]
        public string NumeroPorte { get; set; }

        [StringLength(100)]
        [Column("nom_salle")]
        public string NomSalle { get; set; }

        [Required]
        [Column("capacite")]
        public int Capacite { get; set; }

        [StringLength(20)]
        [Column("type_salle")]
        public string TypeSalle { get; set; }

        // Navigation properties
        public virtual ICollection<Cours> Cours { get; set; }
        public virtual ICollection<DemandeEdt> DemandesEdt { get; set; }
    }
}
