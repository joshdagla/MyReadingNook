# 📚 MyReadingNook

A personal book-tracking web app with a cozy "old library" aesthetic. Search for books, build a reading collection, track status, ratings and notes, organize books into custom collections, and connect with friends.

Built with **ASP.NET Core MVC**, **Entity Framework Core + MySQL**, **JWT authentication**, and a **jQuery + Bootstrap** frontend that talks to a JSON API.

---

## 📸 Screenshots

### Login / Register
![Login](docs/screenshots/login.png)

### Home: Daily Shelf and Search
![Home](docs/screenshots/home.png)

### Book Details
![Book details](docs/screenshots/book-details.png)

### My Books
![My Books](docs/screenshots/my-books.png)

### My Collections
![My Collections](docs/screenshots/collections.png)

### Friends
![Friends](docs/screenshots/friends.png)

---

## ✨ Features

- **Authentication**: register and log in with BCrypt-hashed passwords and JWT bearer tokens
- **Book search**: live, debounced search-as-you-type powered by the Google Books API
- **Book details**: cover, author, publisher, description and ISBN; data comes from my own database if the book is already saved, otherwise from Google Books
- **Personal library**: add or remove books, set a status (Wish to Read / Completed), rate 1-5 stars, and write private notes
- **Collections**: create named collections and view them as cards with cover strips
- **Daily Shelf**: a rotating landing-page selection of books, chosen deterministically from the date and cached client-side in `localStorage` so it's fetched once per day
- **Reading stats and Currently Reading widgets** in the sidebar

---

## 🛠 Tech Stack

| Layer | Technology |
|---|---|
| Backend | ASP.NET Core (MVC + Web API), C# |
| Database | MySQL via Entity Framework Core (Pomelo provider) |
| Auth | JWT Bearer, BCrypt.Net-Next |
| External API | Google Books API |
| Frontend | Razor views, jQuery, Bootstrap 5, Bootstrap Icons |

---

## 🧱 Architecture

```
Controllers/   Thin HTTP layer (API controllers + page-serving MVC controllers)
Services/      Business logic behind interfaces (AuthService, TokenService, BookAddService, ...)
Dtos/          Request/response shapes, kept separate from entities
Models/        EF Core entities + NookContext
Extensions/    ClaimsPrincipal helper to read the current user's id from the JWT
Views/         Razor views with a shared sidebar layout
```

Key design choices:
- Controllers stay thin; services do the work and return a `ServiceResult<T>` so success and failure are explicit.
- DTOs are separate from entities, so password hashes never leave the server and clients can't over-post fields.
- The current user's id always comes from the validated JWT, never from the request body.

---

## 🚀 Getting Started

### Prerequisites
- .NET SDK
- MySQL server
- A Google Books API key

### Setup

```bash
git clone https://github.com/joshdagla/MyReadingNook.git
cd MyReadingNook/MyReadingNookApp
```

Secrets are stored with .NET user secrets, so nothing sensitive is committed:

```bash
dotnet user-secrets init
dotnet user-secrets set "Jwt:Key" "a-long-random-string-of-at-least-32-characters"
dotnet user-secrets set "Jwt:Issuer" "MyReadingNookApp"
dotnet user-secrets set "Jwt:Audience" "MyReadingNookApp"
dotnet user-secrets set "GoogleBooks:ApiKey" "YOUR_GOOGLE_BOOKS_API_KEY"
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Database=myreadingnookdb;User=YOUR_USER;Password=YOUR_PASSWORD;"
```

> Match the connection string key to whatever name `Program.cs` reads.

Create the database and run:

```bash
dotnet ef database update
dotnet run
```

Then open the URL shown in the terminal and register an account.

---

## 🎓 What I Learned

- **Layered backend design**: splitting controllers, services, DTOs and entities, and why each boundary exists
- **Authentication end to end**: password hashing with BCrypt, issuing JWTs, validating them with middleware, and protecting endpoints with `[Authorize]`
- **Security habits**: never trusting client-supplied user ids, using vague login errors to prevent account enumeration, and keeping secrets out of source control with user secrets
- **Dependency injection**: service lifetimes, interface registration, and debugging "unable to resolve service" errors
- **EF Core in practice**: composite keys, explicit foreign-key configuration (and the shadow-column bug when it's missing), two foreign keys to one table, unique indexes, migrations, and MySQL text column limits
- **API design**: `[FromQuery]` vs `[FromBody]` vs `[FromRoute]`, partial-update (PATCH) semantics, and picking correct HTTP status codes
- **Working with an external API**: handling Google Books quirks, including fielded queries returning nothing, and not swallowing exceptions that hide the real error
- **jQuery and DOM manipulation**: AJAX with bearer tokens, event delegation for dynamic content, debouncing, and `sessionStorage` / `localStorage` caching
- **Frontend polish**: building a consistent themed UI with Bootstrap and custom CSS, and making a sidebar layout responsive
- **Debugging method**: reading the Network tab, server logs and computed styles instead of guessing, which even uncovered a browser extension (Dark Reader) rewriting my CSS
- **Git workflow**: merging unrelated histories and resolving `.gitignore` conflicts

---

## 🔮 Roadmap

- Friend profile pages that honor profile visibility
- Unfriend action and profile visibility settings page
- Improving UI
  
---

## 📄 License

See the [LICENSE](LICENSE) file.
