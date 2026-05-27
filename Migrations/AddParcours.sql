-- ============================================================
-- Migration : Ajout de la table PARCOURS
-- À exécuter dans PostgreSQL sur la base EMIT_EDT_DB
-- ============================================================

-- 1. Création de la table parcours
CREATE TABLE IF NOT EXISTS public.parcours (
    id_parcours   SERIAL PRIMARY KEY,
    code_parcours VARCHAR(20)  NOT NULL,
    nom_parcours  VARCHAR(100) NOT NULL,
    cycle         VARCHAR(20)  NOT NULL CHECK (cycle IN ('Licence', 'Master')),
    id_mention    INTEGER      NOT NULL REFERENCES public.mention(id_mention) ON DELETE CASCADE
);

-- 2. Ajout de la colonne id_parcours dans filiere (nullable)
ALTER TABLE public.filiere
    ADD COLUMN IF NOT EXISTS id_parcours INTEGER REFERENCES public.parcours(id_parcours) ON DELETE SET NULL;

-- ============================================================
-- Données initiales — Mentions (si elles n'existent pas)
-- ============================================================
INSERT INTO public.mention (id_mention, code_mention, nom_mention) VALUES
  (1, 'INFO',  'Informatique'),
  (2, 'MGT',   'Management'),
  (3, 'MULTI', 'Multi Média')
ON CONFLICT (code_mention) DO NOTHING;

-- ============================================================
-- Données initiales — Parcours LICENCE
-- ============================================================
INSERT INTO public.parcours (code_parcours, nom_parcours, cycle, id_mention) VALUES
  ('DA2I',  'Développement d''Applications et Intégration Informatique', 'Licence', 1),
  ('AES',   'Administration Économique et Sociale',                        'Licence', 2),
  ('ICM',   'Ingénierie en Communication Multimédia',                      'Licence', 3)
ON CONFLICT DO NOTHING;

-- ============================================================
-- Données initiales — Parcours / Filières MASTER
-- ============================================================
-- Mention Informatique → M2I, SDIA, SIGD
INSERT INTO public.parcours (code_parcours, nom_parcours, cycle, id_mention) VALUES
  ('M2I',  'Maîtrise en Informatique et Ingénierie',     'Master', 1),
  ('SDIA', 'Systèmes de Données et Intelligence Artificielle', 'Master', 1),
  ('SIGD', 'Systèmes d''Information et Génie des Données', 'Master', 1)
ON CONFLICT DO NOTHING;

-- Mention Multi Média → RPOO, OPR, ICO
INSERT INTO public.parcours (code_parcours, nom_parcours, cycle, id_mention) VALUES
  ('RPOO', 'Réseaux et Programmation Orientée Objet',  'Master', 3),
  ('OPR',  'Optimisation et Projet de Recherche',       'Master', 3),
  ('ICO',  'Image, Communication et Organisation',      'Master', 3)
ON CONFLICT DO NOTHING;

-- Mention Management → MAE, MAA, MEE
INSERT INTO public.parcours (code_parcours, nom_parcours, cycle, id_mention) VALUES
  ('MAE', 'Management et Administration des Entreprises', 'Master', 2),
  ('MAA', 'Management, Audit et Accompagnement',          'Master', 2),
  ('MEE', 'Management de l''Économie des Entreprises',    'Master', 2)
ON CONFLICT DO NOTHING;

-- ============================================================
-- Niveaux (L1-L3 / M1-M2) liés aux mentions
-- ============================================================
INSERT INTO public.niveau (code_niveau, ordre, id_mention) VALUES
  ('L1', 1, 1), ('L2', 2, 1), ('L3', 3, 1),
  ('M1', 4, 1), ('M2', 5, 1),
  ('L1', 1, 2), ('L2', 2, 2), ('L3', 3, 2),
  ('M1', 4, 2), ('M2', 5, 2),
  ('L1', 1, 3), ('L2', 2, 3), ('L3', 3, 3),
  ('M1', 4, 3), ('M2', 5, 3)
ON CONFLICT DO NOTHING;
