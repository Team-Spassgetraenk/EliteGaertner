-- ===============================================
-- seed.sql – Testdaten für EliteGärtner (20 User)
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
-- PROFILE (20 Nutzer)
-- =============================
INSERT INTO PROFILE 
(UserName, FirstName, LastName, EMail, PasswordHash,
 PhoneNumber, ProfileText, ShareMail, SharePhoneNumber, UserCreated)
VALUES
('TomatenTiger',     'Lukas',   'Schneider',  'tomatentiger@elitegaertner.test', 'hash_tomate',
 '01511-0000001', 'Liebt saftige Tomaten und probiert jede Sorte einmal aus.', TRUE,  FALSE, NOW()),
('ZucchiniZauberer', 'Anna',    'Bauer',      'zucchinizauberer@elitegaertner.test', 'hash_zucchini',
 '01511-0000002', 'Verwandelt Zucchini in Aufläufe, Kuchen und Magie.', TRUE, TRUE, NOW()),
('GurkenGuru',       'Max',     'Müller',     'gurkenguru@elitegaertner.test', 'hash_gurke',
 '01511-0000003', 'Predigt täglich die Lehre der knackigen Gurke.', FALSE, TRUE, NOW()),
('BeerenBoss',       'Julia',   'Weber',      'beerenboss@elitegaertner.test', 'hash_beeren',
 '01511-0000004', 'Beherrscht das Reich der Erdbeeren, Kirschen und Trauben.', TRUE, TRUE, NOW()),
('KürbisKönig',      'Leon',    'Fischer',    'kuerbiskoenig@elitegaertner.test', 'hash_kuerbis',
 '01511-0000005', 'Regiert einen Garten voller Kürbisse in allen Größen.', TRUE, FALSE, NOW()),
('PaprikaPiratin',   'Sarah',   'Wagner',     'paprikapiratin@elitegaertner.test', 'hash_paprika',
 '01511-0000006', 'Kapert jede Paprika-Sorte, die ihr in die Finger kommt.', FALSE, TRUE, NOW()),
('MelonenMaster',    'Jonas',   'Hoffmann',   'melonenmaster@elitegaertner.test', 'hash_melone',
 '01511-0000007', 'Auf ewiger Mission nach der süßesten Melone aller Zeiten.', TRUE, TRUE, NOW()),
('KartoffelKnight',  'Laura',   'Becker',     'kartoffelknight@elitegaertner.test', 'hash_kartoffel',
 '01511-0000008', 'Beschützt alte Kartoffelsorten wie ein wahrer Ritter.', TRUE, FALSE, NOW()),
('KarottenKönigin',  'David',   'Schulz',     'karottenkoenigin@elitegaertner.test', 'hash_karotte',
 '01511-0000009', 'Regiert über ein Reich aus bunten Karotten.', TRUE, TRUE, NOW()),
('SalatSamurai',     'Nina',    'Keller',     'salatsamurai@elitegaertner.test', 'hash_salat',
 '01511-0000010', 'Schneidet Salate schneller als sein Schatten.', FALSE, TRUE, NOW()),
('ZwiebelZauberin',  'Felix',   'Braun',      'zwiebelzauberin@elitegaertner.test', 'hash_zwiebel',
 '01511-0000011', 'Lässt Tränen fließen – aber nur beim Zwiebelschneiden.', TRUE, FALSE, NOW()),
('TraubenTaktiker',  'Jana',    'Richter',    'traubentaktiker@elitegaertner.test', 'hash_trauben',
 '01511-0000012', 'Plant jede Weinrebe wie einen Schachzug.', TRUE, TRUE, NOW()),
('ApfelAlchemist',   'Tim',     'Vogel',      'apfelalchemist@elitegaertner.test', 'hash_apfel',
 '01511-0000013', 'Veredelt Apfelbäume zu verrückten Sortenexperimenten.', TRUE, TRUE, NOW()),
('BirnenBarde',      'Lisa',    'König',      'birnenbarde@elitegaertner.test', 'hash_birne',
 '01511-0000014', 'Dichtet Oden über die perfekte Birne.', FALSE, TRUE, NOW()),
