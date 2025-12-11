-- ===============================================
-- 02-seed.sql – Testdaten für EliteGärtner
-- ===============================================

-- =============================
-- TAGS
-- =============================
INSERT INTO TAGS (Label) VALUES
('Auberginen'),
('Gurken'),
('Tomaten'),
('Kürbisse'),
('Paprika'),
('Zucchini'),
('Kartoffeln'),
('Karotten'),
('Salate'),
('Zwiebeln'),
('Melonen'),
('Äpfel'),
('Birnen'),
('Pfirsiche'),
('Kirschen'),
('Erdbeeren'),
('Trauben');

-- =============================
-- PROFILE (15 Nutzer)
-- =============================
INSERT INTO PROFILE 
(UserName, FirstName, LastName, EMail, PasswordHash,
 PhoneNumber, ProfileText, ShareMail, SharePhoneNumber, UserCreated)
VALUES
('TomatenTiger',     'Lukas',   'Schneider',  'tomatentiger@elitegaertner.test', 'hash_tomate',
 '01511-0000001', 'Liebt saftige Tomaten und experimentiert mit Sorten.', TRUE,  FALSE, NOW()),
('ZucchiniZauberer', 'Anna',    'Bauer',      'zucchinizauberer@elitegaertner.test', 'hash_zucchini',
 '01511-0000002', 'Zaubert aus Zucchini jedes Jahr neue Rezepte.', TRUE, TRUE, NOW()),
('GurkenGuru',       'Max',     'Müller',     'gurkenguru@elitegaertner.test', 'hash_gurke',
 '01511-0000003', 'Schwört auf knackige Gurken aus dem Hochbeet.', FALSE, TRUE, NOW()),
('BeerenBoss',       'Julia',   'Weber',      'beerenboss@elitegaertner.test', 'hash_beeren',
 '01511-0000004', 'Erdbeeren, Kirschen, Trauben – alles was süß ist.', TRUE, TRUE, NOW()),
('KürbisKönig',      'Leon',    'Fischer',    'kuerbiskoenig@elitegaertner.test', 'hash_kuerbis',
 '01511-0000005', 'Patch voller Kürbisse in allen Formen und Farben.', TRUE, FALSE, NOW()),
('PaprikaPiratin',   'Sarah',   'Wagner',     'paprikapiratin@elitegaertner.test', 'hash_paprika',
 '01511-0000006', 'Sammelt Paprikasamen aus aller Welt.', FALSE, TRUE, NOW()),
('MelonenMaster',    'Jonas',   'Hoffmann',   'melonenmaster@elitegaertner.test', 'hash_melone',
 '01511-0000007', 'Versucht jedes Jahr, noch süßere Melonen zu ziehen.', TRUE, TRUE, NOW()),
('KartoffelKnight',  'Laura',   'Becker',     'kartoffelknight@elitegaertner.test', 'hash_kartoffel',
 '01511-0000008', 'Baut alte Kartoffelsorten im Garten an.', TRUE, FALSE, NOW()),
('KarottenKönigin',  'David',   'Schulz',     'karottenkoenigin@elitegaertner.test', 'hash_karotte',
 '01511-0000009', 'Mag bunte Karotten – lila, gelb und orange.', TRUE, TRUE, NOW()),
('SalatSamurai',     'Nina',    'Keller',     'salatsamurai@elitegaertner.test', 'hash_salat',
 '01511-0000010', 'Beherrscht jede Salatmischung mit perfektem Dressing.', FALSE, TRUE, NOW()),
('ZwiebelZauberin',  'Felix',   'Braun',      'zwiebelzauberin@elitegaertner.test', 'hash_zwiebel',
 '01511-0000011', 'Zieht Zwiebeln in allen Größen und Schärfegraden.', TRUE, FALSE, NOW()),
