using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gestion_Salle_classe.Models
{
    [Table("filiere")]
    public class Filiere
    {
        [Key]
        [Column("id_filiere")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdFiliere { get; set; }

        [Column("id_mention")]
        public int IdMention { get; set; }

        /// <summary>Parcours parent (surtout pour les filières Master ex: M2I, SDIA)</summary>
        [Column("id_parcours")]
        public int? IdParcours { get; set; }

        [StringLength(20)]
        [Column("code_filiere")]
        public string CodeFiliere { get; set; }

        [StringLength(100)]
        [Column("nom_filiere")]
        public string NomFiliere { get; set; }

        // Navigation properties
        [ForeignKey("IdMention")]
        public virtual Mention Mention { get; set; }

        [ForeignKey("IdParcours")]
        public virtual Parcours Parcours { get; set; }

        public virtual ICollection<Classe> Classes { get; set; }
        public virtual ICollection<Matiere> Matieres { get; set; }
    }
}