('PfirsichPilot',    'Marco',   'Hartmann',   'pfirsichpilot@elitegaertner.test', 'hash_pfirsich',
 '01511-0000015', 'Steuert direkt in Turbulenzen, wenn Pfirsichbäume reifen.', TRUE, FALSE, NOW()),
('BohnenBaron',      'Oliver',  'Schmidt',    'bohnenbaron@elitegaertner.test', 'hash_bohnen',
 '01511-0000016', 'Bohnen in allen Farben, Formen und Höhenlagen.', TRUE, TRUE, NOW()),
('SpinatSpion',      'Mia',     'Lehmann',    'spinatspion@elitegaertner.test', 'hash_spinat',
 '01511-0000017', 'Schleicht nachts durch den Garten und checkt den Spinat.', TRUE, FALSE, NOW()),
('RadieschenRocker', 'Paul',    'Jung',       'radieschenrocker@elitegaertner.test', 'hash_radieschen',
 '01511-0000018', 'Spielt laute Musik, damit Radieschen schneller wachsen.', FALSE, TRUE, NOW()),
('BrokkoliBoss',     'Emma',    'Franke',     'brokkoliboss@elitegaertner.test', 'hash_brokkoli',
 '01511-0000019', 'Stellt Brokkoli in jedes Gericht – egal ob passend oder nicht.', TRUE, TRUE, NOW()),
('MaisMagier',       'Noah',    'Seidel',     'maismagier@elitegaertner.test', 'hash_mais',
 '01511-0000020', 'Zaubert Maiskolben vom Grill auf jeden Teller.', TRUE, FALSE, NOW());

-- =============================
-- PROFILEPREFERENCES
-- =============================

-- 1 TomatenTiger: Tomaten, Paprika, Zucchini
INSERT INTO PROFILEPREFERENCES (TagId, ProfileId, DateUpdated)
SELECT t.TagId, p.ProfileId, NOW()
FROM TAGS t, PROFILE p
WHERE p.UserName = 'TomatenTiger'
  AND t.Label IN ('Tomaten','Paprika','Zucchini');

-- 2 ZucchiniZauberer: Zucchini, Kartoffeln, Karotten
INSERT INTO PROFILEPREFERENCES (TagId, ProfileId, DateUpdated)
SELECT t.TagId, p.ProfileId, NOW()
FROM TAGS t, PROFILE p
WHERE p.UserName = 'ZucchiniZauberer'
  AND t.Label IN ('Zucchini','Kartoffeln','Karotten');

-- 3 GurkenGuru: Gurken, Salate
INSERT INTO PROFILEPREFERENCES (TagId, ProfileId, DateUpdated)
SELECT t.TagId, p.ProfileId, NOW()
FROM TAGS t, PROFILE p
WHERE p.UserName = 'GurkenGuru'
  AND t.Label IN ('Gurken','Salate');

-- 4 BeerenBoss: Erdbeeren, Kirschen, Trauben
INSERT INTO PROFILEPREFERENCES (TagId, ProfileId, DateUpdated)
SELECT t.TagId, p.ProfileId, NOW()
FROM TAGS t, PROFILE p
WHERE p.UserName = 'BeerenBoss'
  AND t.Label IN ('Erdbeeren','Kirschen','Trauben');

-- 5 KürbisKönig: Kürbisse, Kartoffeln
INSERT INTO PROFILEPREFERENCES (TagId, ProfileId, DateUpdated)
SELECT t.TagId, p.ProfileId, NOW()
FROM TAGS t, PROFILE p
WHERE p.UserName = 'KürbisKönig'
  AND t.Label IN ('Kürbisse','Kartoffeln');

-- 6 PaprikaPiratin: Paprika, Tomaten
INSERT INTO PROFILEPREFERENCES (TagId, ProfileId, DateUpdated)
SELECT t.TagId, p.ProfileId, NOW()
FROM TAGS t, PROFILE p
WHERE p.UserName = 'PaprikaPiratin'
  AND t.Label IN ('Paprika','Tomaten');