('TraubenTaktiker',  'Jana',    'Richter',    'traubentaktiker@elitegaertner.test', 'hash_trauben',
 '01511-0000012', 'Plant Traubenreihen wie ein Schachmeister.', TRUE, TRUE, NOW()),
('ApfelAlchemist',   'Tim',     'Vogel',      'apfelalchemist@elitegaertner.test', 'hash_apfel',
 '01511-0000013', 'Veredelt Apfelbäume zu verrückten Sortenkombis.', TRUE, TRUE, NOW()),
('BirnenBarde',      'Lisa',    'König',      'birnenbarde@elitegaertner.test', 'hash_birne',
 '01511-0000014', 'Schreibt Gedichte über saftige Birnen.', FALSE, TRUE, NOW()),
('PfirsichPilot',    'Marco',   'Hartmann',   'pfirsichpilot@elitegaertner.test', 'hash_pfirsich',
 '01511-0000015', 'Pfirsichbäume in jedem verfügbaren Quadratmeter.', TRUE, FALSE, NOW());

-- =============================
-- PROFILEPREFERENCES (Tag-Präferenzen)
-- =============================

-- TomatenTiger: Tomaten, Paprika, Zucchini
INSERT INTO PROFILEPREFERENCES (TagId, ProfileId, DateUpdated)
SELECT t.TagId, p.ProfileId, NOW()
FROM TAGS t, PROFILE p
WHERE p.UserName = 'TomatenTiger'
  AND t.Label IN ('Tomaten','Paprika','Zucchini');

-- ZucchiniZauberer: Zucchini, Kartoffeln, Karotten
INSERT INTO PROFILEPREFERENCES (TagId, ProfileId, DateUpdated)
SELECT t.TagId, p.ProfileId, NOW()
FROM TAGS t, PROFILE p
WHERE p.UserName = 'ZucchiniZauberer'
  AND t.Label IN ('Zucchini','Kartoffeln','Karotten');

-- GurkenGuru: Gurken, Salate
INSERT INTO PROFILEPREFERENCES (TagId, ProfileId, DateUpdated)
SELECT t.TagId, p.ProfileId, NOW()
FROM TAGS t, PROFILE p
WHERE p.UserName = 'GurkenGuru'
  AND t.Label IN ('Gurken','Salate');

-- BeerenBoss: Erdbeeren, Kirschen, Trauben
INSERT INTO PROFILEPREFERENCES (TagId, ProfileId, DateUpdated)
SELECT t.TagId, p.ProfileId, NOW()
FROM TAGS t, PROFILE p
WHERE p.UserName = 'BeerenBoss'
  AND t.Label IN ('Erdbeeren','Kirschen','Trauben');

-- KürbisKönig: Kürbisse, Kartoffeln
INSERT INTO PROFILEPREFERENCES (TagId, ProfileId, DateUpdated)
SELECT t.TagId, p.ProfileId, NOW()
FROM TAGS t, PROFILE p
WHERE p.UserName = 'KürbisKönig'
  AND t.Label IN ('Kürbisse','Kartoffeln');

-- PaprikaPiratin: Paprika, Tomaten
INSERT INTO PROFILEPREFERENCES (TagId, ProfileId, DateUpdated)
SELECT t.TagId, p.ProfileId, NOW()
FROM TAGS t, PROFILE p
WHERE p.UserName = 'PaprikaPiratin'
  AND t.Label IN ('Paprika','Tomaten');

-- MelonenMaster: Melonen, Erdbeeren, Trauben
INSERT INTO PROFILEPREFERENCES (TagId, ProfileId, DateUpdated)
SELECT t.TagId, p.ProfileId, NOW()
FROM TAGS t, PROFILE p
WHERE p.UserName = 'MelonenMaster'
  AND t.Label IN ('Melonen','Erdbeeren','Trauben');

