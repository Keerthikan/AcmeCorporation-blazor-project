# AcmeCorporation

A modular Blazor Web App built with **.NET 10** and backed by **PostgreSQL**.  
This solution follows a clean architecture approach, separating concerns into Core, Data, Web, and Test projects.

---

## 📂 Project Structure

- **AcmeCorporation.Web**  
  Blazor front-end application. Hosts UI components and client-side logic.

- **AcmeCorporation.Data**  
  Data access layer using Entity Framework Core with PostgreSQL.

- **AcmeCorporation.Core**  
  Core business logic, domain models, and services.

- **AcmeCorporation.Core.Tests**  
  Unit tests for the Core project.

---

## Features

- Blazor WebAssembly front-end with .NET 10
- PostgreSQL database integration
- Entity Framework Core for ORM and migrations
- Modular architecture for scalability
- Unit testing with xUnit/NUnit (depending on your test framework)

---

## 🛠 Requirements

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [PostgreSQL 15+](https://www.postgresql.org/download/)
- [Entity Framework Core Tools](https://learn.microsoft.com/ef/core/cli/dotnet) (`dotnet-ef`)

---

## How to run it 
1. **Clone the repository**
   ```bash
   git clone <repo-url>
   cd AcmeCorporation

2. **Restore dependencies**
   ```bash
   dotnet restore
3. **Build the solution**
   ```bash
   dotnet build

4. **Run the Blazor Web App**
   ```bash
   dotnet run --project AcmeCorporation.Web


 ---
 ## How to apply migrations? ##
 1. **Verify PostgreSQL Installation**
    ```bash
    psql --version

 If not installed -  please follow - [PostgreSQL 15+](https://www.postgresql.org/download/) and try again. 

 If installed, please enter it by running this command
 2. Connect to Postgresql
    ```bash
    psql -U postgres

 3. Create two databases - one for business logic the other one for Identity
    ```bash 
    CREATE DATABASE my_app_db;
    CREATE DATABASE my_idp_db;

 4. Adjust appsetting.json with the new connectionstring
    ```bash 
    "ConnectionStrings": {
      "BusinessDb": "Host=localhost;Port=5432;Database=my_app_db;Username=postgres;Password=postgres",
      "IdentityDb": "Host=localhost;Port=5432;Database=my_idp_db;Username=postgres;Password=postgres"
    },
 5. Once the databases has been setup the migration can be applied on to them, so the schema and tables will be setup. 
    ```bash
    dotnet ef database update --project AcmeCorporation.Data --startup-project AcmeCorporation.Web

## Note 
Since I had some extra time i decided to play a bit with some of the stuff i have thought of any way... 

