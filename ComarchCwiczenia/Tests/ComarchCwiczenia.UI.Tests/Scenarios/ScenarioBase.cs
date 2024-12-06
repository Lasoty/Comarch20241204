using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;

namespace ComarchCwiczenia.UI.Tests.Scenarios;

[TestFixture]
public abstract class ScenarioBase
{
    protected IWebDriver driver;

    [SetUp]
    public virtual void Setup()
    {
        new WebDriverManager.DriverManager().SetUpDriver(
            new WebDriverManager.DriverConfigs.Impl.ChromeConfig()
        );

        ChromeOptions options = new();
        options.AddArgument("headless");
        options.AddArgument("--disable-gpu");

        driver = new ChromeDriver(options);
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
    }

    [TearDown]
    public virtual void TearDown()
    {
        driver.Quit();
        driver.Dispose();
    }
}