-- 7 MelonenMaster: Melonen, Erdbeeren, Trauben
INSERT INTO PROFILEPREFERENCES (TagId, ProfileId, DateUpdated)
SELECT t.TagId, p.ProfileId, NOW()
FROM TAGS t, PROFILE p
WHERE p.UserName = 'MelonenMaster'
  AND t.Label IN ('Melonen','Erdbeeren','Trauben');

-- 8 KartoffelKnight: Kartoffeln, Zwiebeln, Karotten
INSERT INTO PROFILEPREFERENCES (TagId, ProfileId, DateUpdated)
SELECT t.TagId, p.ProfileId, NOW()
FROM TAGS t, PROFILE p
WHERE p.UserName = 'KartoffelKnight'
  AND t.Label IN ('Kartoffeln','Zwiebeln','Karotten');

-- 9 KarottenKönigin: Karotten, Salate, Gurken
INSERT INTO PROFILEPREFERENCES (TagId, ProfileId, DateUpdated)
SELECT t.TagId, p.ProfileId, NOW()
FROM TAGS t, PROFILE p
WHERE p.UserName = 'KarottenKönigin'
  AND t.Label IN ('Karotten','Salate','Gurken');

-- 10 SalatSamurai: Salate, Tomaten, Zwiebeln
INSERT INTO PROFILEPREFERENCES (TagId, ProfileId, DateUpdated)
SELECT t.TagId, p.ProfileId, NOW()
FROM TAGS t, PROFILE p
WHERE p.UserName = 'SalatSamurai'
  AND t.Label IN ('Salate','Tomaten','Zwiebeln');

-- 11 ZwiebelZauberin: Zwiebeln, Kartoffeln
INSERT INTO PROFILEPREFERENCES (TagId, ProfileId, DateUpdated)
SELECT t.TagId, p.ProfileId, NOW()
FROM TAGS t, PROFILE p
WHERE p.UserName = 'ZwiebelZauberin'
  AND t.Label IN ('Zwiebeln','Kartoffeln');

-- 12 TraubenTaktiker: Trauben, Äpfel, Birnen
INSERT INTO PROFILEPREFERENCES (TagId, ProfileId, DateUpdated)
SELECT t.TagId, p.ProfileId, NOW()
FROM TAGS t, PROFILE p
WHERE p.UserName = 'TraubenTaktiker'
  AND t.Label IN ('Trauben','Äpfel','Birnen');

-- 13 ApfelAlchemist: Äpfel, Birnen, Pfirsiche
INSERT INTO PROFILEPREFERENCES (TagId, ProfileId, DateUpdated)
SELECT t.TagId, p.ProfileId, NOW()
FROM TAGS t, PROFILE p
WHERE p.UserName = 'ApfelAlchemist'
  AND t.Label IN ('Äpfel','Birnen','Pfirsiche');

-- 14 BirnenBarde: Birnen, Äpfel
INSERT INTO PROFILEPREFERENCES (TagId, ProfileId, DateUpdated)
SELECT t.TagId, p.ProfileId, NOW()
FROM TAGS t, PROFILE p
WHERE p.UserName = 'BirnenBarde'
  AND t.Label IN ('Birnen','Äpfel');

-- 15 PfirsichPilot: Pfirsiche, Kirschen, Erdbeeren
INSERT INTO PROFILEPREFERENCES (TagId, ProfileId, DateUpdated)
SELECT t.TagId, p.ProfileId, NOW()
FROM TAGS t, PROFILE p
WHERE p.UserName = 'PfirsichPilot'
  AND t.Label IN ('Pfirsiche','Kirschen','Erdbeeren');

-- 16 BohnenBaron: Kartoffeln, Karotten, Salate (nah an Hülsenfrüchten dran)
INSERT INTO PROFILEPREFERENCES (TagId, ProfileId, DateUpdated)
SELECT t.TagId, p.ProfileId, NOW()
FROM TAGS t, PROFILE p
WHERE p.UserName = 'BohnenBaron'
  AND t.Label IN ('Kartoffeln','Karotten','Salate');

-- 17 SpinatSpion: Salate, Zwiebeln, Gurken
INSERT INTO PROFILEPREFERENCES (TagId, ProfileId, DateUpdated)
SELECT t.TagId, p.ProfileId, NOW()
FROM TAGS t, PROFILE p
WHERE p.UserName = 'SpinatSpion'
  AND t.Label IN ('Salate','Zwiebeln','Gurken');