-- KartoffelKnight: Kartoffeln, Zwiebeln, Karotten
INSERT INTO PROFILEPREFERENCES (TagId, ProfileId, DateUpdated)
SELECT t.TagId, p.ProfileId, NOW()
FROM TAGS t, PROFILE p
WHERE p.UserName = 'KartoffelKnight'
  AND t.Label IN ('Kartoffeln','Zwiebeln','Karotten');

-- KarottenKönigin: Karotten, Salate, Gurken
INSERT INTO PROFILEPREFERENCES (TagId, ProfileId, DateUpdated)
SELECT t.TagId, p.ProfileId, NOW()
FROM TAGS t, PROFILE p
WHERE p.UserName = 'KarottenKönigin'
  AND t.Label IN ('Karotten','Salate','Gurken');

-- SalatSamurai: Salate, Tomaten, Zwiebeln
INSERT INTO PROFILEPREFERENCES (TagId, ProfileId, DateUpdated)
SELECT t.TagId, p.ProfileId, NOW()
FROM TAGS t, PROFILE p
WHERE p.UserName = 'SalatSamurai'
  AND t.Label IN ('Salate','Tomaten','Zwiebeln');

-- ZwiebelZauberin: Zwiebeln, Kartoffeln
INSERT INTO PROFILEPREFERENCES (TagId, ProfileId, DateUpdated)
SELECT t.TagId, p.ProfileId, NOW()
FROM TAGS t, PROFILE p
WHERE p.UserName = 'ZwiebelZauberin'
  AND t.Label IN ('Zwiebeln','Kartoffeln');

-- TraubenTaktiker: Trauben, Äpfel, Birnen
INSERT INTO PROFILEPREFERENCES (TagId, ProfileId, DateUpdated)
SELECT t.TagId, p.ProfileId, NOW()
FROM TAGS t, PROFILE p
WHERE p.UserName = 'TraubenTaktiker'
  AND t.Label IN ('Trauben','Äpfel','Birnen');

-- ApfelAlchemist: Äpfel, Birnen, Pfirsiche
INSERT INTO PROFILEPREFERENCES (TagId, ProfileId, DateUpdated)
SELECT t.TagId, p.ProfileId, NOW()
FROM TAGS t, PROFILE p
WHERE p.UserName = 'ApfelAlchemist'
  AND t.Label IN ('Äpfel','Birnen','Pfirsiche');

-- BirnenBarde: Birnen, Äpfel
INSERT INTO PROFILEPREFERENCES (TagId, ProfileId, DateUpdated)
SELECT t.TagId, p.ProfileId, NOW()
FROM TAGS t, PROFILE p
WHERE p.UserName = 'BirnenBarde'
  AND t.Label IN ('Birnen','Äpfel');

-- PfirsichPilot: Pfirsiche, Kirschen, Erdbeeren
INSERT INTO PROFILEPREFERENCES (TagId, ProfileId, DateUpdated)
SELECT t.TagId, p.ProfileId, NOW()
FROM TAGS t, PROFILE p
WHERE p.UserName = 'PfirsichPilot'
  AND t.Label IN ('Pfirsiche','Kirschen','Erdbeeren');

-- =============================
-- HARVESTUPLOADS (1–3 Uploads pro Nutzer)
-- =============================

-- TomatenTiger: 3 Uploads
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT
  'https://example.com/uploads/tomate1.jpg',
  'Rote Tomaten aus dem Gewächshaus.',
  180, 7, 7, NOW(), p.ProfileId
FROM PROFILE p WHERE p.UserName = 'TomatenTiger';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT
  'https://example.com/uploads/tomate2.jpg',
  'Fleischtomate, perfekt für Soßen.',
  250, 9, 8, NOW(), p.ProfileId
FROM PROFILE p WHERE p.UserName = 'TomatenTiger';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT
  'https://example.com/uploads/tomate3.jpg',
  'Gelbe Tomaten, mild im Geschmack.',
  160, 6, 6, NOW(), p.ProfileId
FROM PROFILE p WHERE p.UserName = 'TomatenTiger';

