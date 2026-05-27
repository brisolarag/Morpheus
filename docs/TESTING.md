# Testing Infrastructure

Morpheus utilizes a comprehensive, multi-layered testing strategy to ensure quality across both backend services and the frontend application. The testing suite runs automatically in our GitHub Actions CI/CD pipeline.

## Backend Unit Tests (xUnit & Moq)
- Located in `Morpheus.Tests/UnitTests/`.
- Tests the business logic of our microservices (`Morpheus.Api`, `Morpheus.Identity`, `Morpheus.Scraper`) in complete isolation.
- Utilizes **Moq** for mocking dependencies and **Entity Framework In-Memory Database** for testing data access logic.

## Behavior-Driven Development (SpecFlow)
- Located in `Morpheus.Tests/Features/`.
- Scenarios are written in plain-English **Gherkin** syntax (`.feature` files) and follow a strict naming convention:
  - **PS (Positive Scenarios)**: Expected successful workflows.
  - **NS (Negative Scenarios)**: Expected error handling and validation workflows.
- These tests validate integration points at the API level (e.g., User Registration, Adding Favorites).

## End-to-End UI Automation (Selenium)
- Located in `Morpheus.Tests/Features/SeleniumTests/`.
- Runs SpecFlow scenarios that interact with a real, headless Google Chrome browser driven by **Selenium WebDriver**.
- Simulates user journeys from end-to-end against the Angular UI.

## Frontend Unit Tests (Angular & Vitest)
- Located within the `Morpheus.Web/src/app/` directory alongside their respective components (`.spec.ts` files).
- We use **Vitest** with JSDOM to render components in a fast, headless environment.
- Mocks Angular Services (`JobService`, etc.) to validate isolated component behavior without needing a running backend.

## How to Run Tests Locally
1. **.NET Tests (Unit, SpecFlow & Selenium)**:
   ```bash
   cd Morpheus.Tests
   dotnet test
   ```
   *(Note: Ensure your Angular frontend is running on `localhost:4200` if you are executing the Selenium UI tests).*

2. **Angular UI Tests (Vitest)**:
   ```bash
   cd Morpheus.Web
   npm run test
   ```