-- 18 RadieschenRocker: Karotten, Salate, Zwiebeln
INSERT INTO PROFILEPREFERENCES (TagId, ProfileId, DateUpdated)
SELECT t.TagId, p.ProfileId, NOW()
FROM TAGS t, PROFILE p
WHERE p.UserName = 'RadieschenRocker'
  AND t.Label IN ('Karotten','Salate','Zwiebeln');

-- 19 BrokkoliBoss: Karotten, Kartoffeln, Salate
INSERT INTO PROFILEPREFERENCES (TagId, ProfileId, DateUpdated)
SELECT t.TagId, p.ProfileId, NOW()
FROM TAGS t, PROFILE p
WHERE p.UserName = 'BrokkoliBoss'
  AND t.Label IN ('Karotten','Kartoffeln','Salate');

-- 20 MaisMagier: Kürbisse, Kartoffeln, Zwiebeln
INSERT INTO PROFILEPREFERENCES (TagId, ProfileId, DateUpdated)
SELECT t.TagId, p.ProfileId, NOW()
FROM TAGS t, PROFILE p
WHERE p.UserName = 'MaisMagier'
  AND t.Label IN ('Kürbisse','Kartoffeln','Zwiebeln');

-- =============================
-- HARVESTUPLOADS (1–4 Uploads)
-- =============================

-- 1 TomatenTiger: 3 Uploads
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'https://example.com/uploads/tomate1.jpg',
       'Rote Tomaten aus dem Gewächshaus.',
       180, 7, 7, NOW(), p.ProfileId
FROM PROFILE p WHERE p.UserName = 'TomatenTiger';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'https://example.com/uploads/tomate2.jpg',
       'Fleischtomate, perfekt für Soßen.',
       250, 9, 8, NOW(), p.ProfileId
FROM PROFILE p WHERE p.UserName = 'TomatenTiger';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'https://example.com/uploads/tomate3.jpg',
       'Gelbe Tomaten, mild im Geschmack.',
       160, 6, 6, NOW(), p.ProfileId
FROM PROFILE p WHERE p.UserName = 'TomatenTiger';

-- 2 ZucchiniZauberer: 2 Uploads
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'https://example.com/uploads/zucchini1.jpg',
       'Lange Zucchini, direkt vom Hochbeet.',
       320, 25, 5, NOW(), p.ProfileId
FROM PROFILE p WHERE p.UserName = 'ZucchiniZauberer';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'https://example.com/uploads/zucchini2.jpg',
       'Runde Zucchini für gefüllte Gerichte.',
       400, 15, 15, NOW(), p.ProfileId
FROM PROFILE p WHERE p.UserName = 'ZucchiniZauberer';

-- 3 GurkenGuru: 2 Uploads
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'https://example.com/uploads/gurke1.jpg',
       'Salatgurke, super knackig.',
       300, 25, 4, NOW(), p.ProfileId
FROM PROFILE p WHERE p.UserName = 'GurkenGuru';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'https://example.com/uploads/gurke2.jpg',
       'Einlegegurken für den Winter.',
       200, 15, 4, NOW(), p.ProfileId
FROM PROFILE p WHERE p.UserName = 'GurkenGuru';

-- 4 BeerenBoss: 3 Uploads
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'https://example.com/uploads/erdbeeren1.jpg',
       'Süße Erdbeeren vom Feld.',
       120, 10, 10, NOW(), p.ProfileId
FROM PROFILE p WHERE p.UserName = 'BeerenBoss';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'https://example.com/uploads/kirschen1.jpg',
       'Dunkelrote Kirschen, sehr aromatisch.',
       150, 8, 8, NOW(), p.ProfileId
FROM PROFILE p WHERE p.UserName = 'BeerenBoss';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'https://example.com/uploads/trauben1.jpg',
       'Kleine, sehr süße Trauben.',
       200, 12, 12, NOW(), p.ProfileId
FROM PROFILE p WHERE p.UserName = 'BeerenBoss';

