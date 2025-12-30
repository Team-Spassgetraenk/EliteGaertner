-- ===============================================
-- seed.sql – Testdaten für EliteGärtner (20 User)
-- ===============================================

\connect elitegaertner

-- Basis-Zeitpunkt für konsistente, aber unterschiedliche Seed-Zeitstempel
SELECT now() AS seed_now \gset

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
    ('Trauben'),
    ('Bohnen'),
    ('Spinat'),
    ('Radieschen'),
    ('Brokkoli'),
    ('Mais');

-- =============================
-- PROFILE (20 Nutzer)
-- =============================
INSERT INTO PROFILE
(ProfilePictureUrl, UserName, FirstName, LastName, EMail, PasswordHash,
 PhoneNumber, ProfileText, ShareMail, SharePhoneNumber, UserCreated)
VALUES
    ('/pictures/Profilbilder/Profilbild_05.png', 'tomatentiger',     'Lukas',   'Schneider',  'tomatentiger@elitegaertner.test', 'hash_tomate',
     '01511-0000001', 'Liebt saftige Tomaten und probiert jede Sorte einmal aus.', TRUE,  FALSE, (:'seed_now')::timestamptz - interval '60 days' + interval '08 hours'),
    ('/pictures/Profilbilder/Profilbild_12.png', 'zucchinizauberer', 'Anna',    'Bauer',      'zucchinizauberer@elitegaertner.test', 'hash_zucchini',
     '01511-0000002', 'Verwandelt Zucchini in Aufläufe, Kuchen und Magie.', TRUE, TRUE, (:'seed_now')::timestamptz - interval '58 days' + interval '11 hours'),
    ('/pictures/Profilbilder/Profilbild_19.png', 'gurkenguru',       'Max',     'Müller',     'gurkenguru@elitegaertner.test', 'hash_gurke',
     '01511-0000003', 'Predigt täglich die Lehre der knackigen Gurke.', FALSE, TRUE, (:'seed_now')::timestamptz - interval '56 days' + interval '15 hours'),
    ('/pictures/Profilbilder/Profilbild_03.png', 'beerenboss',       'Julia',   'Weber',      'beerenboss@elitegaertner.test', 'hash_beeren',
     '01511-0000004', 'Beherrscht das Reich der Erdbeeren, Kirschen und Trauben.', TRUE, TRUE, (:'seed_now')::timestamptz - interval '54 days' + interval '09 hours'),
    ('/pictures/Profilbilder/Profilbild_27.png', 'kürbiskönig',      'Leon',    'Fischer',    'kuerbiskoenig@elitegaertner.test', 'hash_kuerbis',
     '01511-0000005', 'Regiert einen Garten voller Kürbisse in allen Größen.', TRUE, FALSE, (:'seed_now')::timestamptz - interval '52 days' + interval '18 hours'),
    ('/pictures/Profilbilder/Profilbild_08.png', 'paprikapiratin',   'Sarah',   'Wagner',     'paprikapiratin@elitegaertner.test', 'hash_paprika',
     '01511-0000006', 'Kapert jede Paprika-Sorte, die ihr in die Finger kommt.', FALSE, TRUE, (:'seed_now')::timestamptz - interval '50 days' + interval '10 hours'),
    ('/pictures/Profilbilder/Profilbild_14.png', 'melonenmaster',    'Jonas',   'Hoffmann',   'melonenmaster@elitegaertner.test', 'hash_melone',
     '01511-0000007', 'Auf ewiger Mission nach der süßesten Melone aller Zeiten.', TRUE, TRUE, (:'seed_now')::timestamptz - interval '48 days' + interval '14 hours'),
    ('/pictures/Profilbilder/Profilbild_01.png', 'kartoffelknight',  'Laura',   'Becker',     'kartoffelknight@elitegaertner.test', 'hash_kartoffel',
     '01511-0000008', 'Beschützt alte Kartoffelsorten wie ein wahrer Ritter.', TRUE, FALSE, (:'seed_now')::timestamptz - interval '46 days' + interval '07 hours'),
    ('/pictures/Profilbilder/Profilbild_22.png', 'karottenkönigin',  'David',   'Schulz',     'karottenkoenigin@elitegaertner.test', 'hash_karotte',
     '01511-0000009', 'Regiert über ein Reich aus bunten Karotten.', TRUE, TRUE, (:'seed_now')::timestamptz - interval '44 days' + interval '12 hours'),
    ('/pictures/Profilbilder/Profilbild_10.png', 'salatsamurai',     'Nina',    'Keller',     'salatsamurai@elitegaertner.test', 'hash_salat',
     '01511-0000010', 'Schneidet Salate schneller als sein Schatten.', FALSE, TRUE, (:'seed_now')::timestamptz - interval '42 days' + interval '16 hours'),
    ('/pictures/Profilbilder/Profilbild_16.png', 'zwiebelzauberin',  'Felix',   'Braun',      'zwiebelzauberin@elitegaertner.test', 'hash_zwiebel',
     '01511-0000011', 'Lässt Tränen fließen – aber nur beim Zwiebelschneiden.', TRUE, FALSE, (:'seed_now')::timestamptz - interval '40 days' + interval '09 hours'),
    ('/pictures/Profilbilder/Profilbild_06.png', 'traubentaktiker',  'Jana',    'Richter',    'traubentaktiker@elitegaertner.test', 'hash_trauben',
     '01511-0000012', 'Plant jede Weinrebe wie einen Schachzug.', TRUE, TRUE, (:'seed_now')::timestamptz - interval '38 days' + interval '13 hours'),
    ('/pictures/Profilbilder/Profilbild_25.png', 'apfelalchemist',   'Tim',     'Vogel',      'apfelalchemist@elitegaertner.test', 'hash_apfel',
     '01511-0000013', 'Veredelt Apfelbäume zu verrückten Sortenexperimenten.', TRUE, TRUE, (:'seed_now')::timestamptz - interval '36 days' + interval '17 hours'),
    ('/pictures/Profilbilder/Profilbild_04.png', 'birnenbarde',      'Lisa',    'König',      'birnenbarde@elitegaertner.test', 'hash_birne',
     '01511-0000014', 'Dichtet Oden über die perfekte Birne.', FALSE, TRUE, (:'seed_now')::timestamptz - interval '34 days' + interval '08 hours'),
    ('/pictures/Profilbilder/Profilbild_18.png', 'pfirsichpilot',    'Marco',   'Hartmann',   'pfirsichpilot@elitegaertner.test', 'hash_pfirsich',
     '01511-0000015', 'Steuert direkt in Turbulenzen, wenn Pfirsichbäume reifen.', TRUE, FALSE, (:'seed_now')::timestamptz - interval '32 days' + interval '19 hours'),
    ('/pictures/Profilbilder/Profilbild_11.png', 'bohnenbaron',      'Oliver',  'Schmidt',    'bohnenbaron@elitegaertner.test', 'hash_bohnen',
     '01511-0000016', 'Bohnen in allen Farben, Formen und Höhenlagen.', TRUE, TRUE, (:'seed_now')::timestamptz - interval '30 days' + interval '10 hours'),
    ('/pictures/Profilbilder/Profilbild_23.png', 'spinatspion',      'Mia',     'Lehmann',    'spinatspion@elitegaertner.test', 'hash_spinat',
     '01511-0000017', 'Schleicht nachts durch den Garten und checkt den Spinat.', TRUE, FALSE, (:'seed_now')::timestamptz - interval '28 days' + interval '14 hours'),
    ('/pictures/Profilbilder/Profilbild_02.png', 'radieschenrocker', 'Paul',    'Jung',       'radieschenrocker@elitegaertner.test', 'hash_radieschen',
     '01511-0000018', 'Spielt laute Musik, damit Radieschen schneller wachsen.', FALSE, TRUE, (:'seed_now')::timestamptz - interval '26 days' + interval '09 hours'),
    ('/pictures/Profilbilder/Profilbild_17.png', 'brokkoliboss',     'Emma',    'Franke',     'brokkoliboss@elitegaertner.test', 'hash_brokkoli',
     '01511-0000019', 'Stellt Brokkoli in jedes Gericht – egal ob passend oder nicht.', TRUE, TRUE, (:'seed_now')::timestamptz - interval '24 days' + interval '12 hours'),
    ('/pictures/Profilbilder/Profilbild_28.png', 'maismagier',       'Noah',    'Seidel',     'maismagier@elitegaertner.test', 'hash_mais',
     '01511-0000020', 'Zaubert Maiskolben vom Grill auf jeden Teller.', TRUE, FALSE, (:'seed_now')::timestamptz - interval '22 days' + interval '18 hours');

