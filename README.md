# 🍆 Willkommen zu EliteGärtner 

## Was ist EliteGärtner? 

### 🌱 Zeig, was in deinem Garten steckt!
EliteGärtner ist eine Plattform für alle, die ihr selbst angebautes Obst und Gemüse mit Stolz präsentieren möchten. Entdecke die Ernten anderer, lass dich inspirieren und vernetze dich mit Gartenbegeisterten aus deiner Region.

### 📸 Hochladen. Bewerten. Matchen.
Teile Bilder deiner Ernte, bewerte andere Gärtnerprofile und finde Gleichgesinnte – ganz nach dem Prinzip eines „Gardening-Tinders“. Passt ihr zueinander, entsteht ein Match und ihr könnt euch austauschen.

### 🏆 Werde Teil der Elite.
Ranglisten, Auszeichnungen und ein intelligenter Algorithmus sorgen dafür, dass hochwertige Inhalte sichtbar werden und Engagement belohnt wird. Ein integriertes Meldesystem garantiert dabei Qualität und Fairness innerhalb der Community.

---

## Wie starte ich EliteGärtner?

EliteGärtner setzt einen **Docker-Container mit PostgreSQL** voraus.

Die passende **Docker-Compose-Datei inklusive Seeding** befindet sich unter:

```
./EliteGaertner/SetUp/
```

Bitte in diesen Ordner wechseln und im Terminal / in der PowerShell folgenden Befehl ausführen:

```bash
  docker compose up -d
```

Anschließend mit folgendem Befehl überprüfen, ob der PostgreSQL-Container korrekt gestartet wurde:

```bash
  docker ps
```

---

## Datenbanken

Die Docker-Compose-Konfiguration setzt **zwei Datenbanken** auf:

### 🗄️ Produktivdatenbank
- **Name:** `elitegaertner`
- **Connection-URL:** `jdbc:postgresql://localhost:5432/elitegaertner`
- **User / Passwort:** `postgres`

### 🧪 Testdatenbank
- **Name:** `elitegaertner_test`
- **Connection-URL:** `jdbc:postgresql://localhost:5432/elitegaertner_test`
- **User / Passwort:** `postgres`

Während der Entwicklung wurde primär mit der Testdatenbank gearbeitet, um Funktionen flexibel testen zu können.  
Für die Präsentation wird ein fest definiertes Seeding der Produktivdatenbank verwendet.

EliteGärtner benötigt eine größere Datenmenge, damit Funktionen wie der **Vorschlags-Algorithmus** sinnvoll demonstriert werden können.

---

## Wie logge ich mich ein?

Grundsätzlich stehen Ihnen **alle Benutzer des Seedings** zur Verfügung.

Öffnen Sie dazu die Tabelle **`Profile`** in der Datenbank und wählen Sie einen Benutzer aus.  
⚠️ **Wichtig:** Beim Login wird die **E-Mail-Adresse**, nicht der Benutzername verwendet.

### Empfohlener Test-User

- **E-Mail:** `tomatentiger@elitegaertner.test`
- **Benutzername:** Tomatentiger
- **Passwort:** `Passwort1!`  
  *(Das Passwort ist bei allen Profilen identisch.)*

Dieses Profil wurde mehrfach von anderen Profilen bewertet, sodass neue Matches sehr schnell sichtbar werden.

➡️ Einfach die ersten vorgeschlagenen Profile **positiv bewerten**, dann sollten die ersten Matches ausgelöst werden.

In der Rangliste erscheint dieses Profil:
- einmal in den **Top 5**
- einmal **außerhalb der Top 5**
- und einmal **gar nicht**

Alternativ können Sie sich auch **neu registrieren**.  
Dabei ist zu beachten, dass neu registrierte Nutzer zunächst noch nicht bewertet wurden. Matches können jedoch **manuell getriggert** werden.

---

## Wie triggere ich Matches manuell?

Die Tabelle **`Ratings`** ist wie folgt aufgebaut:

- **ContentReceiver:** eingeloggter Benutzer (bewertet)
- **ContentCreator:** Benutzer, der bewertet wird
- **ProfileRating:**
    - `true` → positiv
    - `false` → negativ

Ein Match entsteht, wenn sich **ContentReceiver und ContentCreator gegenseitig positiv bewertet haben**.

### Vorgehen zum manuellen Triggern eines Matches

1. Loggen Sie sich mit einem Benutzer ein.
2. Prüfen Sie in der Tabelle **`Profile`**, welche `ProfileId` Sie besitzen.
3. Bewerten Sie mehrere Profile über die Weboberfläche.
4. Kontrollieren Sie anschließend in der Tabelle **`Ratings`**, ob die Bewertungen eingetragen wurden.
5. Tragen Sie nun eine **Gegenbewertung** ein:
    - `ContentReceiver` ↔ `ContentCreator` vertauschen
    - `ProfileRating` auf `true` setzen
6. Bewerten Sie anschließend ein weiteres Profil über die Webseite.

Bei jeder Bewertung aktualisiert das System die **ActiveMatchesList** und zeigt neue Matches an.  
Mehrere Gegenbewertungen können gleichzeitig eingetragen werden – eine **Match-Queue** sorgt dafür, dass neue Matches nacheinander angezeigt werden.

---

## Tests

Wir haben folgende Tests implementiert:

- **Unit-Tests**  
  Testen die Klassen der **AppLogic-Schicht** isoliert.

- **Integrationstests**  
  Testen die Klassen der **AppLogic-** und **DataManagement-Schicht** mit **Live-Daten aus der Testdatenbank**.

Ein Großteil der Tests wurde mithilfe von **ChatGPT** generiert.  
Da KI-Sprachmodelle mittlerweile sehr zuverlässige Testergebnisse liefern, konnten wir uns dadurch stärker auf andere Projektaspekte konzentrieren.

Zusätzlich war es notwendig, eine **Assembly-Setup-Klasse** zu implementieren, die vor dem Start der Tests automatisch:
- den Docker-Container initialisiert und
- das passende **Test-Seeding** einspielt.

Dabei trat die Herausforderung auf, dass das Test-Seeding **zwischen einzelnen Tests (TestInitialize)** wieder auf den ursprünglichen Zustand zurückgesetzt werden musste, da die Integrationstests sonst nicht zuverlässig reproduzierbar waren.

### ⚠️ Wichtiger Hinweis

In seltenen Fällen kann es vorkommen, dass die Assembly-Klasse den Docker-Container nicht korrekt initialisiert – insbesondere dann, wenn der Container bereits existiert.

**Falls alle Tests fehlschlagen**, bitte wie folgt vorgehen:

1. Terminal / PowerShell öffnen
2. In den `SetUp`-Ordner wechseln
3. Folgenden Befehl ausführen:

```bash
  docker compose down -v
```

Anschließend die Tests erneut starten.  
Der Docker-Container wird dann korrekt neu aufgebaut und initialisiert.


---

## Weitere Hinweise

Das Programm ist **vollständig funktional**.

- Rangliste testen?  
  → Eigene Uploads erstellen und gezielt Daten setzen, um z. B. in den Top 5 zu landen.
- Matching live testen?  
  → Mit zwei verschiedenen Benutzern in zwei separaten Browserfenstern anmelden und sich gegenseitig bewerten.

---

## Abschluss

Wir sind sehr stolz auf unsere **erste große Projektarbeit**.  
Wir hoffen, dass Sie beim Entdecken der Applikation genauso viel Spaß haben wie wir bei der Entwicklung.

**Beste Grüße**  
**Team Spassgetränk 🍹**