-- 5 KürbisKönig: 2 Uploads
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'https://example.com/uploads/kuerbis1.jpg',
       'Hokkaido-Kürbis für Suppe.',
       1000, 25, 25, NOW(), p.ProfileId
FROM PROFILE p WHERE p.UserName = 'KürbisKönig';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'https://example.com/uploads/kuerbis2.jpg',
       'Zierkürbis für die Deko.',
       500, 15, 15, NOW(), p.ProfileId
FROM PROFILE p WHERE p.UserName = 'KürbisKönig';

-- 6 PaprikaPiratin: 2 Uploads
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'https://example.com/uploads/paprika1.jpg',
       'Rote Paprika, sehr aromatisch.',
       180, 7, 7, NOW(), p.ProfileId
FROM PROFILE p WHERE p.UserName = 'PaprikaPiratin';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'https://example.com/uploads/paprika2.jpg',
       'Gelbe Spitzpaprika, süß und mild.',
       160, 6, 8, NOW(), p.ProfileId
FROM PROFILE p WHERE p.UserName = 'PaprikaPiratin';

-- 7 MelonenMaster: 3 Uploads
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'https://example.com/uploads/melone1.jpg',
       'Wassermelone, perfekt gekühlt.',
       3500, 30, 30, NOW(), p.ProfileId
FROM PROFILE p WHERE p.UserName = 'MelonenMaster';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'https://example.com/uploads/melone2.jpg',
       'Honigmelone mit intensivem Aroma.',
       2000, 20, 20, NOW(), p.ProfileId
FROM PROFILE p WHERE p.UserName = 'MelonenMaster';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'https://example.com/uploads/melone3.jpg',
       'Zuckermelone mit feiner Schale.',
       1800, 18, 18, NOW(), p.ProfileId
FROM PROFILE p WHERE p.UserName = 'MelonenMaster';

-- 8 KartoffelKnight: 2 Uploads
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'https://example.com/uploads/kartoffel1.jpg',
       'Festkochende Kartoffeln für Kartoffelsalat.',
       2500, 25, 25, NOW(), p.ProfileId
FROM PROFILE p WHERE p.UserName = 'KartoffelKnight';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'https://example.com/uploads/kartoffel2.jpg',
       'Mehligkochende Kartoffeln für Püree.',
       2600, 25, 25, NOW(), p.ProfileId
FROM PROFILE p WHERE p.UserName = 'KartoffelKnight';

-- 9 KarottenKönigin: 2 Uploads
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'https://example.com/uploads/karotten1.jpg',
       'Bunte Karotten im Bund.',
       800, 10, 25, NOW(), p.ProfileId
FROM PROFILE p WHERE p.UserName = 'KarottenKönigin';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'https://example.com/uploads/karotten2.jpg',
       'Mini-Karotten als Snack.',
       500, 8, 20, NOW(), p.ProfileId
FROM PROFILE p WHERE p.UserName = 'KarottenKönigin';

-- 10 SalatSamurai: 1 Upload
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'https://example.com/uploads/salat1.jpg',
       'Knackiger Blattsalat-Mix.',
       400, 20, 20, NOW(), p.ProfileId
FROM PROFILE p WHERE p.UserName = 'SalatSamurai';

-- 11 ZwiebelZauberin: 2 Uploads
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'https://example.com/uploads/zwiebel1.jpg',
       'Rote Zwiebeln mit milder Schärfe.',
       700, 15, 15, NOW(), p.ProfileId
FROM PROFILE p WHERE p.UserName = 'ZwiebelZauberin';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'https://example.com/uploads/zwiebel2.jpg',
       'Weiße Küchenzwiebeln für alles.',
       900, 18, 18, NOW(), p.ProfileId
FROM PROFILE p WHERE p.UserName = 'ZwiebelZauberin';

-- 12 TraubenTaktiker: 2 Uploads
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'https://example.com/uploads/trauben2.jpg',
       'Grüne Tafeltrauben.',
       600, 15, 15, NOW(), p.ProfileId
FROM PROFILE p WHERE p.UserName = 'TraubenTaktiker';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'https://example.com/uploads/trauben3.jpg',
       'Blaue Trauben mit Kernen.',
       650, 15, 15, NOW(), p.ProfileId
