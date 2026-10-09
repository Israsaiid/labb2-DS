# AuctionLab – Laboration 2 (ASP.NET Core MVC)


## Gruppmedlemmar
Salma Sabul - sabul@kth.se
Bushra Ahmed - Bushraa@kth.se

## Beskrivning
AuctionLab är ett förenklat auktionssystem byggt i ASP.NET Core 8 med MVC-arkitektur.
Användare kan registrera sig, logga in, skapa auktioner, lägga bud och se avslutade auktioner de vunnit.
Systemet använder:
- Entity Framework Core (SQLite)
- ASP.NET Identity (separat databas)
- 3-lager arkitektur: Data / Business / Web

  ## Körning
  1. Öppna terminal i projektets rot
  2. Kör följande kommandon:
   

dotnet build
dotnet run --project AuctionLab.Web --urls "http://localhost:5147"

vi använde av oss en mac
