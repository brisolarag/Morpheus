# Morpheus

![Morpheus Architecture](/home/gbrisolara/.gemini/antigravity/brain/01c6797e-c561-4bf2-8d91-cae4e88ba6e6/morpheus_architecture_1779831327767.png)

Morpheus is an intelligent job aggregation and semantic search platform. The application abandons traditional keyword-based search in favor of **Semantic Search (Embedding-based retrieval)** utilizing the .NET 8 ecosystem and PostgreSQL.

## 1. System Architecture

The system is decoupled into three main domains:

- **Morpheus.Api (The "Brain")**: ASP.NET Core 8 API. Responsible for exposing endpoints, orchestrating vectorization via OpenAI (model: `text-embedding-3-small`), managing database persistence, and serving the semantic search.
- **Morpheus.Scraper (The "Muscles")**: Worker Service hosted as a background service. Extracts data via Apify (LinkedIn Scraper Actor), processes the dataset JSON, and submits the data to the API.
- **Morpheus.Shareds (The Domain)**: Shared library project containing Entities (`Job`, `Technology`, `JobTechnology`), Enums, and DTOs, ensuring consistent typing between the Scraper and the API.
- **Morpheus.Web**: Frontend built with Angular (Standalone Components) + TailwindCSS for consuming the API and providing the user interface.

## 2. Technology Stack & Infrastructure

- **Backend**: .NET 8.0, ASP.NET Core
- **Database**: PostgreSQL with the `pgvector` extension
- **Artificial Intelligence**: OpenAI API for embedding generation (1536 dimensions)
- **Data Orchestration**: Apify (LinkedIn Scraper)
- **Frontend**: Angular (Standalone Components) + TailwindCSS
- **Logging**: Serilog (Structured Logging)

## 3. Data Pipeline (Workflow)

1. **Ingestion**: The Worker triggers the Apify Actor via `run-sync-get-dataset-items`.
2. **Deduplication**: The API validates the existence of the job using `ExternalJobId` before any processing.
3. **Vectorization**: The job description is sent to OpenAI; the returned vector is persisted in the `Embedding` column (`vector(1536)` type).
4. **Indexing**: An HNSW index (`hnsw`) with `vector_cosine_ops` is used to ensure semantic search is efficient at scale.
5. **Retrieval**: The endpoint `GET /api/jobs/search` receives a natural language query, vectorizes it, and returns the top-N results ordered by cosine similarity.

## 4. Current Project State

- ✅ **Completed**: Ingestion pipeline, vector database setup, deduplication logic, embedding service, and functional semantic search endpoint.
- 🚧 **In Progress**: Frontend development (Angular + Tailwind) to consume the API.
- 🎯 **Immediate Technical Challenge**: Implementation of UI components (Job Search, Job Cards) and refinement of data mapping from the Apify dataset to the `Job` entity.

## 5. System Requirements (Specifications)

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

## 6. How to Run

### Environment Setup

1. Clone the repository:
   ```bash
   git clone <repo_url>
   cd Morpheus
   ```
2. Set up your environment variables (or update `appsettings.Development.json` in API and Scraper projects):
   - `ConnectionStrings:DefaultConnection`: Your PostgreSQL connection string.
   - `OpenAI:ApiKey`: Your OpenAI API key.
   - `Apify:Token`: Your Apify token.

### Running on Linux / macOS

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

### Running on Windows

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

## 7. Kubernetes Deployment (Optional)
If you wish to deploy this architecture into a Kubernetes cluster (as illustrated in the architecture diagram), ensure you have `kubectl` configured, a running cluster (e.g., Minikube, kind, or a cloud provider), and apply your manifest files located in the `infra` folder (when fully configured).