-- ZucchiniZauberer: 2 Uploads
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT
  'https://example.com/uploads/zucchini1.jpg',
  'Lange Zucchini, direkt vom Hochbeet.',
  320, 25, 5, NOW(), p.ProfileId
FROM PROFILE p WHERE p.UserName = 'ZucchiniZauberer';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT
  'https://example.com/uploads/zucchini2.jpg',
  'Runde Zucchini für gefüllte Gerichte.',
  400, 15, 15, NOW(), p.ProfileId
FROM PROFILE p WHERE p.UserName = 'ZucchiniZauberer';

-- GurkenGuru: 2 Uploads
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT
  'https://example.com/uploads/gurke1.jpg',
  'Salatgurke, super knackig.',
  300, 25, 4, NOW(), p.ProfileId
FROM PROFILE p WHERE p.UserName = 'GurkenGuru';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT
  'https://example.com/uploads/gurke2.jpg',
  'Einlegegurken für den Winter.',
  200, 15, 4, NOW(), p.ProfileId
FROM PROFILE p WHERE p.UserName = 'GurkenGuru';

-- BeerenBoss: 3 Uploads
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT
  'https://example.com/uploads/erdbeeren1.jpg',
  'Süße Erdbeeren vom Feld.',
  120, 10, 10, NOW(), p.ProfileId
FROM PROFILE p WHERE p.UserName = 'BeerenBoss';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT
  'https://example.com/uploads/kirschen1.jpg',
  'Dunkelrote Kirschen, sehr aromatisch.',
  150, 8, 8, NOW(), p.ProfileId
FROM PROFILE p WHERE p.UserName = 'BeerenBoss';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT
  'https://example.com/uploads/trauben1.jpg',
  'Kleine, sehr süße Trauben.',
  200, 12, 12, NOW(), p.ProfileId
FROM PROFILE p WHERE p.UserName = 'BeerenBoss';

-- KürbisKönig: 2 Uploads
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT
  'https://example.com/uploads/kuerbis1.jpg',
  'Hokkaido-Kürbis für Suppe.',
  1000, 25, 25, NOW(), p.ProfileId
FROM PROFILE p WHERE p.UserName = 'KürbisKönig';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT
  'https://example.com/uploads/kuerbis2.jpg',
  'Zierkürbis für die Deko.',
  500, 15, 15, NOW(), p.ProfileId
FROM PROFILE p WHERE p.UserName = 'KürbisKönig';

-- PaprikaPiratin: 2 Uploads
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT
  'https://example.com/uploads/paprika1.jpg',
  'Rote Paprika, sehr aromatisch.',
  180, 7, 7, NOW(), p.ProfileId
FROM PROFILE p WHERE p.UserName = 'PaprikaPiratin';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT
  'https://example.com/uploads/paprika2.jpg',
  'Gelbe Spitzpaprika, süß und mild.',
  160, 6, 8, NOW(), p.ProfileId
FROM PROFILE p WHERE p.UserName = 'PaprikaPiratin';

-- MelonenMaster: 3 Uploads
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT
  'https://example.com/uploads/melone1.jpg',
  'Wassermelone, perfekt gekühlt.',
  3500, 30, 30, NOW(), p.ProfileId
FROM PROFILE p WHERE p.UserName = 'MelonenMaster';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT
  'https://example.com/uploads/melone2.jpg',
  'Honigmelone mit intensivem Aroma.',
  2000, 20, 20, NOW(), p.ProfileId
FROM PROFILE p WHERE p.UserName = 'MelonenMaster';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT
  'https://example.com/uploads/melone3.jpg',
  'Zuckermelone mit feiner Schale.',
  1800, 18, 18, NOW(), p.ProfileId
FROM PROFILE p WHERE p.UserName = 'MelonenMaster';

