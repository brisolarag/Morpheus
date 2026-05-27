Feature: User Registration
  In order to create an account
  As a new user
  I want to be able to register using my details

  Scenario: PS1 - User registers successfully with valid data
    Given I have valid registration details for "jane@example.com"
    When I submit the registration request
    Then the registration response status should be successful
    And the user "jane@example.com" should exist in the database

  Scenario: NS1 - User registration fails with duplicate email
    Given a user with email "jane@example.com" already exists
    When I submit the registration request for "jane@example.com"
    Then the response status should be a bad request
    And the error message should indicate duplicate email
