using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gestion_Salle_classe.Models
{
    [Table("parcours")]
    public class Parcours
    {
        [Key]
        [Column("id_parcours")]
        public int IdParcours { get; set; }

        [Required]
        [StringLength(20)]
        [Column("code_parcours")]
        public string CodeParcours { get; set; }

        [Required]
        [StringLength(100)]
        [Column("nom_parcours")]
        public string NomParcours { get; set; }

        /// <summary>
        /// Cycle : "Licence" ou "Master"
        /// </summary>
        [Required]
        [StringLength(20)]
        [Column("cycle")]
        public string Cycle { get; set; }

        [Column("id_mention")]
        public int IdMention { get; set; }

        // Navigation properties
        [ForeignKey("IdMention")]
        public virtual Mention Mention { get; set; }

        /// <summary>
        /// Niveaux associés à ce parcours (ex: L1, L2, L3 pour Licence ou M1, M2 pour Master)
        /// </summary>
        public virtual ICollection<Filiere> Filieres { get; set; }
    }
}
