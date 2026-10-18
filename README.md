# PostgresWpfDemo

Ukázková WPF aplikace v C# zaměřená na moderní .NET vývoj: MVVM, Entity Framework Core, PostgreSQL, Docker, dependency injection, konfiguraci, testy a CI/CD.

Projekt slouží jako praktické studijní prostředí. Cílem není jen napsat CRUD aplikaci, ale pochopit **co se dělá, proč se to dělá a jak spolu jednotlivé části souvisí**.

## Technologie

- .NET 10
- WPF
- MVVM
- Entity Framework Core
- PostgreSQL + Npgsql
- Docker / Docker Compose
- Microsoft.Extensions.Hosting
- Microsoft.Extensions.DependencyInjection
- IConfiguration / appsettings.json
- async / await
- xUnit
- Git + GitHub
- GitHub Actions

## Architektura

```text
MainWindow
    ↓
UserViewModel
    ↓
IUserService
    ↓
UserService
    ↓
IDbContextFactory<AppDbContext>
    ↓
AppDbContext
    ↓
PostgreSQL
```

### Proč je projekt rozdělený takto

`MainWindow` řeší pouze UI.

`UserViewModel` drží stav obrazovky a logiku potřebnou pro binding.

`UserService` řeší práci s uživateli a databázové operace.

`AppDbContext` řeší EF Core model a komunikaci s PostgreSQL.

Objekty nejsou ručně skládány pomocí `new`, ale vytváří je dependency injection container.

## Dependency Injection

Služby jsou registrovány v `App.xaml.cs`, například:

```csharp
services.AddDbContextFactory<AppDbContext>();

services.AddSingleton<IUserService, UserService>();
services.AddSingleton<UserValidator>();

services.AddTransient<UserViewModel>();
services.AddTransient<MainWindow>();
```

Díky constructor injection například `MainWindow` dostane svůj `UserViewModel` automaticky:

```csharp
public MainWindow(UserViewModel viewModel)
{
    InitializeComponent();

    _viewModel = viewModel;
    DataContext = viewModel;

    Loaded += MainWindow_Loaded;
}
```

## EF Core a DbContextFactory

Ve WPF aplikaci používáme:

```csharp
IDbContextFactory<AppDbContext>
```

Každá databázová operace si vytvoří vlastní krátce žijící DbContext:

```csharp
await using var db =
    await _dbContextFactory.CreateDbContextAsync();
```

To je vhodnější než držet jeden DbContext po celou dobu běhu desktop aplikace.

## Konfigurace databáze

Connection string není napevno uložený v `AppDbContext`.

Načítá se přes `IConfiguration`:

```csharp
var connectionString =
    context.Configuration.GetConnectionString("Postgres");
```

`appsettings.json` neobsahuje skutečné heslo:

```json
{
  "ConnectionStrings": {
    "Postgres": ""
  }
}
```

Lokální connection string je uložen mimo Git, například jako Windows environment variable:

```text
ConnectionStrings__Postgres
```

Například:

```text
Host=localhost;Port=5433;Database=PostgresWpfDemoDb;Username=postgres;Password=...
```

> Hesla, API klíče a další secrets se nemají commitovat do repozitáře.

## PostgreSQL v Dockeru

Databáze běží v Docker containeru.

Spuštění:

```powershell
docker compose up -d
```

Kontrola:

```powershell
docker ps
```

Aktuální mapování portu:

```text
localhost:5433 → container:5432
```

## Spuštění projektu

1. Spustit Docker Desktop.
2. Spustit databázi:

```powershell
docker compose up -d
```

3. Ověřit lokální connection string:

```powershell
$env:ConnectionStrings__Postgres
```

4. Spustit WPF projekt ve Visual Studiu přes `F5` nebo `Ctrl + F5`.

## Async / Await

Databázové operace používají asynchronní API:

```csharp
await db.Users.ToListAsync();
await db.SaveChangesAsync();
await _dbContextFactory.CreateDbContextAsync();
```

Cílem je neblokovat UI thread a používat moderní async přístup.

## Git workflow

Základní workflow:

```powershell
git status
git add .
git commit -m "Describe change"
git push
```

Postupně chceme používat i firemnější workflow:

```text
main
 ↓
feature branch
 ↓
commit
 ↓
push
 ↓
Pull Request
 ↓
review
 ↓
CI
 ↓
merge
```

Stejný základní Git workflow lze používat jak na GitHubu, tak v Azure DevOps Repos.

## Testy

Projekt obsahuje xUnit testovací projekt.

Cílem je testovat skutečné chování, například:

- validaci,
- ViewModel,
- služby,
- databázovou integraci.

Ne pouze placeholder testy typu:

```csharp
Assert.True(true);
```

## CI/CD

Projekt používá GitHub Actions.

Pipeline postupně ověřuje:

```text
restore
↓
build
↓
test
↓
CI result
```

Později lze stejný princip převést do Azure DevOps Pipelines.

## Aktuálně hotovo

- WPF aplikace
- základní CRUD
- MVVM
- PostgreSQL
- EF Core + Npgsql
- migrace
- Docker databáze
- Dependency Injection
- `IHost`
- `IDbContextFactory<AppDbContext>`
- konfigurace přes `IConfiguration`
- connection string mimo zdrojový kód
- xUnit testovací projekt
- GitHub Actions
- GitHub repository

## Další plán

1. `ILogger<T>` a logování
2. lepší error handling
3. `IsBusy` ve ViewModelu
4. blokování commandů během async operací
5. lepší validace
6. více unit testů
7. integrační testy s PostgreSQL
8. feature branches + Pull Requests
9. simulace Azure DevOps workflow
10. Azure DevOps Pipeline

## Smysl projektu

Projekt vznikl pro praktické procvičení:

```text
WPF
MVVM
Dependency Injection
EF Core
PostgreSQL
Docker
async / await
Git
testing
CI/CD
configuration
```

Cílem je rozumět nejen tomu, **jak kód napsat**, ale hlavně **proč je architektura navržená právě takto**.