-- =============================
-- PROFILEPREFERENCES
-- =============================

-- 1 TomatenTiger
INSERT INTO PROFILEPREFERENCES (TagId, ProfileId, DateUpdated)
SELECT t.TagId, p.ProfileId, (:'seed_now')::timestamptz - interval '21 days' + interval '10 hours'
FROM TAGS t, PROFILE p
WHERE p.UserName = 'tomatentiger'
  AND t.Label IN ('Tomaten','Paprika','Zucchini');

-- 2 ZucchiniZauberer
INSERT INTO PROFILEPREFERENCES (TagId, ProfileId, DateUpdated)
SELECT t.TagId, p.ProfileId, (:'seed_now')::timestamptz - interval '21 days' + interval '12 hours'
FROM TAGS t, PROFILE p
WHERE p.UserName = 'zucchinizauberer'
  AND t.Label IN ('Zucchini','Kartoffeln','Karotten');

-- 3 GurkenGuru
INSERT INTO PROFILEPREFERENCES (TagId, ProfileId, DateUpdated)
SELECT t.TagId, p.ProfileId, (:'seed_now')::timestamptz - interval '20 days' + interval '09 hours'
FROM TAGS t, PROFILE p
WHERE p.UserName = 'gurkenguru'
  AND t.Label IN ('Gurken','Salate');