-- KartoffelKnight: 2 Uploads
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT
  'https://example.com/uploads/kartoffel1.jpg',
  'Festkochende Kartoffeln für Kartoffelsalat.',
  2500, 25, 25, NOW(), p.ProfileId
FROM PROFILE p WHERE p.UserName = 'KartoffelKnight';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT
  'https://example.com/uploads/kartoffel2.jpg',
  'Mehligkochende Kartoffeln für Püree.',
  2600, 25, 25, NOW(), p.ProfileId
FROM PROFILE p WHERE p.UserName = 'KartoffelKnight';

-- KarottenKönigin: 2 Uploads
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT
  'https://example.com/uploads/karotten1.jpg',
  'Bunte Karotten im Bund.',
  800, 10, 25, NOW(), p.ProfileId
FROM PROFILE p WHERE p.UserName = 'KarottenKönigin';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT
  'https://example.com/uploads/karotten2.jpg',
  'Mini-Karotten als Snack.',
  500, 8, 20, NOW(), p.ProfileId
FROM PROFILE p WHERE p.UserName = 'KarottenKönigin';

-- SalatSamurai: 1 Upload
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT
  'https://example.com/uploads/salat1.jpg',
  'Knackiger Blattsalat-Mix.',
  400, 20, 20, NOW(), p.ProfileId
FROM PROFILE p WHERE p.UserName = 'SalatSamurai';

-- ZwiebelZauberin: 2 Uploads
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT
  'https://example.com/uploads/zwiebel1.jpg',
  'Rote Zwiebeln mit milder Schärfe.',
  700, 15, 15, NOW(), p.ProfileId
FROM PROFILE p WHERE p.UserName = 'ZwiebelZauberin';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT
  'https://example.com/uploads/zwiebel2.jpg',
  'Weiße Küchenzwiebeln für alles.',
  900, 18, 18, NOW(), p.ProfileId
FROM PROFILE p WHERE p.UserName = 'ZwiebelZauberin';

-- TraubenTaktiker: 2 Uploads
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT
  'https://example.com/uploads/trauben2.jpg',
  'Grüne Tafeltrauben.',
  600, 15, 15, NOW(), p.ProfileId
FROM PROFILE p WHERE p.UserName = 'TraubenTaktiker';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT
  'https://example.com/uploads/trauben3.jpg',
  'Blaue Trauben mit Kernen.',
  650, 15, 15, NOW(), p.ProfileId
FROM PROFILE p WHERE p.UserName = 'TraubenTaktiker';

-- ApfelAlchemist: 2 Uploads
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT
  'https://example.com/uploads/aepfel1.jpg',
  'Rote Äpfel, sehr knackig.',
  1500, 20, 20, NOW(), p.ProfileId
FROM PROFILE p WHERE p.UserName = 'ApfelAlchemist';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT
  'https://example.com/uploads/aepfel2.jpg',
  'Gemischte Apfelsorten aus eigener Zucht.',
  2000, 22, 22, NOW(), p.ProfileId
FROM PROFILE p WHERE p.UserName = 'ApfelAlchemist';

-- BirnenBarde: 1 Upload
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT
  'https://example.com/uploads/birnen1.jpg',
  'Saftige Birnen, direkt vom Baum.',
  1300, 18, 18, NOW(), p.ProfileId
FROM PROFILE p WHERE p.UserName = 'BirnenBarde';

-- PfirsichPilot: 2 Uploads
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT
  'https://example.com/uploads/pfirsiche1.jpg',
  'Reife Pfirsiche mit viel Duft.',
  1400, 18, 18, NOW(), p.ProfileId
FROM PROFILE p WHERE p.UserName = 'PfirsichPilot';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT
  'https://example.com/uploads/pfirsiche2.jpg',
  'Flache Weinbergpfirsiche.',
  1200, 17, 17, NOW(), p.ProfileId
FROM PROFILE p WHERE p.UserName = 'PfirsichPilot';

-- =============================
-- HARVESTTAGS (Tags anhand Beschreibung)
-- =============================

