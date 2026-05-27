using OpenQA.Selenium;
using TechTalk.SpecFlow;
using Xunit;

namespace Morpheus.Tests.Steps;

[Binding]
public class LoginSteps
{
    private readonly IWebDriver _driver;

    public LoginSteps(IWebDriver driver)
    {
        _driver = driver;
    }

    [Given(@"I have navigated to the login page")]
    public void GivenIHaveNavigatedToTheLoginPage()
    {
        // Navigate to the Angular UI running locally
        _driver.Navigate().GoToUrl("http://localhost:4200/login");
    }

    [When(@"I enter my email ""(.*)"" and password ""(.*)""")]
    public void WhenIEnterMyEmailAndPassword(string email, string password)
    {
        // Wait for elements to be present (example code, in real app you might need explicit waits)
        // var emailInput = _driver.FindElement(By.Id("email"));
        // emailInput.SendKeys(email);
        // var passwordInput = _driver.FindElement(By.Id("password"));
        // passwordInput.SendKeys(password);
    }

    [When(@"I click the login button")]
    public void WhenIClickTheLoginButton()
    {
        // var loginButton = _driver.FindElement(By.Id("login-submit"));
        // loginButton.Click();
    }

    [Then(@"I should be redirected to the dashboard")]
    public void ThenIShouldBeRedirectedToTheDashboard()
    {
        // Assert.Contains("dashboard", _driver.Url);
    }

    [Then(@"I should see a welcome message")]
    public void ThenIShouldSeeAWelcomeMessage()
    {
        // var welcomeMsg = _driver.FindElement(By.ClassName("welcome-text"));
        // Assert.NotNull(welcomeMsg);
    }
}