FROM PROFILE p WHERE p.UserName = 'TraubenTaktiker';

-- 13 ApfelAlchemist: 2 Uploads
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'https://example.com/uploads/aepfel1.jpg',
       'Rote Äpfel, sehr knackig.',
       1500, 20, 20, NOW(), p.ProfileId
FROM PROFILE p WHERE p.UserName = 'ApfelAlchemist';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'https://example.com/uploads/aepfel2.jpg',
       'Gemischte Apfelsorten aus eigener Zucht.',
       2000, 22, 22, NOW(), p.ProfileId
FROM PROFILE p WHERE p.UserName = 'ApfelAlchemist';

-- 14 BirnenBarde: 1 Upload
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'https://example.com/uploads/birnen1.jpg',
       'Saftige Birnen, direkt vom Baum.',
       1300, 18, 18, NOW(), p.ProfileId
FROM PROFILE p WHERE p.UserName = 'BirnenBarde';

-- 15 PfirsichPilot: 2 Uploads
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'https://example.com/uploads/pfirsiche1.jpg',
       'Reife Pfirsiche mit viel Duft.',
       1400, 18, 18, NOW(), p.ProfileId
FROM PROFILE p WHERE p.UserName = 'PfirsichPilot';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'https://example.com/uploads/pfirsiche2.jpg',
       'Flache Weinbergpfirsiche.',
       1200, 17, 17, NOW(), p.ProfileId
FROM PROFILE p WHERE p.UserName = 'PfirsichPilot';

-- 16 BohnenBaron: 3 Uploads
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'https://example.com/uploads/bohnen1.jpg',
       'Buschbohnen im Hochbeet.',
       900, 20, 20, NOW(), p.ProfileId
FROM PROFILE p WHERE p.UserName = 'BohnenBaron';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'https://example.com/uploads/bohnen2.jpg',
       'Stangenbohnen entlang eines Rankgitters.',
       1100, 25, 25, NOW(), p.ProfileId
FROM PROFILE p WHERE p.UserName = 'BohnenBaron';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'https://example.com/uploads/bohnen3.jpg',
       'Bunte Bohnenmischung für Eintöpfe.',
       800, 18, 18, NOW(), p.ProfileId
FROM PROFILE p WHERE p.UserName = 'BohnenBaron';

-- 17 SpinatSpion: 2 Uploads
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'https://example.com/uploads/spinat1.jpg',
       'Junger Spinat für Salate.',
       500, 18, 18, NOW(), p.ProfileId
FROM PROFILE p WHERE p.UserName = 'SpinatSpion';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'https://example.com/uploads/spinat2.jpg',
       'Spinatblätter für Pasta-Gerichte.',
       600, 20, 20, NOW(), p.ProfileId
FROM PROFILE p WHERE p.UserName = 'SpinatSpion';

-- 18 RadieschenRocker: 2 Uploads
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'https://example.com/uploads/radieschen1.jpg',
       'Frische Radieschen mit kräftiger Schärfe.',
       300, 15, 15, NOW(), p.ProfileId
FROM PROFILE p WHERE p.UserName = 'RadieschenRocker';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'https://example.com/uploads/radieschen2.jpg',
       'Bunte Radieschenmischung als Dekoration.',
       350, 15, 15, NOW(), p.ProfileId
FROM PROFILE p WHERE p.UserName = 'RadieschenRocker';

-- 19 BrokkoliBoss: 2 Uploads
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'https://example.com/uploads/brokkoli1.jpg',
       'Kräftiger Brokkoli für den Dampfgarer.',
       900, 20, 20, NOW(), p.ProfileId
FROM PROFILE p WHERE p.UserName = 'BrokkoliBoss';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'https://example.com/uploads/brokkoli2.jpg',
       'Brokkoliröschen für Wok-Gerichte.',
       850, 18, 18, NOW(), p.ProfileId
FROM PROFILE p WHERE p.UserName = 'BrokkoliBoss';