-- 4 BeerenBoss
INSERT INTO PROFILEPREFERENCES (TagId, ProfileId, DateUpdated)
SELECT t.TagId, p.ProfileId, (:'seed_now')::timestamptz - interval '20 days' + interval '15 hours'
FROM TAGS t, PROFILE p
WHERE p.UserName = 'beerenboss'
  AND t.Label IN ('Erdbeeren','Kirschen','Trauben');

-- 5 KürbisKönig
INSERT INTO PROFILEPREFERENCES (TagId, ProfileId, DateUpdated)
SELECT t.TagId, p.ProfileId, (:'seed_now')::timestamptz - interval '19 days' + interval '11 hours'
FROM TAGS t, PROFILE p
WHERE p.UserName = 'kürbiskönig'
  AND t.Label IN ('Kürbisse','Kartoffeln');

-- 6 PaprikaPiratin
INSERT INTO PROFILEPREFERENCES (TagId, ProfileId, DateUpdated)
SELECT t.TagId, p.ProfileId, (:'seed_now')::timestamptz - interval '19 days' + interval '16 hours'
FROM TAGS t, PROFILE p
WHERE p.UserName = 'paprikapiratin'
  AND t.Label IN ('Paprika','Tomaten');

-- 7 MelonenMaster
INSERT INTO PROFILEPREFERENCES (TagId, ProfileId, DateUpdated)
SELECT t.TagId, p.ProfileId, (:'seed_now')::timestamptz - interval '18 days' + interval '10 hours'
FROM TAGS t, PROFILE p
WHERE p.UserName = 'melonenmaster'
  AND t.Label IN ('Melonen','Erdbeeren','Trauben');

-- 8 KartoffelKnight
INSERT INTO PROFILEPREFERENCES (TagId, ProfileId, DateUpdated)
SELECT t.TagId, p.ProfileId, (:'seed_now')::timestamptz - interval '18 days' + interval '14 hours'
FROM TAGS t, PROFILE p
WHERE p.UserName = 'kartoffelknight'
  AND t.Label IN ('Kartoffeln','Zwiebeln','Karotten');

-- 9 KarottenKönigin
INSERT INTO PROFILEPREFERENCES (TagId, ProfileId, DateUpdated)
SELECT t.TagId, p.ProfileId, (:'seed_now')::timestamptz - interval '17 days' + interval '09 hours'
FROM TAGS t, PROFILE p
WHERE p.UserName = 'karottenkönigin'
  AND t.Label IN ('Karotten','Salate','Gurken');

-- 10 SalatSamurai
INSERT INTO PROFILEPREFERENCES (TagId, ProfileId, DateUpdated)
SELECT t.TagId, p.ProfileId, (:'seed_now')::timestamptz - interval '17 days' + interval '13 hours'
FROM TAGS t, PROFILE p
WHERE p.UserName = 'salatsamurai'
  AND t.Label IN ('Salate','Tomaten','Zwiebeln');

-- 11 ZwiebelZauberin
INSERT INTO PROFILEPREFERENCES (TagId, ProfileId, DateUpdated)
SELECT t.TagId, p.ProfileId, (:'seed_now')::timestamptz - interval '16 days' + interval '11 hours'
FROM TAGS t, PROFILE p
WHERE p.UserName = 'zwiebelzauberin'
  AND t.Label IN ('Zwiebeln','Kartoffeln');

-- 12 TraubenTaktiker
INSERT INTO PROFILEPREFERENCES (TagId, ProfileId, DateUpdated)
SELECT t.TagId, p.ProfileId, (:'seed_now')::timestamptz - interval '16 days' + interval '18 hours'
FROM TAGS t, PROFILE p
WHERE p.UserName = 'traubentaktiker'
  AND t.Label IN ('Trauben','Äpfel','Birnen');

-- 13 ApfelAlchemist
INSERT INTO PROFILEPREFERENCES (TagId, ProfileId, DateUpdated)
SELECT t.TagId, p.ProfileId, (:'seed_now')::timestamptz - interval '15 days' + interval '10 hours'
FROM TAGS t, PROFILE p
WHERE p.UserName = 'apfelalchemist'
  AND t.Label IN ('Äpfel','Birnen','Pfirsiche');