-- Tomaten
INSERT INTO HARVESTTAGS (TagId, UploadId)
SELECT t.TagId, h.UploadId
FROM TAGS t
JOIN HARVESTUPLOADS h ON h.Description ILIKE '%Tomate%'
WHERE t.Label = 'Tomaten';

-- Zucchini
INSERT INTO HARVESTTAGS (TagId, UploadId)
SELECT t.TagId, h.UploadId
FROM TAGS t
JOIN HARVESTUPLOADS h ON h.Description ILIKE '%Zucchini%'
WHERE t.Label = 'Zucchini';

-- Gurken
INSERT INTO HARVESTTAGS (TagId, UploadId)
SELECT t.TagId, h.UploadId
FROM TAGS t
JOIN HARVESTUPLOADS h ON h.Description ILIKE '%Gurke%'
WHERE t.Label = 'Gurken';

-- Erdbeeren
INSERT INTO HARVESTTAGS (TagId, UploadId)
SELECT t.TagId, h.UploadId
FROM TAGS t
JOIN HARVESTUPLOADS h ON h.Description ILIKE '%Erdbeer%'
WHERE t.Label = 'Erdbeeren';

-- Kirschen
INSERT INTO HARVESTTAGS (TagId, UploadId)
SELECT t.TagId, h.UploadId
FROM TAGS t
JOIN HARVESTUPLOADS h ON h.Description ILIKE '%Kirsche%'
WHERE t.Label = 'Kirschen';

-- Trauben
INSERT INTO HARVESTTAGS (TagId, UploadId)
SELECT t.TagId, h.UploadId
FROM TAGS t
JOIN HARVESTUPLOADS h ON h.Description ILIKE '%Trauben%'
WHERE t.Label = 'Trauben';

-- Melonen
INSERT INTO HARVESTTAGS (TagId, UploadId)
SELECT t.TagId, h.UploadId
FROM TAGS t
JOIN HARVESTUPLOADS h ON h.Description ILIKE '%Melone%'
WHERE t.Label = 'Melonen';

-- Äpfel
INSERT INTO HARVESTTAGS (TagId, UploadId)
SELECT t.TagId, h.UploadId
FROM TAGS t
JOIN HARVESTUPLOADS h ON h.Description ILIKE '%Apfel%'
WHERE t.Label = 'Äpfel';

-- Birnen
INSERT INTO HARVESTTAGS (TagId, UploadId)
SELECT t.TagId, h.UploadId
FROM TAGS t
JOIN HARVESTUPLOADS h ON h.Description ILIKE '%Birne%'
WHERE t.Label = 'Birnen';

-- Pfirsiche
INSERT INTO HARVESTTAGS (TagId, UploadId)
SELECT t.TagId, h.UploadId
FROM TAGS t
JOIN HARVESTUPLOADS h ON h.Description ILIKE '%Pfirsich%'
WHERE t.Label = 'Pfirsiche';

-- Kartoffeln
INSERT INTO HARVESTTAGS (TagId, UploadId)
SELECT t.TagId, h.UploadId
FROM TAGS t
JOIN HARVESTUPLOADS h ON h.Description ILIKE '%Kartoffel%'
WHERE t.Label = 'Kartoffeln';

-- Karotten
INSERT INTO HARVESTTAGS (TagId, UploadId)
SELECT t.TagId, h.UploadId
FROM TAGS t
JOIN HARVESTUPLOADS h ON h.Description ILIKE '%Karotten%' 
   OR h.Description ILIKE '%Karotte%'
WHERE t.Label = 'Karotten';

-- Salate
INSERT INTO HARVESTTAGS (TagId, UploadId)
SELECT t.TagId, h.UploadId
FROM TAGS t
JOIN HARVESTUPLOADS h ON h.Description ILIKE '%Salat%'
WHERE t.Label = 'Salate';

