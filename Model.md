# 📊 MODÈLE DE DONNÉES - GESTION EDT & SALLES

## Liste des tables (15 tables)

---

### 1. MENTION
| Attribut | Type | Contrainte |
|----------|------|-------------|
| id_mention | INT | PRIMARY KEY |
| code_mention | VARCHAR(20) | NOT NULL |
| nom_mention | VARCHAR(100) | NOT NULL |

**Relations :**
- 1,N avec FILIERE (une mention a plusieurs filières)
- 1,N avec NIVEAU (une mention a plusieurs niveaux)

---

### 2. FILIERE
| Attribut | Type | Contrainte |
|----------|------|-------------|
| id_filiere | INT | PRIMARY KEY |
| id_mention | INT | FOREIGN KEY → MENTION(id_mention) |
| code_filiere | VARCHAR(20) | NOT NULL |
| nom_filiere | VARCHAR(100) | NOT NULL |

**Relations :**
- N,1 avec MENTION
- 1,N avec CLASSE
- 1,N avec MATIERE

---

### 3. NIVEAU
| Attribut | Type | Contrainte |
|----------|------|-------------|
| id_niveau | INT | PRIMARY KEY |
| code_niveau | VARCHAR(10) | NOT NULL (L1, L2, L3, M1, M2) |
| ordre | INT | NOT NULL |
| id_mention | INT | FOREIGN KEY → MENTION(id_mention) |

**Relations :**
- N,1 avec MENTION
- 1,N avec REF_SEMESTRE

---

### 4. REF_SEMESTRE (définition fixe)
| Attribut | Type | Contrainte |
|----------|------|-------------|
| id_ref_semestre | INT | PRIMARY KEY |
| code_semestre | VARCHAR(5) | NOT NULL (S1, S2, S3...) |
| ordre | INT | NOT NULL |
| id_niveau | INT | FOREIGN KEY → NIVEAU(id_niveau) |

**Relations :**
- N,1 avec NIVEAU
- 1,N avec SEMESTRE
- 1,N avec MATIERE

---

### 5. ANNEE_ACADEMIQUE
| Attribut | Type | Contrainte |
|----------|------|-------------|
| id_annee | INT | PRIMARY KEY |
| libelle | VARCHAR(20) | NOT NULL (ex: 2024-2025) |
| date_debut_annee | DATE | NOT NULL |
| date_fin_annee | DATE | NOT NULL |

**Relations :**
- 1,N avec SEMESTRE

---

### 6. SEMESTRE (instance annuelle)
| Attribut | Type | Contrainte |
|----------|------|-------------|
| id_semestre | INT | PRIMARY KEY |
| id_ref_semestre | INT | FOREIGN KEY → REF_SEMESTRE(id_ref_semestre) |
| id_annee | INT | FOREIGN KEY → ANNEE_ACADEMIQUE(id_annee) |
| date_debut | DATE | NOT NULL |
| date_fin | DATE | NOT NULL |

**Relations :**
- N,1 avec REF_SEMESTRE
- N,1 avec ANNEE_ACADEMIQUE
- 1,N avec CLASSE
- 1,N avec COURS

---

### 7. CLASSE
| Attribut | Type | Contrainte |
|----------|------|-------------|
| id_classe | INT | PRIMARY KEY |
| id_filiere | INT | FOREIGN KEY → FILIERE(id_filiere) |
| id_semestre | INT | FOREIGN KEY → SEMESTRE(id_semestre) |
| nom_classe | VARCHAR(100) | NOT NULL |

**Relations :**
- N,1 avec FILIERE
- N,1 avec SEMESTRE
- 1,N avec COURS

---

### 8. MATIERE
| Attribut | Type | Contrainte |
|----------|------|-------------|
| id_matiere | INT | PRIMARY KEY |
| id_filiere | INT | FOREIGN KEY → FILIERE(id_filiere) |
| code_matiere | VARCHAR(20) | NOT NULL |
| nom_matiere | VARCHAR(100) | NOT NULL |
| id_ref_semestre | INT | FOREIGN KEY → REF_SEMESTRE(id_ref_semestre) |
| credit | INT | NOT NULL |

**Relations :**
- N,1 avec FILIERE
- N,1 avec REF_SEMESTRE
- 1,N avec COURS

---

### 9. PROFESSEUR
| Attribut | Type | Contrainte |
|----------|------|-------------|
| id_professeur | INT | PRIMARY KEY |
| nom | VARCHAR(50) | NOT NULL |
| prenom | VARCHAR(50) | NOT NULL |
| email | VARCHAR(100) | NOT NULL, UNIQUE |
| telephone | VARCHAR(20) | |

