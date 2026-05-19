namespace Gestion_Salle_classe.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "public.annee_academique",
                c => new
                    {
                        id_annee = c.Int(nullable: false, identity: true),
                        libelle = c.String(nullable: false, maxLength: 20),
                        date_debut_annee = c.DateTime(nullable: false),
                        date_fin_annee = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id_annee);
            
            CreateTable(
                "public.semestre",
                c => new
                    {
                        id_semestre = c.Int(nullable: false, identity: true),
                        id_ref_semestre = c.Int(nullable: false),
                        id_annee = c.Int(nullable: false),
                        date_debut = c.DateTime(nullable: false),
                        date_fin = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id_semestre)
                .ForeignKey("public.annee_academique", t => t.id_annee, cascadeDelete: true)
                .ForeignKey("public.ref_semestre", t => t.id_ref_semestre, cascadeDelete: true)
                .Index(t => t.id_ref_semestre)
                .Index(t => t.id_annee);
            
            CreateTable(
                "public.classe",
                c => new
                    {
                        id_classe = c.Int(nullable: false, identity: true),
                        id_filiere = c.Int(nullable: false),
                        id_semestre = c.Int(nullable: false),
                        nom_classe = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.id_classe)
                .ForeignKey("public.filiere", t => t.id_filiere, cascadeDelete: true)
                .ForeignKey("public.semestre", t => t.id_semestre, cascadeDelete: true)
                .Index(t => t.id_filiere)
                .Index(t => t.id_semestre);
            
            CreateTable(
                "public.cours",
                c => new
                    {
                        id_cours = c.Int(nullable: false, identity: true),
                        id_matiere = c.Int(nullable: false),
                        id_professeur = c.Int(nullable: false),
                        id_classe = c.Int(nullable: false),
                        id_salle = c.Int(nullable: false),
                        id_semestre = c.Int(nullable: false),
                        type_cours = c.String(nullable: false, maxLength: 10),
                        statut = c.String(maxLength: 20),
                    })
                .PrimaryKey(t => t.id_cours)
                .ForeignKey("public.classe", t => t.id_classe, cascadeDelete: true)
                .ForeignKey("public.salle", t => t.id_salle, cascadeDelete: true)
                .ForeignKey("public.matiere", t => t.id_matiere, cascadeDelete: true)
                .ForeignKey("public.professeur", t => t.id_professeur, cascadeDelete: true)
                .ForeignKey("public.semestre", t => t.id_semestre, cascadeDelete: true)
                .Index(t => t.id_matiere)
                .Index(t => t.id_professeur)
                .Index(t => t.id_classe)
                .Index(t => t.id_salle)
                .Index(t => t.id_semestre);
            
            CreateTable(
                "public.creneau",
                c => new
                    {
                        id_creneau = c.Int(nullable: false),
                        id_cours = c.Int(nullable: false),
                        jour_semaine = c.String(maxLength: 10),
                        heure_debut = c.Time(nullable: false, precision: 6),
                        heure_fin = c.Time(nullable: false, precision: 6),
                        semaine_type = c.String(maxLength: 1, fixedLength: true),
                    })
                .PrimaryKey(t => t.id_creneau)
                .ForeignKey("public.cours", t => t.id_cours, cascadeDelete: true)
                .Index(t => t.id_cours);
            
            CreateTable(
                "public.demande_edt",
                c => new
                    {
                        id_demande = c.Int(nullable: false, identity: true),
                        id_demandeur = c.Int(nullable: false),
                        id_validateur = c.Int(),
                        id_cours = c.Int(),
                        id_salle = c.Int(),
                        type_demande = c.String(nullable: false, maxLength: 25),
                        statut = c.String(nullable: false, maxLength: 15),
                        justification = c.String(),
                    })
                .PrimaryKey(t => t.id_demande)
                .ForeignKey("public.cours", t => t.id_cours)
                .ForeignKey("public.utilisateur", t => t.id_demandeur, cascadeDelete: true)
                .ForeignKey("public.salle", t => t.id_salle)
                .ForeignKey("public.utilisateur", t => t.id_validateur)
                .Index(t => t.id_demandeur)
                .Index(t => t.id_validateur)
                .Index(t => t.id_cours)
                .Index(t => t.id_salle);
            
            CreateTable(
                "public.utilisateur",
                c => new
                    {
                        id_utilisateur = c.Int(nullable: false, identity: true),
                        nom = c.String(nullable: false, maxLength: 50),
                        prenom = c.String(nullable: false, maxLength: 50),
                        email = c.String(nullable: false, maxLength: 100),
                        role = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.id_utilisateur)
                .Index(t => t.email, unique: true);
            
            CreateTable(
                "public.salle",
                c => new
                    {
                        id_salle = c.Int(nullable: false),
                        code_batiment = c.String(maxLength: 1, fixedLength: true),
                        etage = c.Int(nullable: false),
                        numero_porte = c.String(maxLength: 10),
                        nom_salle = c.String(maxLength: 100),
                        capacite = c.Int(nullable: false),
                        type_salle = c.String(maxLength: 20),
                    })
                .PrimaryKey(t => t.id_salle);
            
            CreateTable(
                "public.matiere",
                c => new
                    {
                        id_matiere = c.Int(nullable: false),
                        id_filiere = c.Int(nullable: false),
                        code_matiere = c.String(maxLength: 20),
                        nom_matiere = c.String(maxLength: 100),
                        id_ref_semestre = c.Int(nullable: false),
                        credit = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id_matiere)
                .ForeignKey("public.filiere", t => t.id_filiere, cascadeDelete: true)
                .ForeignKey("public.ref_semestre", t => t.id_ref_semestre, cascadeDelete: true)
                .Index(t => t.id_filiere)
                .Index(t => t.id_ref_semestre);
            
            CreateTable(
                "public.filiere",
                c => new
                    {
                        id_filiere = c.Int(nullable: false),
                        id_mention = c.Int(nullable: false),
                        code_filiere = c.String(maxLength: 20),
                        nom_filiere = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.id_filiere)
                .ForeignKey("public.mention", t => t.id_mention, cascadeDelete: true)
                .Index(t => t.id_mention);
            
            CreateTable(
                "public.mention",
                c => new
                    {
                        id_mention = c.Int(nullable: false),
                        code_mention = c.String(nullable: false, maxLength: 20),
                        nom_mention = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.id_mention)
                .Index(t => t.code_mention, unique: true);
            
            CreateTable(
                "public.niveau",
                c => new
                    {
                        id_niveau = c.Int(nullable: false, identity: true),
                        code_niveau = c.String(nullable: false, maxLength: 10),
                        ordre = c.Int(nullable: false),
                        id_mention = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id_niveau)
                .ForeignKey("public.mention", t => t.id_mention, cascadeDelete: true)
                .Index(t => t.id_mention);
            
            CreateTable(
                "public.ref_semestre",
                c => new
                    {
                        id_ref_semestre = c.Int(nullable: false, identity: true),
                        code_semestre = c.String(nullable: false, maxLength: 5),
                        ordre = c.Int(nullable: false),
                        id_niveau = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id_ref_semestre)
                .ForeignKey("public.niveau", t => t.id_niveau, cascadeDelete: true)
                .Index(t => t.id_niveau);
            
            CreateTable(
                "public.professeur",
                c => new
                    {
                        id_professeur = c.Int(nullable: false, identity: true),
                        nom = c.String(nullable: false, maxLength: 50),
                        prenom = c.String(nullable: false, maxLength: 50),
                        email = c.String(nullable: false, maxLength: 100),
                        telephone = c.String(maxLength: 20),
                    })
                .PrimaryKey(t => t.id_professeur)
                .Index(t => t.email, unique: true);
            
            CreateTable(
                "public.disponibilite_prof",
                c => new
                    {
                        id_dispo = c.Int(nullable: false, identity: true),
                        id_professeur = c.Int(nullable: false),
                        jour_semaine = c.String(nullable: false, maxLength: 15),
                        heure_debut = c.Time(nullable: false, precision: 6),
                        heure_fin = c.Time(nullable: false, precision: 6),
                        semaine_type = c.String(maxLength: 1, fixedLength: true),
                    })
                .PrimaryKey(t => t.id_dispo)
                .ForeignKey("public.professeur", t => t.id_professeur, cascadeDelete: true)
                .Index(t => t.id_professeur);
            
        }
        
        public override void Down()
        {
            DropForeignKey("public.classe", "id_semestre", "public.semestre");
            DropForeignKey("public.cours", "id_semestre", "public.semestre");
            DropForeignKey("public.disponibilite_prof", "id_professeur", "public.professeur");
            DropForeignKey("public.cours", "id_professeur", "public.professeur");
            DropForeignKey("public.semestre", "id_ref_semestre", "public.ref_semestre");
            DropForeignKey("public.ref_semestre", "id_niveau", "public.niveau");
            DropForeignKey("public.matiere", "id_ref_semestre", "public.ref_semestre");
            DropForeignKey("public.niveau", "id_mention", "public.mention");
            DropForeignKey("public.filiere", "id_mention", "public.mention");
            DropForeignKey("public.matiere", "id_filiere", "public.filiere");
            DropForeignKey("public.classe", "id_filiere", "public.filiere");
            DropForeignKey("public.cours", "id_matiere", "public.matiere");
            DropForeignKey("public.demande_edt", "id_validateur", "public.utilisateur");
            DropForeignKey("public.demande_edt", "id_salle", "public.salle");
            DropForeignKey("public.cours", "id_salle", "public.salle");
            DropForeignKey("public.demande_edt", "id_demandeur", "public.utilisateur");
            DropForeignKey("public.demande_edt", "id_cours", "public.cours");
            DropForeignKey("public.creneau", "id_cours", "public.cours");
            DropForeignKey("public.cours", "id_classe", "public.classe");
            DropForeignKey("public.semestre", "id_annee", "public.annee_academique");
            DropIndex("public.disponibilite_prof", new[] { "id_professeur" });
            DropIndex("public.professeur", new[] { "email" });
            DropIndex("public.ref_semestre", new[] { "id_niveau" });
            DropIndex("public.niveau", new[] { "id_mention" });
            DropIndex("public.mention", new[] { "code_mention" });
            DropIndex("public.filiere", new[] { "id_mention" });
            DropIndex("public.matiere", new[] { "id_ref_semestre" });
            DropIndex("public.matiere", new[] { "id_filiere" });
            DropIndex("public.utilisateur", new[] { "email" });
            DropIndex("public.demande_edt", new[] { "id_salle" });
            DropIndex("public.demande_edt", new[] { "id_cours" });
            DropIndex("public.demande_edt", new[] { "id_validateur" });
            DropIndex("public.demande_edt", new[] { "id_demandeur" });
            DropIndex("public.creneau", new[] { "id_cours" });
            DropIndex("public.cours", new[] { "id_semestre" });
            DropIndex("public.cours", new[] { "id_salle" });
            DropIndex("public.cours", new[] { "id_classe" });
            DropIndex("public.cours", new[] { "id_professeur" });
            DropIndex("public.cours", new[] { "id_matiere" });
            DropIndex("public.classe", new[] { "id_semestre" });
            DropIndex("public.classe", new[] { "id_filiere" });
            DropIndex("public.semestre", new[] { "id_annee" });
            DropIndex("public.semestre", new[] { "id_ref_semestre" });
            DropTable("public.disponibilite_prof");
            DropTable("public.professeur");
            DropTable("public.ref_semestre");
            DropTable("public.niveau");
            DropTable("public.mention");
            DropTable("public.filiere");
            DropTable("public.matiere");
            DropTable("public.salle");
            DropTable("public.utilisateur");
            DropTable("public.demande_edt");
            DropTable("public.creneau");
            DropTable("public.cours");
            DropTable("public.classe");
            DropTable("public.semestre");
            DropTable("public.annee_academique");
        }
    }
}
