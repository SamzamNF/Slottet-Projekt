# Slottet Projekt

Slottet er et fuldstack system til håndtering af beboere for Østruplund, som personalet kan bruge til at holde styr på de daglige dage for beboerne

## Indhold

- [Opsætning og kørsel](#opsætning-og-kørsel)
- [Systembeskrivelse](#systembeskrivelse)
- [Teknologier](#teknologier)
- [Hvem har lavet projektet](#hvem-har-lavet-projektet)
- [Arkitektur](#arkitektur)
- [Projektstruktur](#projektstruktur)
- [Krav for at køre projektet](#krav-for-at-køre-projektet)
- [Database](#database)
- [Udvikling lokalt](#udvikling-lokalt)
- [API-moduler](#api-moduler)
- [Frontend](#frontend)
- [Test](#test)
- [Bemærkninger](#bemærkninger)

## Opsætning og kørsel

Den nemmeste måde at køre projektet på er med Docker.

### Sådan starter du projektet

1. Sørg for at Docker Desktop er installeret.
2. Start Docker Desktop og vent til det kører.
3. Tilføj de nødvendige secrets og login-oplysninger fra PDF-filen.
4. Opret en .env fil i "Slottet csharp" mappen, og sæt secrets ind fra PDF-filen
5. Kør følgende fra roden af løsningen:

```bash
docker compose up --build
```

Det bygger og starter både API og frontend.

### Typiske adresser

Når projektet kører, er de typiske adresser:
(Dog har API ingen Postman / Swagger)

- Frontend: `http://localhost:8082`
- API: `http://localhost:8081`

## Systembeskrivelse

Slottet er et administrationssystem til intern brug i en pleje- eller institutionslignende kontekst. Systemet gør det muligt at håndtere:

- Beboere
- Medarbejdere
- Roller
- Afdelinger
- Ansvarsområder
- Telefoner
- Pn-tider
- Medicin
- Post-It-notater
- Risikovurderinger

### Funktionalitet

Systemet understøtter blandt andet:

- Oprettelse, visning, redigering og sletning af data
- Rollebaseret adgangskontrol
- Historik på Post-It-notater
- Kobling mellem beboere, medarbejdere og øvrige registreringer
- Validering i domænelaget
- Dataadgang via repositories og Entity Framework Core

### Login og adgang

Der bruges Microsoft Entra ID til login og autorisation. Roller bruges aktivt i systemet, blandt andet:

- Admin
- Personale

## Teknologier

Projektet bruger følgende teknologier og værktøjer:

- .NET 10
- ASP.NET Core
- Blazor Web App
- Entity Framework Core
- MSSQL
- Docker
- Microsoft Identity Web
- Microsoft Entra ID
- MudBlazor
- Bootstrap-utilities og standard webstyling

### Bemærkning om MudBlazor

MudBlazor er et komponentbibliotek til Blazor, som bruges til at bygge UI med genanvendelige komponenter og et mere sammenhængende layout.

## Hvem har lavet projektet

Projektet er lavet af:

- Freja
- Steffen
- Nicolai
- Thomas

## Arkitektur

Projektet følger Clean Architecture, hvor ansvar og afhængigheder er opdelt i lag.

### Lagene

- **Slottet.API**: Modtager HTTP-kald, håndterer controllers, authentication og CORS
- **Slottet.Frontend**: Brugerfladen bygget som Blazor Web App
- **Slottet.Application**: Forretningslogik og services
- **Slottet.Domain**: Domænemodeller og valideringsregler
- **Slottet.Infrastructure**: EF Core, databaseadgang og repositories
- **Slottet.Shared**: DTO'er og delte dataobjekter

### Fordele ved arkitekturen

- Klar opdeling af ansvar
- Lettere at vedligeholde
- Løs kobling
- Bedre testbarhed
- Overskuelig strukur

## Projektstruktur

Her er en forenklet visning af hovedmappene i løsningen:

```
Slottet csharp/
├── Slottet.API/
├── Slottet.Application/
├── Slottet.Domain/
├── Slottet.Frontend/
├── Slottet.Infrastructure/
├── Slottet.Shared/
├── Slottet.Tests/
├── docker-compose.yml
└── Slottet.slnx
```

### Kort forklaring

- Slottet.API indeholder API'et og controllers
- Slottet.Application indeholder business logic og interfaces
- Slottet.Domain indeholder entiteter og regler
- Slottet.Frontend indeholder brugergrænsefladen
- Slottet.Infrastructure indeholder database og repositories
- Slottet.Shared indeholder DTO'er
- Slottet.Tests indeholder tests

## Krav for at køre projektet

For at køre projektet skal du have:

- Docker Desktop installeret
- Docker Desktop kørende
- Adgang til de nødvendige secrets og login-oplysninger fra PDF-filen
- En gyldig MSSQL connection string
- Internetadgang til Microsoft Entra ID / Azure AD login, hvis authentication skal bruges

## Database

Projektet bruger MS SQL Server, via Entity Framework Core. Data access håndteres gennem repositories som forbinder til Azure databasen

## API-moduler

API'et indeholder blandt andet controllers til:

- Staff
- Resident
- Role
- Department
- ResponsibilityArea
- Phone
- Risk
- Medicine
- PnTime
- PostIt

## Frontend

Frontend er lavet som en Blazor Web App og kommunikerer med API'et via HTTP. UI'et bruger blandt andet:

- Blazor komponenter
- MudBlazor
- Authentication med Microsoft Identity
- HttpClient til API-kald

## Test

Projektet indeholder et testprojekt:

- Slottet.Tests

## Bemærkninger

- Login-oplysninger og secrets ligger i PDF-filen
- Hvis ports eller miljøvariabler ændres, skal docker-compose.yml også opdateres

