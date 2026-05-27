using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TechTalk.SpecFlow;
using BoDi;

namespace Morpheus.Tests.UI;

[Binding]
public class WebDriverSetup
{
    private readonly IObjectContainer _objectContainer;
    private IWebDriver? _driver;

    public WebDriverSetup(IObjectContainer objectContainer)
    {
        _objectContainer = objectContainer;
    }

    [BeforeScenario]
    public void InitializeWebDriver()
    {
        var options = new ChromeOptions();
        options.AddArgument("--headless=new"); // Run in headless mode by default for CI
        options.AddArgument("--disable-gpu");
        options.AddArgument("--window-size=1920,1080");

        // Assumes ChromeDriver is installed/available in PATH
        _driver = new ChromeDriver(options);
        
        // Register the driver in the DI container so steps can inject it
        _objectContainer.RegisterInstanceAs<IWebDriver>(_driver);
    }

    [AfterScenario]
    public void DisposeWebDriver()
    {
        if (_driver != null)
        {
            _driver.Quit();
            _driver.Dispose();
        }
    }
}