-- 14 BirnenBarde
INSERT INTO PROFILEPREFERENCES (TagId, ProfileId, DateUpdated)
SELECT t.TagId, p.ProfileId, (:'seed_now')::timestamptz - interval '15 days' + interval '16 hours'
FROM TAGS t, PROFILE p
WHERE p.UserName = 'birnenbarde'
  AND t.Label IN ('Birnen','Äpfel');

-- 15 PfirsichPilot
INSERT INTO PROFILEPREFERENCES (TagId, ProfileId, DateUpdated)
SELECT t.TagId, p.ProfileId, (:'seed_now')::timestamptz - interval '14 days' + interval '12 hours'
FROM TAGS t, PROFILE p
WHERE p.UserName = 'pfirsichpilot'
  AND t.Label IN ('Pfirsiche','Kirschen','Erdbeeren');

-- 16 BohnenBaron
INSERT INTO PROFILEPREFERENCES (TagId, ProfileId, DateUpdated)
SELECT t.TagId, p.ProfileId, (:'seed_now')::timestamptz - interval '14 days' + interval '17 hours'
FROM TAGS t, PROFILE p
WHERE p.UserName = 'bohnenbaron'
  AND t.Label IN ('Kartoffeln','Karotten','Salate');

-- 17 SpinatSpion
INSERT INTO PROFILEPREFERENCES (TagId, ProfileId, DateUpdated)
SELECT t.TagId, p.ProfileId, (:'seed_now')::timestamptz - interval '13 days' + interval '09 hours'
FROM TAGS t, PROFILE p
WHERE p.UserName = 'spinatspion'
  AND t.Label IN ('Salate','Zwiebeln','Gurken');

-- 18 RadieschenRocker
INSERT INTO PROFILEPREFERENCES (TagId, ProfileId, DateUpdated)
SELECT t.TagId, p.ProfileId, (:'seed_now')::timestamptz - interval '13 days' + interval '15 hours'
FROM TAGS t, PROFILE p
WHERE p.UserName = 'radieschenrocker'
  AND t.Label IN ('Karotten','Salate','Zwiebeln');

-- 19 BrokkoliBoss
INSERT INTO PROFILEPREFERENCES (TagId, ProfileId, DateUpdated)
SELECT t.TagId, p.ProfileId, (:'seed_now')::timestamptz - interval '12 days' + interval '11 hours'
FROM TAGS t, PROFILE p
WHERE p.UserName = 'brokkoliboss'
  AND t.Label IN ('Karotten','Kartoffeln','Salate');

-- 20 MaisMagier
INSERT INTO PROFILEPREFERENCES (TagId, ProfileId, DateUpdated)
SELECT t.TagId, p.ProfileId, (:'seed_now')::timestamptz - interval '12 days' + interval '19 hours'
FROM TAGS t, PROFILE p
WHERE p.UserName = 'maismagier'
  AND t.Label IN ('Kürbisse','Kartoffeln','Zwiebeln');

-- =============================
-- HARVESTUPLOADS (1–4 Uploads)
-- =============================

-- 1 TomatenTiger: 3 Uploads
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT '/uploads/uploadid01_profileid01.jpg',
       'Rote Tomaten aus dem Gewächshaus.',
       180, 7, 7, (:'seed_now')::timestamptz - interval '9 days' + interval '08 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'tomatentiger';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT '/uploads/uploadid02_profileid01.jpg',
       'Fleischtomate, perfekt für Soßen.',
       250, 9, 8, (:'seed_now')::timestamptz - interval '8 days' + interval '14 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'tomatentiger';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT '/uploads/uploadid03_profileid01.jpg',
       'Gelbe Tomaten, mild im Geschmack.',
       160, 6, 6, (:'seed_now')::timestamptz - interval '7 days' + interval '18 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'tomatentiger';

-- 2 ZucchiniZauberer: 2 Uploads
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT '/uploads/uploadid04_profileid02.jpg',
       'Lange Zucchini, direkt vom Hochbeet.',
       320, 25, 5, (:'seed_now')::timestamptz - interval '8 days' + interval '09 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'zucchinizauberer';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT '/uploads/uploadid05_profileid02.jpg',
       'Runde Zucchini für gefüllte Gerichte.',
       400, 15, 15, (:'seed_now')::timestamptz - interval '6 days' + interval '16 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'zucchinizauberer';