-- Zwiebeln
INSERT INTO HARVESTTAGS (TagId, UploadId)
SELECT t.TagId, h.UploadId
FROM TAGS t
JOIN HARVESTUPLOADS h ON h.Description ILIKE '%Zwiebel%'
WHERE t.Label = 'Zwiebeln';

-- Kürbisse
INSERT INTO HARVESTTAGS (TagId, UploadId)
SELECT t.TagId, h.UploadId
FROM TAGS t
JOIN HARVESTUPLOADS h ON h.Description ILIKE '%Kürbis%'
WHERE t.Label = 'Kürbisse';

-- Paprika
INSERT INTO HARVESTTAGS (TagId, UploadId)
SELECT t.TagId, h.UploadId
FROM TAGS t
JOIN HARVESTUPLOADS h ON h.Description ILIKE '%Paprika%'
WHERE t.Label = 'Paprika';

-- =============================
-- RATINGS (Beispielbewertungen)
-- =============================

-- TomatenTiger bewertet BeerenBoss positiv
INSERT INTO RATING (ContentCreatorId, ContentReceiverId, ProfileRating, RatingDate)
SELECT p1.ProfileId, p2.ProfileId, TRUE, NOW()
FROM PROFILE p1, PROFILE p2
WHERE p1.UserName = 'TomatenTiger'
  AND p2.UserName = 'BeerenBoss';

-- ZucchiniZauberer bewertet GurkenGuru positiv
INSERT INTO RATING (ContentCreatorId, ContentReceiverId, ProfileRating, RatingDate)
SELECT p1.ProfileId, p2.ProfileId, TRUE, NOW()
FROM PROFILE p1, PROFILE p2
WHERE p1.UserName = 'ZucchiniZauberer'
  AND p2.UserName = 'GurkenGuru';

-- GurkenGuru bewertet TomatenTiger negativ
INSERT INTO RATING (ContentCreatorId, ContentReceiverId, ProfileRating, RatingDate)
SELECT p1.ProfileId, p2.ProfileId, FALSE, NOW()
FROM PROFILE p1, PROFILE p2
WHERE p1.UserName = 'GurkenGuru'
  AND p2.UserName = 'TomatenTiger';

-- MelonenMaster bewertet BeerenBoss positiv
INSERT INTO RATING (ContentCreatorId, ContentReceiverId, ProfileRating, RatingDate)
SELECT p1.ProfileId, p2.ProfileId, TRUE, NOW()
FROM PROFILE p1, PROFILE p2
WHERE p1.UserName = 'MelonenMaster'
  AND p2.UserName = 'BeerenBoss';

-- ApfelAlchemist bewertet BirnenBarde positiv
INSERT INTO RATING (ContentCreatorId, ContentReceiverId, ProfileRating, RatingDate)
SELECT p1.ProfileId, p2.ProfileId, TRUE, NOW()
FROM PROFILE p1, PROFILE p2
WHERE p1.UserName = 'ApfelAlchemist'
  AND p2.UserName = 'BirnenBarde';

-- =============================
-- REPORTS (einige Meldungen)
-- =============================

-- GurkenGuru meldet einen Kürbis-Upload vom KürbisKönig
INSERT INTO REPORT (Reason, UploadId)
SELECT 
  'Bild wirkt wie Stockfoto.' AS Reason,
  h.UploadId
FROM HARVESTUPLOADS h
JOIN PROFILE p ON p.ProfileId = h.ProfileId
WHERE p.UserName = 'KürbisKönig'
LIMIT 1;

-- ZwiebelZauberin meldet einen Upload von TomatenTiger
INSERT INTO REPORT (Reason, UploadId)
SELECT 
  'Beschreibung ist zu ungenau.' AS Reason,
  h.UploadId
FROM HARVESTUPLOADS h
JOIN PROFILE p ON p.ProfileId = h.ProfileId
WHERE p.UserName = 'TomatenTiger'
LIMIT 1;