Feature: Job Ingestion
  In order to populate the system with job opportunities
  As the background scraper service
  I want to be able to ingest and store jobs

  Scenario: PS1 - Scraper successfully processes valid job data
    Given a mock payload of valid job data from LinkedIn
    When the scraper processes the payload
    Then the system should store the job details correctly
    And the job should be available for semantic search

  Scenario: NS1 - Scraper handles invalid or incomplete job data
    Given a mock payload with missing required fields
    When the scraper processes the payload
    Then the system should reject the invalid data
    And log an appropriate error