-- 20 MaisMagier: 3 Uploads
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'https://example.com/uploads/mais1.jpg',
       'Maiskolben frisch vom Feld.',
       1200, 25, 25, NOW(), p.ProfileId
FROM PROFILE p WHERE p.UserName = 'MaisMagier';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'https://example.com/uploads/mais2.jpg',
       'Maiskolben auf dem Grill.',
       1300, 25, 25, NOW(), p.ProfileId
FROM PROFILE p WHERE p.UserName = 'MaisMagier';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'https://example.com/uploads/mais3.jpg',
       'Maiskörner für Salate und Bowls.',
       700, 18, 18, NOW(), p.ProfileId
FROM PROFILE p WHERE p.UserName = 'MaisMagier';

-- =============================
-- HARVESTTAGS
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
JOIN HARVESTUPLOADS h ON h.Description ILIKE '%Birnen%'
   OR h.Description ILIKE '%Birne%'
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
-- RATINGS (inkl. „Matches“)
-- =============================

-- Match 1: TomatenTiger ❤️ BeerenBoss (beidseitig positiv)
INSERT INTO RATING (ContentCreatorId, ContentReceiverId, ProfileRating, RatingDate)
SELECT p1.ProfileId, p2.ProfileId, TRUE, NOW()
FROM PROFILE p1, PROFILE p2
WHERE p1.UserName = 'TomatenTiger'
  AND p2.UserName = 'BeerenBoss';

INSERT INTO RATING (ContentCreatorId, ContentReceiverId, ProfileRating, RatingDate)
SELECT p2.ProfileId, p1.ProfileId, TRUE, NOW()
FROM PROFILE p1, PROFILE p2
WHERE p1.UserName = 'TomatenTiger'
  AND p2.UserName = 'BeerenBoss';

-- Match 2: ZucchiniZauberer ❤️ GurkenGuru
INSERT INTO RATING (ContentCreatorId, ContentReceiverId, ProfileRating, RatingDate)
SELECT p1.ProfileId, p2.ProfileId, TRUE, NOW()
FROM PROFILE p1, PROFILE p2
WHERE p1.UserName = 'ZucchiniZauberer'
  AND p2.UserName = 'GurkenGuru';

INSERT INTO RATING (ContentCreatorId, ContentReceiverId, ProfileRating, RatingDate)
SELECT p2.ProfileId, p1.ProfileId, TRUE, NOW()
FROM PROFILE p1, PROFILE p2
WHERE p1.UserName = 'ZucchiniZauberer'
  AND p2.UserName = 'GurkenGuru';

-- Match 3: MelonenMaster ❤️ BeerenBoss
INSERT INTO RATING (ContentCreatorId, ContentReceiverId, ProfileRating, RatingDate)
SELECT p1.ProfileId, p2.ProfileId, TRUE, NOW()
FROM PROFILE p1, PROFILE p2
WHERE p1.UserName = 'MelonenMaster'
  AND p2.UserName = 'BeerenBoss';

INSERT INTO RATING (ContentCreatorId, ContentReceiverId, ProfileRating, RatingDate)
SELECT p2.ProfileId, p1.ProfileId, TRUE, NOW()
FROM PROFILE p1, PROFILE p2
WHERE p1.UserName = 'MelonenMaster'
  AND p2.UserName = 'BeerenBoss';

-- Positives Rating ohne Match: ApfelAlchemist -> BirnenBarde
INSERT INTO RATING (ContentCreatorId, ContentReceiverId, ProfileRating, RatingDate)
SELECT p1.ProfileId, p2.ProfileId, TRUE, NOW()
FROM PROFILE p1, PROFILE p2
WHERE p1.UserName = 'ApfelAlchemist'
  AND p2.UserName = 'BirnenBarde';

-- Negatives Rating: GurkenGuru -> TomatenTiger
INSERT INTO RATING (ContentCreatorId, ContentReceiverId, ProfileRating, RatingDate)
SELECT p1.ProfileId, p2.ProfileId, FALSE, NOW()
FROM PROFILE p1, PROFILE p2
WHERE p1.UserName = 'GurkenGuru'
  AND p2.UserName = 'TomatenTiger';

-- =============================
-- REPORTS (10 Meldungen)
-- =============================

