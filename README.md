# Monster Trading Card Game (MTCG)
By Jakob Friedl, if20b089 <br>
https://github.com/jakobfriedl/monster-trading-card-game<br>
Erstellt im Rahmen der Lehrveranstaltung Software Engineering (SWEN1) im Wintersemester 2021 an der Fachhochschule Technikum Wien

## Technical Steps 

Das Monster Trading Card Game ist ein Konsolen-basiertes Kartenspiel bei dem der Spieler mit einem selbst gebauten Deck aus unterschiedlichen Monster- und Spell-Karten gegen andere Spieler kämpft und seine Karten tauschen, kaufen oder verkaufen kann. <br>

### Programm-Aufbau 

Das Programm besteht aus einem Konsolen-Frontend, durch welches User-Eingaben über die CLI-Klasse aufgenommen werden und anschließend im Backend verarbeitet und in einer PostgreSQL-Datenbank persistiert werden. Die vollständige Architektur in Form eines UML-Diagramms kann weiter unten im Unterpunkt [UML-Diagramm](#uml-diagramm) angesehen werden. Für die User und Card Klasse wurde jeweils ein Interface verwendet. Dies wäre für die User-Klasse nicht unbedingt notwendig gewesen, da es nur eine Art von User gibt, jedoch könnte man das Programm mit einem Admin-Account erweitern, wodurch diese Klasse auch das IUser Interface implementiert. Zusätzlich kann durch bei Unit-Tests ein Mock der User-Klasse erstellt werden, ohne, dass eine Implementierung der Funktionalität notwendig ist. <br>

### Datenbank

Die PostgreSQL Datenbank besteht aus folgenden 5 Tabellen: 

- user: Relevante Informationen für User (Username, gehashtes Passwort, Coins, ...)
- card: Relevante Informationen für Karten (Name, Damage, Level, ...)
- battle: Battle Request von einem User an einen anderen
- offer: Angebotene Karte sowie Informationen zur gewünschten Karte
- transaction: Informationen über abgeschlossene Geschäfte, bei denen User Karten tauschen oder Coins ausgeben.

[Hier](Resources/db.png) kann das beschriebene Datenbank-Modell angesehen werden. 

### Features

Nach dem Starten des Programms hat der User zunächst 3 Befehle zur Verfügung. Mit Register kann sich der User einen neuen Account erstellen, mit Login kann sich der User mit einem bestehenden Account anmelden und mit Quit wird das Programm beendet. Loggt sich der User erfolgreich ein, so stehen ihm weitere Befehle zur Verfügung.

- Profile: Zeigt das Profil des Users an, hier können Statistiken, wie die Anzahl der ausgegebenen Coins, Siege, Verluste und Karten gesehen werden. Zusätzlich können jeweils das aktuelle Deck des Users und die anderen nicht verwendeten Karten betrachtet werden. Weiters ist es möglich das Passwort des Users unter Angabe des alten Passworts zu ändern, sowie die Transaction History, also Informationen über gekaufte Packages und vollendete Trades anzuzeigen. 
- Deck: Das aktuelle Kampfdeck angezeigt und kann auf Wunsch geändert werden.
- Buy: Der User kann für 5 Coins eines von 8 verschiedenen Packages kaufen. Es gibt Packages für jeden ElementTyp, sowie spezielle Packages für Spells und Monster.
- Trade: Hier findet der Handel mit anderen Usern statt. Ein Spieler kann eine seiner Karten auf dem Markt anbieten und beschreibt beim Angebot jene Karte, bzw. jenen Coin-Betrag den er dafür haben möchte. Die gestellten Angebote können im Nachhinein geändert oder gelöscht werden. Andere Spieler sehen die Angebote und tauschen dann entweder passende Karten oder zahlen Coins dafür. 
- Scores: Scoreboard mit allen Users, geordnet nach ELO.
- Battle: Der aktuelle User kann mit seinem Kampfdeck entweder gegen einen zufälligen Bot spielen, oder Battle-Requests an andere Spieler schicken, bzw. diese annehmen oder ablehnen.
- Logout: Session des Users wird beendet.
- Quit: Programm wird beendet. 

### Mandatory Unique Feature

Im Rahmen der Aufgabenstellung ist es notwendig, ein kreatives Feature einzubauen, das nicht in der Angabe vorgeschlagen wird. Ich habe mich dazu entschieden, ein Level-System für Karten zu implementieren. Alle Karten starten bei Level 0 mit 0 EXP und einer 0% Chance einen Critical Hit zu treffen und können durch das Gewinnen von Runden in Battles EXP bekommen. Die Menge der EXP pro Runde hängen vom eigenen Level der Karte, sowie vom Level der Karte ab, die besiegt wurde. Erreicht eine Karte 1000 EXP wird das Level - welches maximal 5 sein kann - erhöht und die Critical Hit Chance wird um 10% erhöht. Ein Critical Hit im Battle bedeutet, dass sich der Damage verdoppelt.  

### Vorgehensweise 

Die Vorgehensweise beim Programmieren wird im [Zeitprotokoll](#zeitprotokoll) sowie in der Git-Commit-History detailiert beschrieben. Grundsätzlich sah die Reihenfolge der Implementierung folgendermaßen aus:

- Notwendige Klassen erstellen
- Battle-Logik und Battle-Tests
- Anbindung der bestehenden Funktionalität an die Datenbank
- Weitere Must-Have Feature (Profile, Trading, Scores, Mandatory Unique Feature)
- Optionale Features  
  - Transaction History
  - Trade Cards vs Coins
  - neue Elemente (Ice, Electric, Ground) inklusive Tests
  - +1 Coin bei Win
  - Spent Coin Stats
  - Win/Loss Ratio
  - Extended Scoreboard
- UI Verbesserungen, Unit-Tests

### Failures

Der in der Angabe beschriebene REST-HTTP-Server wurde nicht implementiert, da in der Lehrveranstaltung vermittelt wurde, dass dieser nicht unbedingt für das Projekt notwendig ist. Eine Implementierung dieses Servers hätte zur Folge, dass das gesamte bestehende Projekt umgeschrieben werden muss, um die Anbindung und Kommunikation sicherzustellen. 

## Unit Tests

Das Test-Projekt gliedert sich in 3 Klassen, die jeweils einen unterschiedlichen Teil des Programms testen sollen.
Hauptsächlich wird mit Unit-Tests die Battle-Logik abgedeckt, da dadurch diese nach Änderungen leicht getestet und festgestellt werden kann, wo im Programm Fehler auftreten. Die Tests in dieser TestCardWeakness-Testklasse bezieht sich großteils auf die Element-Schwächen und -Stärken der Karten und überprüfen, ob diese Abhängigkeiten korrekt kalkuliert werden. Zusätzlich werden spezielle Sonderfälle von Kämpfen, wie beispielsweise, dass Drachen immer gegen Goblins gewinnen getestet. Die Tests der Battle-Logik wurden bereits vor der Implementation der Funktionalität geschrieben, sodass stets die aktuelle Version überprüft werden konnte. Die Klasse wurde mit dem Hinzufügen von 3 neuen Element-Typen auch mit neuen Tests erweitert. <br>

Die TestCardCollection-Klasse testet das Hinzufügen und Löschen von Karten aus den unterschiedlichen CardCollections, aber auch beispielsweise das automatische Generieren eines Decks aus dem CardStack, bei dem die stärksten 4 Karten ins Deck aufgenommen werden. <br>

Die TestPasswordHasher-Klasse testet die Security-Hash Funktion und überprüft ob ein Hash mit einem passenden Passwort verifiziert werden kann. <br>

## Zeitprotokoll

- 28/9/2021	    1h UML-Diagram
- 6/10/2021	    1h Classes
- 20/10/2021 	  2h Battle Logic & Unit Tests for Battle <br>
- 20/10/2021    1h Random-generated Stacks and Decks <br>
- 20/10/2021    1h Trying out Mocks
- 3/11/2021	    2h UI User Commands and other changes
- 28/11/2021	  2h Database Design
- 2/12/2021	    3h Database Implementation (Register)
- 7/12/2021     2h Database Implementation (Login)
- 11/12/2021	  2h Database Implementation (Buy)
- 28/12/2021	  3h PW Hash, Design, Profile, ...
- 29/12/2021	  3h Trading / Offers + DB
- 30/12/2021 	  1h Password Change <br>
- 30/12/2021    2h Manage Own Offers (Remove, Edit), Improve UI
- 1/1/2022	    2h Transaction History
- 2/1/2022	    2h Start Multiplayer Battle Requests, Improve UI
- 3/1/2022	    2h Finish Multiplayer Battle, Comment Functions in User Class
- 4/1/2022	    1h Mandatory Unique Feature (Electric, Ice, Ground Elements) 
- 5/1/2022	    3h Mandatory Unique Feature (Level System + extend Battle Log)
- 6/1/2022	    2h Added more information to battle log <br>
- 6/1/2022      2h Add Unit Tests for CardCollection and PasswordHash
- 7/1/2022      2h Protocol

Total: 42h

## UML-Diagramm

[PNG](./Resources/uml.png)<br>
[SVG](https://raw.githubusercontent.com/jakobfriedl/monster-trading-card-game/main/Resources/uml.svg)