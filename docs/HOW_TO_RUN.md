# How to Run Morpheus

## System Requirements (Specifications)

Before running the project, ensure your machine meets the following requirements:

- **OS**: Windows 10/11, macOS (Intel or Apple Silicon), or Linux (Ubuntu 20.04+, Linux Mint, etc.)
- **RAM**: Minimum 8GB (16GB recommended for running frontend, backend, and DB simultaneously)
- **CPU**: Dual-core processor or better
- **Dependencies**:
  - [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
  - [Node.js](https://nodejs.org/) (v18+ recommended) & npm (for Angular frontend)
  - [PostgreSQL](https://www.postgresql.org/download/) (v15+) with [`pgvector`](https://github.com/pgvector/pgvector) extension
  - (Optional but highly recommended) [Docker Desktop](https://www.docker.com/products/docker-desktop/) or Docker Engine (for easy PostgreSQL/pgvector setup)
  - OpenAI API Key
  - Apify API Token

## Environment Setup

1. Clone the repository:
   ```bash
   git clone <repo_url>
   cd Morpheus
   ```
2. Set up your environment variables (or update `appsettings.Development.json` in API and Scraper projects):
   - `ConnectionStrings:DefaultConnection`: Your PostgreSQL connection string.
   - `OpenAI:ApiKey`: Your OpenAI API key.
   - `Apify:Token`: Your Apify token.

## Running on Linux / macOS

1. **Database Setup**: We recommend using Docker for the database.
   ```bash
   docker run --name morpheus-db -e POSTGRES_USER=postgres -e POSTGRES_PASSWORD=postgres -e POSTGRES_DB=morpheus -p 5432:5432 -d pgvector/pgvector:pg16
   ```
2. **Apply Migrations** (if applicable):
   ```bash
   cd Morpheus.Api
   dotnet ef database update
   ```
3. **Run the API**:
   ```bash
   dotnet run --project Morpheus.Api
   ```
4. **Run the Scraper** (in a separate terminal):
   ```bash
   dotnet run --project Morpheus.Scraper
   ```
5. **Run the Frontend**:
   ```bash
   cd Morpheus.Web
   npm install
   npm start
   ```

## Running on Windows

1. **Database Setup**: You can use Docker Desktop to run the pgvector image, or install PostgreSQL natively and compile the pgvector extension (Docker is highly recommended).
   ```powershell
   docker run --name morpheus-db -e POSTGRES_USER=postgres -e POSTGRES_PASSWORD=postgres -e POSTGRES_DB=morpheus -p 5432:5432 -d pgvector/pgvector:pg16
   ```
2. **Apply Migrations**: Open PowerShell or Command Prompt.
   ```powershell
   cd Morpheus.Api
   dotnet ef database update
   ```
3. **Run the API**:
   ```powershell
   dotnet run --project Morpheus.Api
   ```
4. **Run the Scraper** (in a separate window):
   ```powershell
   dotnet run --project Morpheus.Scraper
   ```
5. **Run the Frontend**:
   ```powershell
   cd Morpheus.Web
   npm install
   npm start
   ```