-- 3 GurkenGuru: 2 Uploads
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT '/uploads/uploadid06_profileid03.jpg',
       'Gurke, super knackig.',
       300, 25, 4, (:'seed_now')::timestamptz - interval '7 days' + interval '10 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'gurkenguru';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT '/uploads/uploadid07_profileid03.jpg',
       'Einlegegurken für den Winter.',
       200, 15, 4, (:'seed_now')::timestamptz - interval '5 days' + interval '13 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'gurkenguru';

-- 4 BeerenBoss: 3 Uploads
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT '/uploads/uploadid08_profileid04.jpg',
       'Süße Erdbeeren vom Feld.',
       120, 10, 10, (:'seed_now')::timestamptz - interval '6 days' + interval '08 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'beerenboss';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT '/uploads/uploadid09_profileid04.jpg',
       'Dunkelrote Kirschen, sehr aromatisch.',
       150, 8, 8, (:'seed_now')::timestamptz - interval '4 days' + interval '12 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'beerenboss';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT '/uploads/uploadid10_profileid04.jpg',
       'Kleine, sehr süße Trauben.',
       200, 12, 12, (:'seed_now')::timestamptz - interval '3 days' + interval '19 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'beerenboss';

-- 5 KürbisKönig: 2 Uploads
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT '/uploads/uploadid11_profileid05.jpg',
       'Hokkaido-Kürbis für Suppe.',
       1000, 25, 25, (:'seed_now')::timestamptz - interval '10 days' + interval '17 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'kürbiskönig';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT '/uploads/uploadid12_profileid05.jpg',
       'Zierkürbis für die Deko.',
       500, 15, 15, (:'seed_now')::timestamptz - interval '2 days' + interval '09 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'kürbiskönig';

-- 6 PaprikaPiratin: 2 Uploads
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT '/uploads/uploadid13_profileid06.jpg',
       'Rote Paprika, sehr aromatisch.',
       180, 7, 7, (:'seed_now')::timestamptz - interval '5 days' + interval '18 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'paprikapiratin';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT '/uploads/uploadid14_profileid06.jpg',
       'Gelbe Spitzpaprika, süß und mild.',
       160, 6, 8, (:'seed_now')::timestamptz - interval '2 days' + interval '16 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'paprikapiratin';

-- 7 MelonenMaster: 3 Uploads
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT '/uploads/uploadid15_profileid07.jpg',
       'Wassermelone, perfekt gekühlt.',
       3500, 30, 30, (:'seed_now')::timestamptz - interval '11 days' + interval '11 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'melonenmaster';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT '/uploads/uploadid16_profileid07.jpg',
       'Honigmelone mit intensivem Aroma.',
       2000, 20, 20, (:'seed_now')::timestamptz - interval '9 days' + interval '15 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'melonenmaster';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT '/uploads/uploadid17_profileid07.jpg',
       'Zuckermelone mit feiner Schale.',
       1800, 18, 18, (:'seed_now')::timestamptz - interval '1 days' + interval '20 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'melonenmaster';

-- 8 KartoffelKnight: 2 Uploads
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT '/uploads/uploadid18_profileid08.jpg',
       'Festkochende Kartoffeln.',
       2500, 25, 25, (:'seed_now')::timestamptz - interval '12 days' + interval '07 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'kartoffelknight';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT '/uploads/uploadid19_profileid08.jpg',
       'Mehligkochende Kartoffeln für Püree.',
       2600, 25, 25, (:'seed_now')::timestamptz - interval '6 days' + interval '20 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'kartoffelknight';

-- 9 KarottenKönigin: 2 Uploads
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT '/uploads/uploadid20_profileid09.jpg',
       'Bunte Karotten im Bund.',
       800, 10, 25, (:'seed_now')::timestamptz - interval '8 days' + interval '19 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'karottenkönigin';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT '/uploads/uploadid21_profileid09.jpg',
       'Mini-Karotten als Snack.',
       500, 8, 20, (:'seed_now')::timestamptz - interval '3 days' + interval '10 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'karottenkönigin';

-- 10 SalatSamurai: 1 Upload
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT '/uploads/uploadid22_profileid10.jpg',
       'Knackiger Blattsalat-Mix.',
       400, 20, 20, (:'seed_now')::timestamptz - interval '4 days' + interval '08 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'salatsamurai';

-- 11 ZwiebelZauberin: 2 Uploads
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT '/uploads/uploadid23_profileid11.jpg',
       'Rote Zwiebeln mit milder Schärfe.',
       700, 15, 15, (:'seed_now')::timestamptz - interval '7 days' + interval '21 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'zwiebelzauberin';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT '/uploads/uploadid24_profileid11.jpg',
       'Weiße Küchenzwiebeln für alles.',
       900, 18, 18, (:'seed_now')::timestamptz - interval '2 days' + interval '07 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'zwiebelzauberin';

