# Gestion Salle de Classe & Emploi du Temps - API REST

Cette application est une API REST développée en ASP.NET Web API (EF6) pour gérer les salles, les professeurs, les classes et les emplois du temps.

## 🚀 Installation & Configuration

1. **Restauration des packages** :
   - Ouvrez la solution dans Visual Studio.
   - Faites un clic droit sur la solution > **Restore NuGet Packages**.

2. **Configuration de la Base de Données** :
   - Assurez-vous que PostgreSQL est installé et tourne sur le port `5432`.
   - Modifiez le fichier [`.env`](.env) à la racine du projet avec vos identifiants.

3. **Migrations** :
   - Ouvrez la console du gestionnaire de paquets NuGet.
   - Tapez : `Update-Database` pour créer les tables.

## 📡 Documentation des APIs

Toutes les URLs commencent par `https://localhost:XXXXX/api/`.

### 1. Salles (`/api/Salle`)
| Méthode | Route | Description |
| :--- | :--- | :--- |
| **GET** | `/api/Salle` | Liste toutes les salles |
| **GET** | `/api/Salle/{id}` | Détails d'une salle |
| **POST** | `/api/Salle` | Créer une salle |
| **PUT** | `/api/Salle/{id}` | Modifier une salle |
| **DELETE** | `/api/Salle/{id}` | Supprimer une salle |

### 2. Professeurs (`/api/Professeur`)
| Méthode | Route | Description |
| :--- | :--- | :--- |
| **GET** | `/api/Professeur` | Liste des professeurs |
| **GET** | `/api/Professeur/{id}` | Détails |
| **GET** | `/api/Professeur/{id}/Cours` | Liste des cours d'un prof |

### 3. Cours (`/api/Cours`)
| Méthode | Route | Description |
| :--- | :--- | :--- |
| **GET** | `/api/Cours` | Liste tous les cours |
| **POST** | `/api/Cours/Planifier` | Planifier un nouveau cours |
| **POST** | `/api/Cours/{id}/Valider` | Valider un cours |
| **POST** | `/api/Cours/{id}/Annuler` | Annuler un cours |

### 4. Emploi du Temps (`/api/EDT`)
| Méthode | Route | Description |
| :--- | :--- | :--- |
| **GET** | `/api/EDT/Hebdomadaire` | EDT complet (filtres possibles) |
| **GET** | `/api/EDT/ParSalle/{id}` | EDT d'une salle spécifique |
| **GET** | `/api/EDT/ParClasse/{id}` | EDT d'une classe spécifique |
| **GET** | `/api/EDT/ParProfesseur/{id}` | EDT d'un professeur |

### 5. Utilisateurs & Auth (`/api/Utilisateur`)
| Méthode | Route | Description |
| :--- | :--- | :--- |
| **POST** | `/api/Utilisateur/Login` | Authentification |
| **POST** | `/api/Utilisateur/Register` | Inscription |
| **GET** | `/api/Utilisateur` | Liste des utilisateurs (admin) |

### 6. Dashboard (`/api/Dashboard`)
| Méthode | Route | Description |
| :--- | :--- | :--- |
| **GET** | `/api/Dashboard/Stats` | Statistiques globales |

## 🛠 Outils de Test recommandés
- **Postman** ou **Insomnia** : Pour tester les requêtes POST, PUT et DELETE.
- **Navigateur Web** : Pour tester les requêtes GET rapidement.

---
*Note : Pour les requêtes POST/PUT, n'oubliez pas d'ajouter le header `Content-Type: application/json`.*
