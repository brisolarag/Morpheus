Feature: User Login
  In order to access my account
  As a registered user
  I want to be able to log in to the Morpheus platform

  @UI
  Scenario: Successful login with valid credentials
    Given I have navigated to the login page
    When I enter my email "john@example.com" and password "Password123!"
    And I click the login button
    Then I should be redirected to the dashboard
    And I should see a welcome message