**Relations :**
- 1,N avec DISPONIBILITE_PROF
- 1,N avec COURS

---

### 10. SALLE
| Attribut | Type | Contrainte |
|----------|------|-------------|
| id_salle | INT | PRIMARY KEY |
| code_batiment | CHAR(1) | NOT NULL (A, B, C, D) |
| etage | INT | NOT NULL |
| numero_porte | VARCHAR(10) | NOT NULL |
| nom_salle | VARCHAR(100) | |
| capacite | INT | NOT NULL |
| type_salle | VARCHAR(20) | (cours, td, tp, amphi, labo) |

**Relations :**
- 1,N avec COURS
- 1,N avec DEMANDE_EDT

---

### 11. DISPONIBILITE_PROF
| Attribut | Type | Contrainte |
|----------|------|-------------|
| id_dispo | INT | PRIMARY KEY |
| id_professeur | INT | FOREIGN KEY → PROFESSEUR(id_professeur) |
| jour_semaine | VARCHAR(15) | NOT NULL (Lundi, Mardi...) |
| heure_debut | TIME | NOT NULL |
| heure_fin | TIME | NOT NULL |
| semaine_type | CHAR(1) | (A ou B) |

**Relations :**
- N,1 avec PROFESSEUR

---

### 12. COURS
| Attribut | Type | Contrainte |
|----------|------|-------------|
| id_cours | INT | PRIMARY KEY |
| id_matiere | INT | FOREIGN KEY → MATIERE(id_matiere) |
| id_professeur | INT | FOREIGN KEY → PROFESSEUR(id_professeur) |
| id_classe | INT | FOREIGN KEY → CLASSE(id_classe) |
| id_salle | INT | FOREIGN KEY → SALLE(id_salle) |
| id_semestre | INT | FOREIGN KEY → SEMESTRE(id_semestre) |
| type_cours | VARCHAR(10) | NOT NULL (cours, td, tp, examen) |
| statut | VARCHAR(20) | (planifié, validé, annulé) |

**Relations :**
- N,1 avec MATIERE
- N,1 avec PROFESSEUR
- N,1 avec CLASSE
- N,1 avec SALLE
- N,1 avec SEMESTRE
- 1,N avec CRENEAU
- 1,N avec DEMANDE_EDT

---

### 13. CRENEAU
| Attribut | Type | Contrainte |
|----------|------|-------------|
| id_creneau | INT | PRIMARY KEY |
| id_cours | INT | FOREIGN KEY → COURS(id_cours) |
| jour_semaine | VARCHAR(15) | NOT NULL |
| heure_debut | TIME | NOT NULL |
| heure_fin | TIME | NOT NULL |
| semaine_type | CHAR(1) | (A ou B) |

**Relations :**
- N,1 avec COURS

---

### 14. UTILISATEUR
| Attribut | Type | Contrainte |
|----------|------|-------------|
| id_utilisateur | INT | PRIMARY KEY |
| nom | VARCHAR(50) | NOT NULL |
| prenom | VARCHAR(50) | NOT NULL |
| email | VARCHAR(100) | NOT NULL, UNIQUE |
| role | VARCHAR(20) | NOT NULL (admin, demandeur, validateur) |

**Relations :**
- 1,N avec DEMANDE_EDT (comme demandeur)
- 0,N avec DEMANDE_EDT (comme validateur)

---

### 15. DEMANDE_EDT
| Attribut | Type | Contrainte |
|----------|------|-------------|
| id_demande | INT | PRIMARY KEY |
| id_demandeur | INT | FOREIGN KEY → UTILISATEUR(id_utilisateur) |
| id_validateur | INT | FOREIGN KEY → UTILISATEUR(id_utilisateur) |
| id_cours | INT | FOREIGN KEY → COURS(id_cours), NULLABLE |
| id_salle | INT | FOREIGN KEY → SALLE(id_salle), NULLABLE |
| type_demande | VARCHAR(25) | NOT NULL (modification_cours, reservation_salle) |
| statut | VARCHAR(15) | NOT NULL (en_attente, validée, refusée) |
| justification | TEXT | |

**Relations :**
- N,1 avec UTILISATEUR (demandeur)
- N,1 avec UTILISATEUR (validateur)
- N,1 avec COURS
- N,1 avec SALLE

---

## 📊 Graphe des relations
