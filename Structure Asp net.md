# 📁 STRUCTURE D'UN PROJET ASP.NET MVC - GESTION EDT & SALLES

## 🎯 Objectif de ce document

Ce README explique :
- La **structure des dossiers** d’un projet ASP.NET MVC (.NET Framework)
- Le **rôle de chaque dossier et fichier**
- Les **étapes pour créer** cette structure

✅ Aucun code complexe.  
✅ Explication claire pour débutant ou pour documentation.

---

## 🧱 Structure générale du projet

---

## 📘 Explication détaillée (dossier par dossier)

### 1. `App_Data`
| Rôle | Exemple |
|------|---------|
| Stocker les fichiers de base de données locale | `EMIT_EDT.mdf` |
| Fichiers XML, JSON ou CSV utilisés par l’app | `salles.json` |
| ⚠️ Dossier sécurisé : invisible dans le navigateur |

---

### 2. `App_Start`
| Fichier | Rôle |
|---------|------|
| `RouteConfig.cs` | Définit les URLs (ex: `/Salle/Index`) |
| `BundleConfig.cs` | Regroupe les fichiers CSS et JS |
| `FilterConfig.cs` | Gère les filtres de sécurité |
| `IdentityConfig.cs` | Gère l’authentification (si utilisée) |

---

### 3. `Areas`
| Rôle | Exemple |
|------|---------|
| Découper le projet en sous-modules indépendants | `Areas/Admin` = gestion des salles |
| Chaque zone a ses propres **Controllers**, **Models**, **Views** | `Areas/Etudiant/Views/EDT.cshtml` |

---

### 4. `bin`
| Rôle | À savoir |
|------|----------|
| Contient les fichiers compilés (.dll, .exe) | Généré automatiquement |
| ❌ Ne pas modifier manuellement | ❌ Ne pas versionner (gitignore) |

---

### 5. `Content`
| Rôle | Exemple |
|------|---------|
| Fichiers statiques : CSS, images, polices | `site.css`, `bootstrap.css`, `logo.png` |

---

### 6. `Controllers`
| Rôle | Exemple |
|------|---------|
| Contient la logique métier (C#) | `SalleController.cs`, `CoursController.cs` |
| Une action = une page ou une API | `Index()`, `Ajouter()`, `Modifier()` |

---

### 7. `fonts`
| Rôle | Exemple |
|------|---------|
| Polices personnalisées | FontAwesome, icônes Bootstrap |

---

### 8. `Models`
| Rôle | Exemple |
|------|---------|
| Représente les données (classes C#) | `Salle.cs`, `Cours.cs`, `Utilisateur.cs` |
| Contient le DbContext (Entity Framework) | `EMITDbContext.cs` |

---

### 9. `obj`
| Rôle | À savoir |
|------|----------|
| Fichiers temporaires de compilation | Généré automatiquement |
| ❌ Ne pas modifier | ❌ Ne pas versionner |

---

### 10. `packages`
| Rôle | Exemple |
|------|---------|
| Contient les packages NuGet installés | EntityFramework, Bootstrap, jQuery |

---

### 11. `Properties`
| Rôle | Exemple |
|------|---------|
| Configuration du projet Visual Studio | `AssemblyInfo.cs` (version, description) |

---

### 12. `Scripts`
| Rôle | Exemple |
|------|---------|
| Fichiers JavaScript et TypeScript | `jquery.js`, `bootstrap.js`, `edt.js` |

---

### 13. `Views`
| Rôle | Exemple |
|------|---------|
| Pages HTML mélangées avec C# (Razor) | `Views/Salle/Index.cshtml` |
| Organisation : 1 dossier par Controller | `Views/EDT/Consulter.cshtml` |

---

## 📄 Fichiers à la racine

| Fichier | Rôle |
|---------|------|
| `favicon.ico` | Icône du site (dans l’onglet navigateur) |
| `Global.asax` | Point d’entrée de l’application |
| `Global.asax.cs` | Code derrière (démarrage, événements) |
| `packages.config` | Liste des packages NuGet utilisés |
| `Web.config` | Configuration principale (base de données, sessions, sécurité) |
| `Web.Debug.config` | Configuration spécifique au mode Debug |
| `Web.Release.config` | Configuration spécifique au mode Release |
| `Gestion_Salle_classe.csproj` | Fichier projet Visual Studio |
| `Gestion_Salle_classe.csproj.user` | Configuration utilisateur du projet |
| `Gestion_Salle_classe.sln` | Fichier solution Visual Studio |

---

## 🧭 Étapes pour créer cette structure (sans code)

### Étape 1 – Créer le projet
- Ouvrir **Visual Studio**
- `Nouveau projet` → `ASP.NET Web Application (.NET Framework)`
- Choisir `MVC`
- Nommer : `Gestion_Salle_classe`
- Cliquer sur `Créer`

### Étape 2 – Structure automatique
Visual Studio crée automatiquement :





---

## 📌 Remarques importantes

- Ne jamais modifier `bin/`, `obj/` ou `packages/` manuellement
- Toujours versionner les dossiers `Controllers/`, `Models/`, `Views/`, `Content/`, `Scripts/`
- Le fichier `Web.config` contient la connexion à la base de données
- Un projet ASP.NET MVC suit l’architecture **Modèle-Vue-Contrôleur**

---

## 🧪 Exemple d’organisation pour votre projet EDT

| Dossier | Contenu lié à votre projet |
|---------|----------------------------|
| `Models/` | Mention.cs, Filiere.cs, Semestre.cs, Cours.cs, Salle.cs... |
| `Controllers/` | EDTController.cs, SalleController.cs, DemandeController.cs |
| `Views/EDT/` | Consulter.cshtml, Planifier.cshtml |
| `Views/Salle/` | Liste.cshtml, Ajouter.cshtml, Modifier.cshtml |
| `Views/Demande/` | Valider.cshtml, Refuser.cshtml |

---

## 🔁 Prochaine étape suggérée

1. Créer ce projet dans Visual Studio
2. Ajouter vos 15 classes dans `Models/`
3. Ajouter `DbContext` et chaîne de connexion
4. Créer les contrôleurs
5. Tester l’affichage des pages

> 📄 Ce fichier README peut être utilisé tel quel dans votre dépôt GitHub ou comme documentation interne.