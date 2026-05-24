using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gestion_Salle_classe.Models
{
    [Table("demande_edt")]
    public class DemandeEdt
    {
        [Key]
        [Column("id_demande")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdDemande { get; set; }

        [Column("id_demandeur")]
        public int IdDemandeur { get; set; }

        [Column("id_validateur")]
        public int? IdValidateur { get; set; }

        [Column("id_cours")]
        public int? IdCours { get; set; }

        [Column("id_salle")]
        public int? IdSalle { get; set; }

        [Required]
        [StringLength(25)]
        [Column("type_demande")]
        public string TypeDemande { get; set; }

        [Required]
        [StringLength(15)]
        [Column("statut")]
        public string Statut { get; set; }

        [Column("justification")]
        public string Justification { get; set; }

        // Navigation properties
        [ForeignKey("IdDemandeur")]
        public virtual Utilisateur Demandeur { get; set; }

        [ForeignKey("IdValidateur")]
        public virtual Utilisateur Validateur { get; set; }

        [ForeignKey("IdCours")]
        public virtual Cours Cours { get; set; }

        [ForeignKey("IdSalle")]
        public virtual Salle Salle { get; set; }
    }
}
