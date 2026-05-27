Feature: Job Favorites
  In order to save jobs I like
  As an authenticated user
  I want to be able to add and remove jobs from my favorites

  Scenario: PS1 - User adds a job to favorites successfully
    Given I am authenticated as a valid user
    And a valid job exists in the database
    When I request to add the job to my favorites
    Then the response status should be successful
    And the job should appear in my favorites list

  Scenario: NS1 - User tries to favorite a non-existent job
    Given I am authenticated as a valid user
    When I request to add a non-existent job to my favorites
    Then the response status should be not found