-- 1–4: Vier verschiedene User melden dasselbe Bild von TomatenTiger
INSERT INTO REPORT (Reason, UploadId)
SELECT 'Sieht aus wie ein Stockfoto.' AS Reason,
       h.UploadId
FROM HARVESTUPLOADS h
         JOIN PROFILE p ON p.ProfileId = h.ProfileId
WHERE p.UserName = 'TomatenTiger'
    LIMIT 1;

INSERT INTO REPORT (Reason, UploadId)
SELECT 'Beschreibung passt nicht zum Bild.' AS Reason,
       h.UploadId
FROM HARVESTUPLOADS h
         JOIN PROFILE p ON p.ProfileId = h.ProfileId
WHERE p.UserName = 'TomatenTiger'
    LIMIT 1;

INSERT INTO REPORT (Reason, UploadId)
SELECT 'Verdacht auf Fake-Upload.' AS Reason,
       h.UploadId
FROM HARVESTUPLOADS h
         JOIN PROFILE p ON p.ProfileId = h.ProfileId
WHERE p.UserName = 'TomatenTiger'
    LIMIT 1;

INSERT INTO REPORT (Reason, UploadId)
SELECT 'Mehrfach hochgeladenes Bild.' AS Reason,
       h.UploadId
FROM HARVESTUPLOADS h
         JOIN PROFILE p ON p.ProfileId = h.ProfileId
WHERE p.UserName = 'TomatenTiger'
    LIMIT 1;

-- 5: GurkenGuru meldet einen Kürbis-Upload vom KürbisKönig
INSERT INTO REPORT (Reason, UploadId)
SELECT 'Zu dunkel aufgenommen, schwer erkennbar.' AS Reason,
       h.UploadId
FROM HARVESTUPLOADS h
         JOIN PROFILE p ON p.ProfileId = h.ProfileId
WHERE p.UserName = 'KürbisKönig'
    LIMIT 1;

-- 6: ZwiebelZauberin meldet einen Melonen-Upload von MelonenMaster
INSERT INTO REPORT (Reason, UploadId)
SELECT 'Kategorie wirkt unpassend für das Bild.' AS Reason,
       h.UploadId
FROM HARVESTUPLOADS h
         JOIN PROFILE p ON p.ProfileId = h.ProfileId
WHERE p.UserName = 'MelonenMaster'
    LIMIT 1;

-- 7: RadieschenRocker meldet einen Paprika-Upload von PaprikaPiratin
INSERT INTO REPORT (Reason, UploadId)
SELECT 'Text enthält unpassende Formulierungen.' AS Reason,
       h.UploadId
FROM HARVESTUPLOADS h
         JOIN PROFILE p ON p.ProfileId = h.ProfileId
WHERE p.UserName = 'PaprikaPiratin'
    LIMIT 1;

-- 8: BrokkoliBoss meldet einen Kartoffel-Upload von KartoffelKnight
INSERT INTO REPORT (Reason, UploadId)
SELECT 'Bildqualität ist zu niedrig.' AS Reason,
       h.UploadId
FROM HARVESTUPLOADS h
         JOIN PROFILE p ON p.ProfileId = h.ProfileId
WHERE p.UserName = 'KartoffelKnight'
    LIMIT 1;

-- 9: MaisMagier meldet einen Trauben-Upload von TraubenTaktiker
INSERT INTO REPORT (Reason, UploadId)
SELECT 'Vermutlich nicht selbst angebaut.' AS Reason,
       h.UploadId
FROM HARVESTUPLOADS h
         JOIN PROFILE p ON p.ProfileId = h.ProfileId
WHERE p.UserName = 'TraubenTaktiker'
    LIMIT 1;

-- 10: BeerenBoss meldet einen Brokkoli-Upload von BrokkoliBoss
INSERT INTO REPORT (Reason, UploadId)
SELECT 'Kein Obst, gehört eher in Gemüse-Kategorie.' AS Reason,
       h.UploadId
FROM HARVESTUPLOADS h
         JOIN PROFILE p ON p.ProfileId = h.ProfileId
WHERE p.UserName = 'BrokkoliBoss'
    LIMIT 1;