-- 12 TraubenTaktiker: 2 Uploads
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT '/uploads/uploadid25_profileid12.jpg',
       'Grüne Tafeltrauben.',
       600, 15, 15, (:'seed_now')::timestamptz - interval '5 days' + interval '09 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'traubentaktiker';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT '/uploads/uploadid26_profileid12.jpg',
       'Blaue Trauben mit Kernen.',
       650, 15, 15, (:'seed_now')::timestamptz - interval '1 days' + interval '09 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'traubentaktiker';

-- 13 ApfelAlchemist: 2 Uploads
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT '/uploads/uploadid27_profileid13.jpg',
       'Roter Apfel, sehr knackig.',
       1500, 20, 20, (:'seed_now')::timestamptz - interval '6 days' + interval '10 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'apfelalchemist';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT '/uploads/uploadid28_profileid13.jpg',
       'Gemischte Apfelsorten aus eigener Zucht.',
       2000, 22, 22, (:'seed_now')::timestamptz - interval '2 days' + interval '20 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'apfelalchemist';

-- 14 BirnenBarde: 1 Upload
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT '/uploads/uploadid29_profileid14.jpg',
       'Saftige Birnen, direkt vom Baum.',
       1300, 18, 18, (:'seed_now')::timestamptz - interval '3 days' + interval '08 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'birnenbarde';

-- 15 PfirsichPilot: 2 Uploads
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT '/uploads/uploadid30_profileid15.jpg',
       'Reife Pfirsiche mit viel Duft.',
       1400, 18, 18, (:'seed_now')::timestamptz - interval '9 days' + interval '12 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'pfirsichpilot';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT '/uploads/uploadid31_profileid15.jpg',
       'Flache Weinbergpfirsiche.',
       1200, 17, 17, (:'seed_now')::timestamptz - interval '2 days' + interval '12 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'pfirsichpilot';

-- 16 BohnenBaron: 3 Uploads
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT '/uploads/uploadid32_profileid16.jpg',
       'Buschbohnen im Hochbeet.',
       900, 20, 20, (:'seed_now')::timestamptz - interval '10 days' + interval '08 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'bohnenbaron';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT '/uploads/uploadid33_profileid16.jpg',
       'Stangenbohnen entlang eines Rankgitters.',
       1100, 25, 25, (:'seed_now')::timestamptz - interval '6 days' + interval '11 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'bohnenbaron';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT '/uploads/uploadid34_profileid16.jpg',
       'Bunte Bohnenmischung für Eintöpfe.',
       800, 18, 18, (:'seed_now')::timestamptz - interval '1 days' + interval '18 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'bohnenbaron';

-- 17 SpinatSpion: 2 Uploads
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT '/uploads/uploadid35_profileid17.jpg',
       'Junger Spinat.',
       500, 18, 18, (:'seed_now')::timestamptz - interval '5 days' + interval '21 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'spinatspion';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT '/uploads/uploadid36_profileid17.jpg',
       'Spinatblätter für Pasta-Gerichte.',
       600, 20, 20, (:'seed_now')::timestamptz - interval '1 days' + interval '07 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'spinatspion';

-- 18 RadieschenRocker: 2 Uploads
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT '/uploads/uploadid37_profileid18.jpg',
       'Frische Radieschen mit kräftiger Schärfe.',
       300, 15, 15, (:'seed_now')::timestamptz - interval '4 days' + interval '19 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'radieschenrocker';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT '/uploads/uploadid38_profileid18.jpg',
       'Bunte Radieschenmischung als Dekoration.',
       350, 15, 15, (:'seed_now')::timestamptz - interval '2 days' + interval '10 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'radieschenrocker';

-- 19 BrokkoliBoss: 2 Uploads
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT '/uploads/uploadid39_profileid19.jpg',
       'Kräftiger Brokkoli für den Dampfgarer.',
       900, 20, 20, (:'seed_now')::timestamptz - interval '3 days' + interval '22 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'brokkoliboss';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT '/uploads/uploadid40_profileid19.jpg',
       'Brokkoliröschen für Wok-Gerichte.',
       850, 18, 18, (:'seed_now')::timestamptz - interval '1 days' + interval '12 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'brokkoliboss';

-- 20 MaisMagier: 3 Uploads
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT '/uploads/uploadid41_profileid20.jpg',
       'Maiskolben frisch vom Feld.',
       1200, 25, 25, (:'seed_now')::timestamptz - interval '7 days' + interval '08 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'maismagier';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT '/uploads/uploadid42_profileid20.jpg',
       'Maiskolben auf dem Grill.',
       1300, 25, 25, (:'seed_now')::timestamptz - interval '3 days' + interval '16 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'maismagier';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT '/uploads/uploadid43_profileid20.jpg',
       'Maiskörner für Bowls.',
       700, 18, 18, (:'seed_now')::timestamptz + interval '09 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'maismagier';

