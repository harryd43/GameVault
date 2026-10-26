GameVault
A private-first video game tracker — think Letterboxd, but for games.

GameVault is being built incrementally and in public. This README separates what works today from what is planned.
---
About the project
GameVault lets a player browse a catalogue of games, keep one personal record per game in a private library (status, platform, dates, rating and review), and organise games into custom collections. Social features such as public profiles and shared reviews are a deliberate later phase: the first goal is a reliable, secure personal tracker.
Why I'm building it
I graduated with a First-Class BSc (Hons) in Games Development, where most of my work was in C# and Java. GameVault is where I apply those foundations to conventional software engineering: designing a relational data model, building a REST API with clear contracts, enforcing business rules, and working towards automated testing, authentication, CI and deployment.
---
Current status
Relational data model and EF Core migrations	✅ Done
Game catalogue API (CRUD)	✅ Done
Platform and genre catalogue APIs	✅ Done
Linking games to platforms and genres	✅ Done
One-library-entry-per-game rule (database constraint)	✅ Done
Async data access, consistent error responses, delete rules	🔨 In progress
Automated tests and CI (xUnit, GitHub Actions)	🔨 In progress
Personal library API	📋 Planned
Collections API	📋 Planned
Accounts, authentication and per-user authorisation	📋 Planned
React + TypeScript frontend	📋 Planned
Docker and live deployment	📋 Planned
---
Features

Working today
Game catalogue: create, read, update and delete games (title, developer, release date).
Platforms and genres: manage catalogue platforms (e.g. PlayStation 5, PC) and genres (e.g. Fighting).
Many-to-many relationships: attach games to multiple platforms and genres and detach them again. Game responses include their platforms and genres.
Validation and business rules:
required fields and length limits, using data annotations
release dates cannot be in the future
platform and genre names are trimmed and checked for case-insensitive duplicates
duplicate links are rejected
Meaningful HTTP responses:
`201 Created` with a `Location` header
`204 No Content`
`400 Bad Request`
`404 Not Found`
`409 Conflict` for duplicates
Clean API boundary: request and response DTOs, so database entities are never exposed directly.

Planned
Personal library: add each game once, then track its status (Backlogged, Playing, Completed or Abandoned), chosen platform, start and finish dates, a personal rating and a private review.
Collections: custom lists such as "Favourites" or "Games to finish".
Search and personal statistics: backlog size, completion rate and average rating.
Accounts: registration and login with ASP.NET Core Identity. Each user can only access their own data.
Web frontend: React + TypeScript.
Later, optional: opt-in public profiles and reviews, wishlist and playtime tracking.
---

Tech stack

Language	C#
Framework	ASP.NET Core Web API (.NET 10)
Data access	Entity Framework Core 10 (code-first migrations)
Database	SQLite (development); PostgreSQL or SQL Server under consideration for deployment
API description	OpenAPI
Testing (planned)	xUnit, ASP.NET Core integration tests (`WebApplicationFactory`)
CI/CD (planned)	GitHub Actions, Docker, cloud hosting
Frontend (planned)	React + TypeScript
---
Key design decisions
Explicit join entities (`GamePlatform`, `GameGenre`, `CollectionGame`) with composite primary keys model the many-to-many relationships. Each row is identified by the pair it links.
One library entry per user per game. `LibraryEntry` has its own `Id`, because it is a record that gets edited, but a unique database index on `(UserId, GameId)` guarantees a user can't add the same game twice. Replaying a game or switching platform updates the existing entry.
Private first. Ratings and reviews belong to their owner. Any future public sharing will be opt-in.
---
API overview
Method	Route	Description
`GET`	`/api/games`	List games with their platforms and genres
`GET`	`/api/games/{id}`	Get one game
`POST`	`/api/games`	Create a game
`PUT`	`/api/games/{id}`	Update a game
`DELETE`	`/api/games/{id}`	Delete a game
`POST`	`/api/games/{gameId}/platforms/{platformId}`	Link a platform to a game
`DELETE`	`/api/games/{gameId}/platforms/{platformId}`	Unlink a platform
`POST`	`/api/games/{gameId}/genres/{genreId}`	Link a genre to a game
`DELETE`	`/api/games/{gameId}/genres/{genreId}`	Unlink a genre
`GET`	`/api/platforms` · `/api/platforms/{id}`	List platforms / get one
`POST`	`/api/platforms`	Create a platform
`DELETE`	`/api/platforms/{id}`	Delete a platform
`GET`	`/api/genres` · `/api/genres/{id}`	List genres / get one
`POST`	`/api/genres`	Create a genre
`DELETE`	`/api/genres/{id}`	Delete a genre
<details>
<summary>Example response: <code>GET /api/games/7</code></summary>
```json
{
  "id": 7,
  "name": "Mortal Kombat 1",
  "releaseDate": "2023-09-19",
  "developer": "NetherRealm Studios",
  "platforms": [
    { "id": 1, "name": "PlayStation 5" },
    { "id": 3, "name": "Xbox Series X" },
    { "id": 4, "name": "Nintendo Switch" },
    { "id": 5, "name": "PC" }
  ],
  "genres": [
    { "id": 1, "name": "Fighting" }
  ]
}
```
</details>
---
Getting started
Prerequisites
.NET 10 SDK
EF Core CLI tools: `dotnet tool install --global dotnet-ef`
Run locally
```bash
git clone https://github.com/harryd43/GameVault.git
cd GameVault
```
Run the following from the folder that contains `ApiPlayground.csproj`:
```bash
dotnet ef database update   # creates the local SQLite database from the migrations
dotnet run
```
The API starts on the URL shown in the console. In Development, the OpenAPI document is served at `/openapi/v1.json`, and the included `.http` file contains ready-made requests for Visual Studio, Rider or VS Code (REST Client).
> The database file is not committed. It is generated from the EF Core migrations, which are the source of truth for the schema.
---
Roadmap
Foundation hardening (current)
[x] Public repository and `.gitignore`
[ ] Deliberate delete rules (stop catalogue deletions cascading into users' library data)
[ ] Async EF Core data access throughout
[ ] Configuration via `appsettings.json`; consistent `ProblemDetails` error responses
[ ] Database-level uniqueness for platform and genre names
[ ] xUnit integration test suite
[ ] GitHub Actions build-and-test pipeline
[ ] Interactive API documentation
[ ] Rename internal project and namespaces from `ApiPlayground` to `GameVault`
Personal library and collections
[ ] Library entry endpoints (one per user per game, `409 Conflict` on duplicates)
[ ] Collections and collection membership
[ ] Search, filtering, paging and personal statistics
Accounts and security
[ ] Registration and login (ASP.NET Core Identity)
[ ] Per-user authorisation on all personal data
Web app and release
[ ] React + TypeScript frontend
[ ] Docker, production database and live deployment
[ ] Screenshots and a demo walkthrough
---
Known limitations
The project and namespaces are still named `ApiPlayground` from early prototyping. A rename is planned.
There is no authentication yet, so all endpoints are open. Run it locally only.
Testing is currently manual (`.http` requests). Automated tests are the next milestone.
Delete rules for catalogue items are being reworked (see roadmap).
---
Author
Harry Dale: First-Class BSc (Hons) Games Development graduate, York St John University (BCS-accredited).
Looking for graduate and junior software engineering roles in C#/.NET, backend and full-stack development.
GitHub: @harryd43
<!-- TODO: add LinkedIn URL once confirmed -->
