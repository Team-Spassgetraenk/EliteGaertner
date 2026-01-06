-- ===============================================
-- seed.sql – Testdaten für EliteGärtner (20 User)
-- ===============================================

\connect elitegaertner_test

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
    ('/pictures/Profilbilder/Profilbild_05.png', 'tomatentiger',     'Lukas',   'Schneider',  'tomatentiger@elitegaertner.test', '$2a$11$SpIw9Lq4nhdbkBINBmn7GepsBAp3ItvroIBryj3EDK2O4p1nQEIBm',
     '01511-0000001', 'Liebt saftige Tomaten und probiert jede Sorte einmal aus.', TRUE,  FALSE, (:'seed_now')::timestamptz - interval '60 days' + interval '08 hours'),
    ('/pictures/Profilbilder/Profilbild_12.png', 'zucchinizauberer', 'Anna',    'Bauer',      'zucchinizauberer@elitegaertner.test', '$2a$11$SpIw9Lq4nhdbkBINBmn7GepsBAp3ItvroIBryj3EDK2O4p1nQEIBm',
     '01511-0000002', 'Verwandelt Zucchini in Aufläufe, Kuchen und Magie.', TRUE, TRUE, (:'seed_now')::timestamptz - interval '58 days' + interval '11 hours'),
    ('/pictures/Profilbilder/Profilbild_19.png', 'gurkenguru',       'Max',     'Müller',     'gurkenguru@elitegaertner.test', '$2a$11$SpIw9Lq4nhdbkBINBmn7GepsBAp3ItvroIBryj3EDK2O4p1nQEIBm',
     '01511-0000003', 'Predigt täglich die Lehre der knackigen Gurke.', FALSE, TRUE, (:'seed_now')::timestamptz - interval '56 days' + interval '15 hours'),
    ('/pictures/Profilbilder/Profilbild_03.png', 'beerenboss',       'Julia',   'Weber',      'beerenboss@elitegaertner.test', '$2a$11$SpIw9Lq4nhdbkBINBmn7GepsBAp3ItvroIBryj3EDK2O4p1nQEIBm',
     '01511-0000004', 'Beherrscht das Reich der Erdbeeren, Kirschen und Trauben.', TRUE, TRUE, (:'seed_now')::timestamptz - interval '54 days' + interval '09 hours'),
    ('/pictures/Profilbilder/Profilbild_27.png', 'kürbiskönig',      'Leon',    'Fischer',    'kuerbiskoenig@elitegaertner.test', '$2a$11$SpIw9Lq4nhdbkBINBmn7GepsBAp3ItvroIBryj3EDK2O4p1nQEIBm',
     '01511-0000005', 'Regiert einen Garten voller Kürbisse in allen Größen.', TRUE, FALSE, (:'seed_now')::timestamptz - interval '52 days' + interval '18 hours'),
    ('/pictures/Profilbilder/Profilbild_08.png', 'paprikapiratin',   'Sarah',   'Wagner',     'paprikapiratin@elitegaertner.test', '$2a$11$SpIw9Lq4nhdbkBINBmn7GepsBAp3ItvroIBryj3EDK2O4p1nQEIBm',
     '01511-0000006', 'Kapert jede Paprika-Sorte, die ihr in die Finger kommt.', FALSE, TRUE, (:'seed_now')::timestamptz - interval '50 days' + interval '10 hours'),
    ('/pictures/Profilbilder/Profilbild_14.png', 'melonenmaster',    'Jonas',   'Hoffmann',   'melonenmaster@elitegaertner.test', '$2a$11$SpIw9Lq4nhdbkBINBmn7GepsBAp3ItvroIBryj3EDK2O4p1nQEIBm',
     '01511-0000007', 'Auf ewiger Mission nach der süßesten Melone aller Zeiten.', TRUE, TRUE, (:'seed_now')::timestamptz - interval '48 days' + interval '14 hours'),
    ('/pictures/Profilbilder/Profilbild_01.png', 'kartoffelknight',  'Laura',   'Becker',     'kartoffelknight@elitegaertner.test', '$2a$11$SpIw9Lq4nhdbkBINBmn7GepsBAp3ItvroIBryj3EDK2O4p1nQEIBm',
     '01511-0000008', 'Beschützt alte Kartoffelsorten wie ein wahrer Ritter.', TRUE, FALSE, (:'seed_now')::timestamptz - interval '46 days' + interval '07 hours'),
    ('/pictures/Profilbilder/Profilbild_22.png', 'karottenkönigin',  'David',   'Schulz',     'karottenkoenigin@elitegaertner.test', '$2a$11$SpIw9Lq4nhdbkBINBmn7GepsBAp3ItvroIBryj3EDK2O4p1nQEIBm',
     '01511-0000009', 'Regiert über ein Reich aus bunten Karotten.', TRUE, TRUE, (:'seed_now')::timestamptz - interval '44 days' + interval '12 hours'),
    ('/pictures/Profilbilder/Profilbild_10.png', 'salatsamurai',     'Nina',    'Keller',     'salatsamurai@elitegaertner.test', '$2a$11$SpIw9Lq4nhdbkBINBmn7GepsBAp3ItvroIBryj3EDK2O4p1nQEIBm',
     '01511-0000010', 'Schneidet Salate schneller als sein Schatten.', FALSE, TRUE, (:'seed_now')::timestamptz - interval '42 days' + interval '16 hours'),
    ('/pictures/Profilbilder/Profilbild_16.png', 'zwiebelzauberin',  'Felix',   'Braun',      'zwiebelzauberin@elitegaertner.test', '$2a$11$SpIw9Lq4nhdbkBINBmn7GepsBAp3ItvroIBryj3EDK2O4p1nQEIBm',
     '01511-0000011', 'Lässt Tränen fließen – aber nur beim Zwiebelschneiden.', TRUE, FALSE, (:'seed_now')::timestamptz - interval '40 days' + interval '09 hours'),
    ('/pictures/Profilbilder/Profilbild_06.png', 'traubentaktiker',  'Jana',    'Richter',    'traubentaktiker@elitegaertner.test', '$2a$11$SpIw9Lq4nhdbkBINBmn7GepsBAp3ItvroIBryj3EDK2O4p1nQEIBm',
     '01511-0000012', 'Plant jede Weinrebe wie einen Schachzug.', TRUE, TRUE, (:'seed_now')::timestamptz - interval '38 days' + interval '13 hours'),
    ('/pictures/Profilbilder/Profilbild_25.png', 'apfelalchemist',   'Tim',     'Vogel',      'apfelalchemist@elitegaertner.test', '$2a$11$SpIw9Lq4nhdbkBINBmn7GepsBAp3ItvroIBryj3EDK2O4p1nQEIBm',
     '01511-0000013', 'Veredelt Apfelbäume zu verrückten Sortenexperimenten.', TRUE, TRUE, (:'seed_now')::timestamptz - interval '36 days' + interval '17 hours'),
    ('/pictures/Profilbilder/Profilbild_04.png', 'birnenbarde',      'Lisa',    'König',      'birnenbarde@elitegaertner.test', '$2a$11$SpIw9Lq4nhdbkBINBmn7GepsBAp3ItvroIBryj3EDK2O4p1nQEIBm',
     '01511-0000014', 'Dichtet Oden über die perfekte Birne.', FALSE, TRUE, (:'seed_now')::timestamptz - interval '34 days' + interval '08 hours'),
    ('/pictures/Profilbilder/Profilbild_18.png', 'pfirsichpilot',    'Marco',   'Hartmann',   'pfirsichpilot@elitegaertner.test', '$2a$11$SpIw9Lq4nhdbkBINBmn7GepsBAp3ItvroIBryj3EDK2O4p1nQEIBm',
     '01511-0000015', 'Steuert direkt in Turbulenzen, wenn Pfirsichbäume reifen.', TRUE, FALSE, (:'seed_now')::timestamptz - interval '32 days' + interval '19 hours'),
    ('/pictures/Profilbilder/Profilbild_11.png', 'bohnenbaron',      'Oliver',  'Schmidt',    'bohnenbaron@elitegaertner.test', '$2a$11$SpIw9Lq4nhdbkBINBmn7GepsBAp3ItvroIBryj3EDK2O4p1nQEIBm',
     '01511-0000016', 'Bohnen in allen Farben, Formen und Höhenlagen.', TRUE, TRUE, (:'seed_now')::timestamptz - interval '30 days' + interval '10 hours'),
    ('/pictures/Profilbilder/Profilbild_23.png', 'spinatspion',      'Mia',     'Lehmann',    'spinatspion@elitegaertner.test', '$2a$11$SpIw9Lq4nhdbkBINBmn7GepsBAp3ItvroIBryj3EDK2O4p1nQEIBm',
     '01511-0000017', 'Schleicht nachts durch den Garten und checkt den Spinat.', TRUE, FALSE, (:'seed_now')::timestamptz - interval '28 days' + interval '14 hours'),
    ('/pictures/Profilbilder/Profilbild_02.png', 'radieschenrocker', 'Paul',    'Jung',       'radieschenrocker@elitegaertner.test', '$2a$11$SpIw9Lq4nhdbkBINBmn7GepsBAp3ItvroIBryj3EDK2O4p1nQEIBm',
     '01511-0000018', 'Spielt laute Musik, damit Radieschen schneller wachsen.', FALSE, TRUE, (:'seed_now')::timestamptz - interval '26 days' + interval '09 hours'),
    ('/pictures/Profilbilder/Profilbild_17.png', 'brokkoliboss',     'Emma',    'Franke',     'brokkoliboss@elitegaertner.test', '$2a$11$SpIw9Lq4nhdbkBINBmn7GepsBAp3ItvroIBryj3EDK2O4p1nQEIBm',
     '01511-0000019', 'Stellt Brokkoli in jedes Gericht – egal ob passend oder nicht.', TRUE, TRUE, (:'seed_now')::timestamptz - interval '24 days' + interval '12 hours'),
    ('/pictures/Profilbilder/Profilbild_28.png', 'maismagier',       'Noah',    'Seidel',     'maismagier@elitegaertner.test', '$2a$11$SpIw9Lq4nhdbkBINBmn7GepsBAp3ItvroIBryj3EDK2O4p1nQEIBm',
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
SELECT 'pictures/uploads/uploadid01_profileid01.png',
       'Rote Tomaten aus dem Gewächshaus.',
       180, 7, 7, (:'seed_now')::timestamptz - interval '9 days' + interval '08 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'tomatentiger';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'pictures/uploads/uploadid02_profileid01.png',
       'Fleischtomate, perfekt für Soßen.',
       250, 9, 8, (:'seed_now')::timestamptz - interval '8 days' + interval '14 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'tomatentiger';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'pictures/uploads/uploadid03_profileid01.png',
       'Gelbe Tomaten, mild im Geschmack.',
       160, 6, 6, (:'seed_now')::timestamptz - interval '7 days' + interval '18 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'tomatentiger';

-- 2 ZucchiniZauberer: 2 Uploads
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'pictures/uploads/uploadid04_profileid02.png',
       'Lange Zucchini, direkt vom Hochbeet.',
       320, 25, 5, (:'seed_now')::timestamptz - interval '8 days' + interval '09 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'zucchinizauberer';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'pictures/uploads/uploadid05_profileid02.png',
       'Runde Zucchini für gefüllte Gerichte.',
       400, 15, 15, (:'seed_now')::timestamptz - interval '6 days' + interval '16 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'zucchinizauberer';

-- 3 GurkenGuru: 1 Upload
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'pictures/uploads/uploadid06_profileid03.png',
       'Gurke, super knackig.',
       300, 25, 18, (:'seed_now')::timestamptz - interval '7 days' + interval '10 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'gurkenguru';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'pictures/uploads/uploadid07_profileid03.png',
       'Einlegegurken für den Winter.',
       200, 15, 4, (:'seed_now')::timestamptz - interval '5 days' + interval '13 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'birnenbarde';

-- 4 BeerenBoss: 3 Uploads
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'pictures/uploads/uploadid08_profileid04.png',
       'Süße Erdbeeren vom Feld.',
       120, 10, 10, (:'seed_now')::timestamptz - interval '6 days' + interval '08 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'beerenboss';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'pictures/uploads/uploadid09_profileid04.png',
       'Dunkelrote Kirschen, sehr aromatisch.',
       150, 8, 8, (:'seed_now')::timestamptz - interval '4 days' + interval '12 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'beerenboss';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'pictures/uploads/uploadid10_profileid04.png',
       'Kleine, sehr süße Trauben.',
       200, 12, 12, (:'seed_now')::timestamptz - interval '3 days' + interval '19 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'beerenboss';

-- 5 KürbisKönig: 2 Uploads
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'pictures/uploads/uploadid11_profileid05.png',
       'Hokkaido-Kürbis für Suppe.',
       1000, 25, 25, (:'seed_now')::timestamptz - interval '10 days' + interval '17 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'kürbiskönig';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'pictures/uploads/uploadid12_profileid05.png',
       'Zierkürbis für die Deko.',
       500, 15, 15, (:'seed_now')::timestamptz - interval '2 days' + interval '09 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'kürbiskönig';

-- 6 PaprikaPiratin: 2 Uploads
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'pictures/uploads/uploadid13_profileid06.png',
       'Rote Paprika, sehr aromatisch.',
       180, 7, 7, (:'seed_now')::timestamptz - interval '5 days' + interval '18 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'paprikapiratin';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'pictures/uploads/uploadid14_profileid06.png',
       'Gelbe Spitzpaprika, süß und mild.',
       160, 6, 8, (:'seed_now')::timestamptz - interval '2 days' + interval '16 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'paprikapiratin';

-- 7 MelonenMaster: 3 Uploads
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'pictures/uploads/uploadid15_profileid07.png',
       'Wassermelone, perfekt gekühlt.',
       3500, 30, 30, (:'seed_now')::timestamptz - interval '11 days' + interval '11 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'melonenmaster';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'pictures/uploads/uploadid16_profileid07.png',
       'Honigmelone mit intensivem Aroma.',
       2000, 20, 20, (:'seed_now')::timestamptz - interval '9 days' + interval '15 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'melonenmaster';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'pictures/uploads/uploadid17_profileid07.png',
       'Zuckermelone mit feiner Schale.',
       1800, 18, 18, (:'seed_now')::timestamptz - interval '1 days' + interval '20 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'melonenmaster';

-- 8 KartoffelKnight: 2 Uploads
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'pictures/uploads/uploadid18_profileid08.png',
       'Festkochende Kartoffeln.',
       2500, 25, 25, (:'seed_now')::timestamptz - interval '12 days' + interval '07 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'kartoffelknight';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'pictures/uploads/uploadid19_profileid08.png',
       'Mehligkochende Kartoffeln für Püree.',
       2600, 25, 25, (:'seed_now')::timestamptz - interval '6 days' + interval '20 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'kartoffelknight';

-- 9 KarottenKönigin: 2 Uploads
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'pictures/uploads/uploadid20_profileid09.png',
       'Bunte Karotten im Bund.',
       800, 10, 25, (:'seed_now')::timestamptz - interval '8 days' + interval '19 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'karottenkönigin';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'pictures/uploads/uploadid21_profileid09.png',
       'Mini-Karotten als Snack.',
       500, 8, 20, (:'seed_now')::timestamptz - interval '3 days' + interval '10 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'karottenkönigin';

-- 10 SalatSamurai: 1 Upload
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'pictures/uploads/uploadid22_profileid10.png',
       'Knackiger Blattsalat-Mix.',
       400, 20, 20, (:'seed_now')::timestamptz - interval '4 days' + interval '08 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'salatsamurai';

-- 11 ZwiebelZauberin: 2 Uploads
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'pictures/uploads/uploadid23_profileid11.png',
       'Rote Zwiebeln mit milder Schärfe.',
       700, 15, 15, (:'seed_now')::timestamptz - interval '7 days' + interval '21 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'zwiebelzauberin';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'pictures/uploads/uploadid24_profileid11.png',
       'Weiße Küchenzwiebeln für alles.',
       900, 18, 18, (:'seed_now')::timestamptz - interval '2 days' + interval '07 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'zwiebelzauberin';

-- 12 TraubenTaktiker: 2 Uploads
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'pictures/uploads/uploadid25_profileid12.png',
       'Grüne Tafeltrauben.',
       600, 15, 15, (:'seed_now')::timestamptz - interval '5 days' + interval '09 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'traubentaktiker';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'pictures/uploads/uploadid26_profileid12.png',
       'Blaue Trauben mit Kernen.',
       650, 15, 15, (:'seed_now')::timestamptz - interval '1 days' + interval '09 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'traubentaktiker';

-- 13 ApfelAlchemist: 2 Uploads
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'pictures/uploads/uploadid27_profileid13.png',
       'Roter Apfel, sehr knackig.',
       1500, 20, 20, (:'seed_now')::timestamptz - interval '6 days' + interval '10 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'apfelalchemist';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'pictures/uploads/uploadid28_profileid13.png',
       'Gemischte Apfelsorten aus eigener Zucht.',
       2000, 22, 22, (:'seed_now')::timestamptz - interval '2 days' + interval '20 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'apfelalchemist';

-- 14 BirnenBarde: 2 Uploads
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'pictures/uploads/uploadid29_profileid14.png',
       'Saftige Birnen, direkt vom Baum.',
       1300, 18, 18, (:'seed_now')::timestamptz - interval '3 days' + interval '08 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'birnenbarde';

-- 15 PfirsichPilot: 2 Uploads
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'pictures/uploads/uploadid30_profileid15.png',
       'Reife Pfirsiche mit viel Duft.',
       1400, 18, 18, (:'seed_now')::timestamptz - interval '9 days' + interval '12 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'pfirsichpilot';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'pictures/uploads/uploadid31_profileid15.png',
       'Flache Weinbergpfirsiche.',
       1200, 17, 17, (:'seed_now')::timestamptz - interval '2 days' + interval '12 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'pfirsichpilot';

-- 16 BohnenBaron: 3 Uploads
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'pictures/uploads/uploadid32_profileid16.png',
       'Buschbohnen im Hochbeet.',
       900, 20, 20, (:'seed_now')::timestamptz - interval '10 days' + interval '08 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'bohnenbaron';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'pictures/uploads/uploadid33_profileid16.png',
       'Stangenbohnen entlang eines Rankgitters.',
       1100, 25, 25, (:'seed_now')::timestamptz - interval '6 days' + interval '11 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'bohnenbaron';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'pictures/uploads/uploadid34_profileid16.png',
       'Bunte Bohnenmischung für Eintöpfe.',
       800, 18, 18, (:'seed_now')::timestamptz - interval '1 days' + interval '18 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'bohnenbaron';

-- 17 SpinatSpion: 2 Uploads
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'pictures/uploads/uploadid35_profileid17.png',
       'Junger Spinat.',
       500, 18, 18, (:'seed_now')::timestamptz - interval '5 days' + interval '21 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'spinatspion';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'pictures/uploads/uploadid36_profileid17.png',
       'Spinatblätter für Pasta-Gerichte.',
       600, 20, 20, (:'seed_now')::timestamptz - interval '1 days' + interval '07 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'spinatspion';

-- 18 RadieschenRocker: 2 Uploads
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'pictures/uploads/uploadid37_profileid18.png',
       'Frische Radieschen mit kräftiger Schärfe.',
       300, 15, 15, (:'seed_now')::timestamptz - interval '4 days' + interval '19 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'radieschenrocker';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'pictures/uploads/uploadid38_profileid18.png',
       'Bunte Radieschenmischung als Dekoration.',
       350, 15, 15, (:'seed_now')::timestamptz - interval '2 days' + interval '10 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'radieschenrocker';

-- 19 BrokkoliBoss: 2 Uploads
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'pictures/uploads/uploadid39_profileid19.png',
       'Kräftiger Brokkoli für den Dampfgarer.',
       900, 20, 20, (:'seed_now')::timestamptz - interval '3 days' + interval '22 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'brokkoliboss';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'pictures/uploads/uploadid40_profileid19.png',
       'Brokkoliröschen für Wok-Gerichte.',
       850, 18, 18, (:'seed_now')::timestamptz - interval '1 days' + interval '12 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'brokkoliboss';

-- 20 MaisMagier: 3 Uploads
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'pictures/uploads/uploadid41_profileid20.png',
       'Maiskolben frisch vom Feld.',
       1200, 25, 25, (:'seed_now')::timestamptz - interval '7 days' + interval '08 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'maismagier';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'pictures/uploads/uploadid42_profileid20.png',
       'Maiskolben auf dem Grill.',
       1300, 25, 25, (:'seed_now')::timestamptz - interval '3 days' + interval '16 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'maismagier';


INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'pictures/uploads/uploadid43_profileid20.png',
       'Maiskörner für Bowls.',
       700, 18, 18, (:'seed_now')::timestamptz + interval '09 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'maismagier';

-- =============================
-- ZUSÄTZLICHE HARVESTUPLOADS (Bilder-Pool)
-- (zusätzliche Bilder, verteilt auf Profile)
-- =============================

-- Auberginen (3)
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'pictures/uploads/Auberginen_01.jpg',
       'Frisch geerntete Auberginen aus dem Garten.',
       420, 8, 18, (:'seed_now')::timestamptz - interval '6 days' + interval '09 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'zucchinizauberer';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'pictures/uploads/Auberginen_02.jpg',
       'Auberginen – glänzend und direkt vom Beet.',
       380, 7, 17, (:'seed_now')::timestamptz - interval '5 days' + interval '18 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'paprikapiratin';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'pictures/uploads/Auberginen_03.jpg',
       'Zwei Auberginen frisch geerntet, bereit zum Kochen.',
       560, 9, 20, (:'seed_now')::timestamptz - interval '4 days' + interval '12 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'apfelalchemist';

-- Erdbeeren (1)
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'pictures/uploads/Erdbeeren_01.jpg',
       'Erdbeeren – süß und frisch vom Feld.',
       350, 14, 14, (:'seed_now')::timestamptz - interval '2 days' + interval '08 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'beerenboss';

-- Gurken (5)
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'pictures/uploads/Gurken_01.jpg',
       'Gurke, frisch geerntet und super knackig.',
       320, 26, 29, (:'seed_now')::timestamptz - interval '3 days' + interval '10 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'karottenkönigin';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'pictures/uploads/Gurken_02.jpg',
       'Gurke für den Salat – direkt aus dem Hochbeet.',
       290, 24, 22, (:'seed_now')::timestamptz - interval '2 days' + interval '18 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'salatsamurai';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'pictures/uploads/Gurken_03.jpg',
       'Gurke – frisch geerntet, noch mit etwas Erde.',
       310, 25, 24, (:'seed_now')::timestamptz - interval '2 days' + interval '11 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'spinatspion';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'pictures/uploads/Gurken_04.jpg',
       'Gurken-Ernte: eine besonders gerade Gurke.',
       340, 27, 26, (:'seed_now')::timestamptz - interval '1 days' + interval '19 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'zucchinizauberer';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'pictures/uploads/Gurken_05.jpg',
       'Gurke aus dem Gewächshaus – frisch geerntet.',
       360, 28, 1, (:'seed_now')::timestamptz - interval '1 days' + interval '08 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'tomatentiger';

-- Karotten (2)
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'pictures/uploads/Karotten_01.jpg',
       'Karotten im Bund, frisch aus der Erde gezogen.',
       900, 10, 28, (:'seed_now')::timestamptz - interval '6 days' + interval '14 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'karottenkönigin';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'pictures/uploads/Karotten_02.png',
       'Karotten – bunt gemischt und frisch geerntet.',
       780, 9, 26, (:'seed_now')::timestamptz - interval '5 days' + interval '09 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'bohnenbaron';

-- Kürbisse (2)
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'pictures/uploads/Kürbisse_01.jpg',
       'Kürbis frisch geerntet – perfekt für Suppe.',
       1400, 24, 24, (:'seed_now')::timestamptz - interval '8 days' + interval '17 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'kürbiskönig';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'pictures/uploads/Kürbisse_02.jpg',
       'Kleine Kürbisse für Deko, frisch geerntet.',
       650, 16, 16, (:'seed_now')::timestamptz - interval '7 days' + interval '12 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'maismagier';

-- Mais (3)
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'pictures/uploads/Mais_01.png',
       'Maiskolben frisch geerntet vom Feld.',
       1250, 26, 6, (:'seed_now')::timestamptz - interval '6 days' + interval '08 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'maismagier';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'pictures/uploads/Mais_02.png',
       'Mais – zwei Kolben frisch geerntet.',
       2100, 28, 7, (:'seed_now')::timestamptz - interval '4 days' + interval '16 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'kürbiskönig';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'pictures/uploads/Mais_03.png',
       'Maiskolben – goldgelb und frisch geerntet.',
       1150, 25, 6, (:'seed_now')::timestamptz - interval '3 days' + interval '07 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'kartoffelknight';

-- Melonen (1)
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'pictures/uploads/Melonen_01.jpg',
       'Melone – frisch geerntet und bereit zum Kühlen.',
       2400, 22, 22, (:'seed_now')::timestamptz - interval '9 days' + interval '13 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'melonenmaster';

-- Radieschen (4)
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'pictures/uploads/Radieschen_01.jpg',
       'Radieschen – frisch geerntet und richtig scharf.',
       320, 16, 16, (:'seed_now')::timestamptz - interval '3 days' + interval '19 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'radieschenrocker';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'pictures/uploads/Radieschen_02.jpg',
       'Radieschen als Topping, frisch geerntet.',
       280, 15, 15, (:'seed_now')::timestamptz - interval '2 days' + interval '09 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'salatsamurai';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'pictures/uploads/Radieschen_03.jpg',
       'Radieschen aus dem Beet – frisch geerntet.',
       300, 15, 15, (:'seed_now')::timestamptz - interval '1 days' + interval '11 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'zwiebelzauberin';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'pictures/uploads/Radieschen_04.jpg',
       'Bunte Radieschen – frisch geerntet und fotogen.',
       340, 16, 16, (:'seed_now')::timestamptz - interval '1 days' + interval '06 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'beerenboss';

-- Tomaten (1)
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'pictures/uploads/Tomaten_01.jpg',
       'Tomaten frisch geerntet – ideal für Caprese.',
       520, 14, 14, (:'seed_now')::timestamptz - interval '2 days' + interval '14 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'tomatentiger';

-- Trauben (2)
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'pictures/uploads/Trauben_01.jpg',
       'Trauben – frisch geerntet und sehr süß.',
       700, 16, 16, (:'seed_now')::timestamptz - interval '4 days' + interval '09 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'traubentaktiker';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'pictures/uploads/Trauben_02.jpg',
       'Trauben aus dem Garten – frisch geerntet.',
       820, 17, 17, (:'seed_now')::timestamptz - interval '3 days' + interval '08 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'birnenbarde';

-- Zwiebeln (4)
INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'pictures/uploads/Zwiebeln_01.jpg',
       'Zwiebeln – frisch geerntet und küchenbereit.',
       900, 18, 18, (:'seed_now')::timestamptz - interval '6 days' + interval '21 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'zwiebelzauberin';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'pictures/uploads/Zwiebeln_02.jpg',
       'Zwiebeln frisch geerntet – milde Schärfe.',
       850, 17, 17, (:'seed_now')::timestamptz - interval '5 days' + interval '07 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'kartoffelknight';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'pictures/uploads/Zwiebeln_03.jpg',
       'Zwiebeln für den Salat – frisch geerntet.',
       780, 16, 16, (:'seed_now')::timestamptz - interval '3 days' + interval '16 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'salatsamurai';

INSERT INTO HARVESTUPLOADS
(ImageUrl, Description, WeightGramm, WidthCm, LengthCm, UploadDate, ProfileId)
SELECT 'pictures/uploads/Zwiebeln_04.jpg',
       'Zwiebeln – frisch geerntet, kräftig im Aroma.',
       920, 18, 18, (:'seed_now')::timestamptz - interval '2 days' + interval '19 hours', p.ProfileId
FROM PROFILE p WHERE p.UserName = 'brokkoliboss';


-- =============================
-- HARVESTTAGS
-- =============================
-- (unverändert)

-- Auberginen
INSERT INTO HARVESTTAGS (TagId, UploadId)
SELECT t.TagId, h.UploadId
FROM TAGS t
         JOIN HARVESTUPLOADS h ON h.Description ILIKE '%Aubergin%'
WHERE t.Label = 'Auberginen';

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

-- ==================================================
-- Zusätzliche Ratings für mehr Verteilung / Matches
-- ==================================================

-- TomatenTiger bekommt mehrere Likes, liked aber NICHT zurück (kein Match)
INSERT INTO RATING (ContentCreatorId, ContentReceiverId, ProfileRating, RatingDate)
SELECT p1.ProfileId, p2.ProfileId, TRUE, (:'seed_now')::timestamptz - interval '4 days' + interval '13 hours'
FROM PROFILE p1, PROFILE p2
WHERE p1.UserName = 'tomatentiger'
  AND p2.UserName = 'zucchinizauberer';

INSERT INTO RATING (ContentCreatorId, ContentReceiverId, ProfileRating, RatingDate)
SELECT p1.ProfileId, p2.ProfileId, TRUE, (:'seed_now')::timestamptz - interval '3 days' + interval '09 hours'
FROM PROFILE p1, PROFILE p2
WHERE p1.UserName = 'tomatentiger'
  AND p2.UserName = 'paprikapiratin';

INSERT INTO RATING (ContentCreatorId, ContentReceiverId, ProfileRating, RatingDate)
SELECT p1.ProfileId, p2.ProfileId, TRUE, (:'seed_now')::timestamptz - interval '3 days' + interval '12 hours' + interval '18 minutes'
FROM PROFILE p1, PROFILE p2
WHERE p1.UserName = 'tomatentiger'
  AND p2.UserName = 'kartoffelknight';

INSERT INTO RATING (ContentCreatorId, ContentReceiverId, ProfileRating, RatingDate)
SELECT p1.ProfileId, p2.ProfileId, TRUE, (:'seed_now')::timestamptz - interval '2 days' + interval '17 hours'
FROM PROFILE p1, PROFILE p2
WHERE p1.UserName = 'tomatentiger'
  AND p2.UserName = 'melonenmaster';

INSERT INTO RATING (ContentCreatorId, ContentReceiverId, ProfileRating, RatingDate)
SELECT p1.ProfileId, p2.ProfileId, TRUE, (:'seed_now')::timestamptz - interval '2 days' + interval '10 hours' + interval '07 minutes'
FROM PROFILE p1, PROFILE p2
WHERE p1.UserName = 'tomatentiger'
  AND p2.UserName = 'apfelalchemist';

-- Mehr Matches (gegenseitige Likes)
-- Match 4: PaprikaPiratin <-> MelonenMaster
INSERT INTO RATING (ContentCreatorId, ContentReceiverId, ProfileRating, RatingDate)
SELECT p1.ProfileId, p2.ProfileId, TRUE, (:'seed_now')::timestamptz - interval '6 days' + interval '18 hours'
FROM PROFILE p1, PROFILE p2
WHERE p1.UserName = 'paprikapiratin'
  AND p2.UserName = 'melonenmaster';

INSERT INTO RATING (ContentCreatorId, ContentReceiverId, ProfileRating, RatingDate)
SELECT p2.ProfileId, p1.ProfileId, TRUE, (:'seed_now')::timestamptz - interval '6 days' + interval '18 hours' + interval '19 minutes'
FROM PROFILE p1, PROFILE p2
WHERE p1.UserName = 'paprikapiratin'
  AND p2.UserName = 'melonenmaster';

-- Match 5: KartoffelKnight <-> ZwiebelZauberin
INSERT INTO RATING (ContentCreatorId, ContentReceiverId, ProfileRating, RatingDate)
SELECT p1.ProfileId, p2.ProfileId, TRUE, (:'seed_now')::timestamptz - interval '7 days' + interval '09 hours'
FROM PROFILE p1, PROFILE p2
WHERE p1.UserName = 'kartoffelknight'
  AND p2.UserName = 'zwiebelzauberin';

INSERT INTO RATING (ContentCreatorId, ContentReceiverId, ProfileRating, RatingDate)
SELECT p2.ProfileId, p1.ProfileId, TRUE, (:'seed_now')::timestamptz - interval '7 days' + interval '09 hours' + interval '41 minutes'
FROM PROFILE p1, PROFILE p2
WHERE p1.UserName = 'kartoffelknight'
  AND p2.UserName = 'zwiebelzauberin';

-- Match 6: BohnenBaron <-> SpinatSpion
INSERT INTO RATING (ContentCreatorId, ContentReceiverId, ProfileRating, RatingDate)
SELECT p1.ProfileId, p2.ProfileId, TRUE, (:'seed_now')::timestamptz - interval '5 days' + interval '20 hours'
FROM PROFILE p1, PROFILE p2
WHERE p1.UserName = 'bohnenbaron'
  AND p2.UserName = 'spinatspion';

INSERT INTO RATING (ContentCreatorId, ContentReceiverId, ProfileRating, RatingDate)
SELECT p2.ProfileId, p1.ProfileId, TRUE, (:'seed_now')::timestamptz - interval '5 days' + interval '20 hours' + interval '12 minutes'
FROM PROFILE p1, PROFILE p2
WHERE p1.UserName = 'bohnenbaron'
  AND p2.UserName = 'spinatspion';

-- Match 7: MaisMagier <-> KürbisKönig
INSERT INTO RATING (ContentCreatorId, ContentReceiverId, ProfileRating, RatingDate)
SELECT p1.ProfileId, p2.ProfileId, TRUE, (:'seed_now')::timestamptz - interval '8 days' + interval '16 hours'
FROM PROFILE p1, PROFILE p2
WHERE p1.UserName = 'maismagier'
  AND p2.UserName = 'kürbiskönig';

INSERT INTO RATING (ContentCreatorId, ContentReceiverId, ProfileRating, RatingDate)
SELECT p2.ProfileId, p1.ProfileId, TRUE, (:'seed_now')::timestamptz - interval '8 days' + interval '16 hours' + interval '33 minutes'
FROM PROFILE p1, PROFILE p2
WHERE p1.UserName = 'maismagier'
  AND p2.UserName = 'kürbiskönig';

-- Match 8: TraubenTaktiker <-> ApfelAlchemist
INSERT INTO RATING (ContentCreatorId, ContentReceiverId, ProfileRating, RatingDate)
SELECT p1.ProfileId, p2.ProfileId, TRUE, (:'seed_now')::timestamptz - interval '9 days' + interval '11 hours'
FROM PROFILE p1, PROFILE p2
WHERE p1.UserName = 'traubentaktiker'
  AND p2.UserName = 'apfelalchemist';

INSERT INTO RATING (ContentCreatorId, ContentReceiverId, ProfileRating, RatingDate)
SELECT p2.ProfileId, p1.ProfileId, TRUE, (:'seed_now')::timestamptz - interval '9 days' + interval '11 hours' + interval '27 minutes'
FROM PROFILE p1, PROFILE p2
WHERE p1.UserName = 'traubentaktiker'
  AND p2.UserName = 'apfelalchemist';

-- Match 9: KarottenKönigin <-> SalatSamurai
INSERT INTO RATING (ContentCreatorId, ContentReceiverId, ProfileRating, RatingDate)
SELECT p1.ProfileId, p2.ProfileId, TRUE, (:'seed_now')::timestamptz - interval '6 days' + interval '07 hours'
FROM PROFILE p1, PROFILE p2
WHERE p1.UserName = 'karottenkönigin'
  AND p2.UserName = 'salatsamurai';

INSERT INTO RATING (ContentCreatorId, ContentReceiverId, ProfileRating, RatingDate)
SELECT p2.ProfileId, p1.ProfileId, TRUE, (:'seed_now')::timestamptz - interval '6 days' + interval '07 hours' + interval '08 minutes'
FROM PROFILE p1, PROFILE p2
WHERE p1.UserName = 'karottenkönigin'
  AND p2.UserName = 'salatsamurai';

-- Zusätzliche verteilte Likes ohne Match
INSERT INTO RATING (ContentCreatorId, ContentReceiverId, ProfileRating, RatingDate)
SELECT p1.ProfileId, p2.ProfileId, TRUE, (:'seed_now')::timestamptz - interval '4 days' + interval '19 hours'
FROM PROFILE p1, PROFILE p2
WHERE p1.UserName = 'beerenboss'
  AND p2.UserName = 'traubentaktiker';

INSERT INTO RATING (ContentCreatorId, ContentReceiverId, ProfileRating, RatingDate)
SELECT p1.ProfileId, p2.ProfileId, TRUE, (:'seed_now')::timestamptz - interval '4 days' + interval '19 hours' + interval '26 minutes'
FROM PROFILE p1, PROFILE p2
WHERE p1.UserName = 'melonenmaster'
  AND p2.UserName = 'kartoffelknight';

INSERT INTO RATING (ContentCreatorId, ContentReceiverId, ProfileRating, RatingDate)
SELECT p1.ProfileId, p2.ProfileId, TRUE, (:'seed_now')::timestamptz - interval '3 days' + interval '21 hours'
FROM PROFILE p1, PROFILE p2
WHERE p1.UserName = 'brokkoliboss'
  AND p2.UserName = 'bohnenbaron';

INSERT INTO RATING (ContentCreatorId, ContentReceiverId, ProfileRating, RatingDate)
SELECT p1.ProfileId, p2.ProfileId, FALSE, (:'seed_now')::timestamptz - interval '3 days' + interval '22 hours'
FROM PROFILE p1, PROFILE p2
WHERE p1.UserName = 'radieschenrocker'
  AND p2.UserName = 'salatsamurai';

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