-- =============================
-- HARVESTTAGS
-- =============================
-- (unverändert)

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

-- Bohnen
INSERT INTO HARVESTTAGS (TagId, UploadId)
SELECT t.TagId, h.UploadId
FROM TAGS t
         JOIN HARVESTUPLOADS h ON h.Description ILIKE '%Bohne%'
   OR h.Description ILIKE '%Bohnen%'
WHERE t.Label = 'Bohnen';

-- Spinat
INSERT INTO HARVESTTAGS (TagId, UploadId)
SELECT t.TagId, h.UploadId
FROM TAGS t
         JOIN HARVESTUPLOADS h ON h.Description ILIKE '%Spinat%'
WHERE t.Label = 'Spinat';

-- Radieschen
INSERT INTO HARVESTTAGS (TagId, UploadId)
SELECT t.TagId, h.UploadId
FROM TAGS t
         JOIN HARVESTUPLOADS h ON h.Description ILIKE '%Radieschen%'
WHERE t.Label = 'Radieschen';

-- Brokkoli
INSERT INTO HARVESTTAGS (TagId, UploadId)
SELECT t.TagId, h.UploadId
FROM TAGS t
         JOIN HARVESTUPLOADS h ON h.Description ILIKE '%Brokkoli%'
WHERE t.Label = 'Brokkoli';

-- Mais
INSERT INTO HARVESTTAGS (TagId, UploadId)
SELECT t.TagId, h.UploadId
FROM TAGS t
         JOIN HARVESTUPLOADS h ON h.Description ILIKE '%Mais%'
WHERE t.Label = 'Mais';

-- =============================
-- RATINGS (inkl. „Matches“)
-- =============================

-- Match 1
INSERT INTO RATING (ContentCreatorId, ContentReceiverId, ProfileRating, RatingDate)
SELECT p1.ProfileId, p2.ProfileId, TRUE, (:'seed_now')::timestamptz - interval '2 days' + interval '11 hours'
FROM PROFILE p1, PROFILE p2
WHERE p1.UserName = 'tomatentiger'
  AND p2.UserName = 'beerenboss';

INSERT INTO RATING (ContentCreatorId, ContentReceiverId, ProfileRating, RatingDate)
SELECT p2.ProfileId, p1.ProfileId, TRUE, (:'seed_now')::timestamptz - interval '2 days' + interval '11 hours' + interval '35 minutes'
FROM PROFILE p1, PROFILE p2
WHERE p1.UserName = 'tomatentiger'
  AND p2.UserName = 'beerenboss';

-- Match 2
INSERT INTO RATING (ContentCreatorId, ContentReceiverId, ProfileRating, RatingDate)
SELECT p1.ProfileId, p2.ProfileId, TRUE, (:'seed_now')::timestamptz - interval '3 days' + interval '18 hours'
FROM PROFILE p1, PROFILE p2
WHERE p1.UserName = 'zucchinizauberer'
  AND p2.UserName = 'gurkenguru';

INSERT INTO RATING (ContentCreatorId, ContentReceiverId, ProfileRating, RatingDate)
SELECT p2.ProfileId, p1.ProfileId, TRUE, (:'seed_now')::timestamptz - interval '3 days' + interval '18 hours' + interval '22 minutes'
FROM PROFILE p1, PROFILE p2
WHERE p1.UserName = 'zucchinizauberer'
  AND p2.UserName = 'gurkenguru';

-- Match 3
INSERT INTO RATING (ContentCreatorId, ContentReceiverId, ProfileRating, RatingDate)
SELECT p1.ProfileId, p2.ProfileId, TRUE, (:'seed_now')::timestamptz - interval '1 days' + interval '20 hours'
FROM PROFILE p1, PROFILE p2
WHERE p1.UserName = 'melonenmaster'
  AND p2.UserName = 'beerenboss';

INSERT INTO RATING (ContentCreatorId, ContentReceiverId, ProfileRating, RatingDate)
SELECT p2.ProfileId, p1.ProfileId, TRUE, (:'seed_now')::timestamptz - interval '1 days' + interval '20 hours' + interval '08 minutes'
FROM PROFILE p1, PROFILE p2
WHERE p1.UserName = 'melonenmaster'
  AND p2.UserName = 'beerenboss';

-- Positives Rating ohne Match
INSERT INTO RATING (ContentCreatorId, ContentReceiverId, ProfileRating, RatingDate)
SELECT p1.ProfileId, p2.ProfileId, TRUE, (:'seed_now')::timestamptz - interval '4 days' + interval '09 hours'
FROM PROFILE p1, PROFILE p2
WHERE p1.UserName = 'apfelalchemist'
  AND p2.UserName = 'birnenbarde';

-- Negatives Rating
INSERT INTO RATING (ContentCreatorId, ContentReceiverId, ProfileRating, RatingDate)
SELECT p1.ProfileId, p2.ProfileId, FALSE, (:'seed_now')::timestamptz - interval '5 days' + interval '14 hours'
FROM PROFILE p1, PROFILE p2
WHERE p1.UserName = 'gurkenguru'
  AND p2.UserName = 'tomatentiger';

-- =============================
-- REPORTS (10 Meldungen)
-- =============================

INSERT INTO REPORT (Reason, ReportDate, UploadId)
SELECT 'Spam' AS Reason, (:'seed_now')::timestamptz - interval '20 hours' AS ReportDate,
       h.UploadId
FROM HARVESTUPLOADS h
    JOIN PROFILE p ON p.ProfileId = h.ProfileId
WHERE p.UserName = 'tomatentiger'
    LIMIT 1;

INSERT INTO REPORT (Reason, ReportDate, UploadId)
SELECT 'CatFishing' AS Reason, (:'seed_now')::timestamptz - interval '18 hours' AS ReportDate,
       h.UploadId
FROM HARVESTUPLOADS h
    JOIN PROFILE p ON p.ProfileId = h.ProfileId
WHERE p.UserName = 'tomatentiger'
    LIMIT 1;

INSERT INTO REPORT (Reason, ReportDate, UploadId)
SELECT 'CatFishing' AS Reason, (:'seed_now')::timestamptz - interval '16 hours' AS ReportDate,
       h.UploadId
FROM HARVESTUPLOADS h
    JOIN PROFILE p ON p.ProfileId = h.ProfileId
WHERE p.UserName = 'tomatentiger'
    LIMIT 1;

INSERT INTO REPORT (Reason, ReportDate, UploadId)
SELECT 'Spam' AS Reason, (:'seed_now')::timestamptz - interval '15 hours' AS ReportDate,
       h.UploadId
FROM HARVESTUPLOADS h
    JOIN PROFILE p ON p.ProfileId = h.ProfileId
WHERE p.UserName = 'tomatentiger'
    LIMIT 1;

INSERT INTO REPORT (Reason, ReportDate, UploadId)
SELECT 'AbstossendeInhalte' AS Reason, (:'seed_now')::timestamptz - interval '12 hours' AS ReportDate,
       h.UploadId
FROM HARVESTUPLOADS h
    JOIN PROFILE p ON p.ProfileId = h.ProfileId
WHERE p.UserName = 'kürbiskönig'
    LIMIT 1;

INSERT INTO REPORT (Reason, ReportDate, UploadId)
SELECT 'Spam' AS Reason, (:'seed_now')::timestamptz - interval '10 hours' AS ReportDate,
       h.UploadId
FROM HARVESTUPLOADS h
    JOIN PROFILE p ON p.ProfileId = h.ProfileId
WHERE p.UserName = 'melonenmaster'
    LIMIT 1;

INSERT INTO REPORT (Reason, ReportDate, UploadId)
SELECT 'AbstossendeInhalte' AS Reason, (:'seed_now')::timestamptz - interval '8 hours' AS ReportDate,
       h.UploadId
FROM HARVESTUPLOADS h
    JOIN PROFILE p ON p.ProfileId = h.ProfileId
WHERE p.UserName = 'paprikapiratin'
    LIMIT 1;

INSERT INTO REPORT (Reason, ReportDate, UploadId)
SELECT 'Spam' AS Reason, (:'seed_now')::timestamptz - interval '6 hours' AS ReportDate,
       h.UploadId
FROM HARVESTUPLOADS h
    JOIN PROFILE p ON p.ProfileId = h.ProfileId
WHERE p.UserName = 'kartoffelknight'
    LIMIT 1;

INSERT INTO REPORT (Reason, ReportDate, UploadId)
SELECT 'CatFishing' AS Reason, (:'seed_now')::timestamptz - interval '4 hours' AS ReportDate,
       h.UploadId
FROM HARVESTUPLOADS h
    JOIN PROFILE p ON p.ProfileId = h.ProfileId
WHERE p.UserName = 'traubentaktiker'
    LIMIT 1;

INSERT INTO REPORT (Reason, ReportDate, UploadId)
SELECT 'Spam' AS Reason, (:'seed_now')::timestamptz - interval '2 hours' AS ReportDate,
       h.UploadId
FROM HARVESTUPLOADS h
    JOIN PROFILE p ON p.ProfileId = h.ProfileId
WHERE p.UserName = 'brokkoliboss'
    LIMIT 1;