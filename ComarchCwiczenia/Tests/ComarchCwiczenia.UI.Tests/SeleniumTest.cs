using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace ComarchCwiczenia.UI.Tests;

public class SeleniumTests
{
    private IWebDriver driver;

    [SetUp]
    public void Setup()
    {
        new WebDriverManager.DriverManager().SetUpDriver(
            new WebDriverManager.DriverConfigs.Impl.ChromeConfig()
        );

        ChromeOptions options = new();
        options.AddArgument("headless");
        options.AddArgument("--disable-gpu");

        // --window-size=1920,1080
        // --incognito
        // --disable-extensions
        // --ignore-certificate-errors
        // --lang=en-US
        // --allow-insecure-localhost
        // --no-sandbox

        driver = new ChromeDriver(options);
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
    }

    [TearDown]
    public void TearDown()
    {
        driver.Quit();
        driver.Dispose();
    }



    [Test]
    [TestCase(Category = "Smoke")]
    public void CorrectLoginTest()
    {
        driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/login");

        var usernameField = driver.FindElement(By.Id("username"));
        var passwordField = driver.FindElement(By.Id("password"));

        usernameField.SendKeys("tomsmith");
        passwordField.SendKeys("SuperSecretPassword!");

        var loginButton = driver.FindElement(By.XPath("//*[@id=\"login\"]/button"));
        loginButton.Click();

        var successMessage = driver.FindElement(By.Id("flash"));

        Assert.That(successMessage.Text, Does.Contain("You logged into a secure area"));
    }

    [Test]
    public void DropdownTest()
    {
        driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/dropdown");

        var dropdown = driver.FindElement(By.Id("dropdown"));

        var selectElement = new SelectElement(dropdown);
        selectElement.SelectByValue("1");

        Assert.That(selectElement.SelectedOption.Text, Is.EqualTo("Option 1"));

        selectElement.SelectByValue("2");
        Assert.That(selectElement.SelectedOption.Text, Is.EqualTo("Option 2"));
    }

    [Test]
    public void HandleJavaScriptAlerts()
    {
        // Otwieramy stronę z JavaScript Alerts
        driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/javascript_alerts");

        // Znajdujemy i klikamy przycisk wywołujący alert
        var alertButton = driver.FindElement(By.XPath("//button[text()='Click for JS Alert']"));
        alertButton.Click();

        // Przechwytujemy alert
        var alert = driver.SwitchTo().Alert();

        // Weryfikujemy treść alertu
        Assert.That(alert.Text, Is.EqualTo("I am a JS Alert"), "Tekst alertu jest nieprawidłowy!");

        // Akceptujemy alert
        alert.Accept();

        // Sprawdzamy, czy wyświetlił się komunikat o akceptacji
        var resultText = driver.FindElement(By.Id("result"));
        Assert.That(resultText.Text, Is.EqualTo("You successfully clicked an alert"), "Komunikat po zaakceptowaniu alertu jest nieprawidłowy!");

        // Test dla JS Confirm
        var confirmButton = driver.FindElement(By.XPath("//button[text()='Click for JS Confirm']"));
        confirmButton.Click();

        // Przechwytujemy alert confirm
        alert = driver.SwitchTo().Alert();
        Assert.That(alert.Text, Is.EqualTo("I am a JS Confirm"), "Tekst confirm jest nieprawidłowy!");

        // Odrzucamy confirm
        alert.Dismiss();

        // Sprawdzamy, czy wyświetlił się komunikat o odrzuceniu
        resultText = driver.FindElement(By.Id("result"));
        Assert.That(resultText.Text, Is.EqualTo("You clicked: Cancel"), "Komunikat po odrzuceniu confirm jest nieprawidłowy!");

        // Test dla JS Prompt
        var promptButton = driver.FindElement(By.XPath("//button[text()='Click for JS Prompt']"));
        promptButton.Click();

        // Przechwytujemy prompt
        alert = driver.SwitchTo().Alert();
        Assert.That(alert.Text, Is.EqualTo("I am a JS prompt"), "Tekst prompt jest nieprawidłowy!");

        // Wprowadzamy tekst do prompta
        alert.SendKeys("Test Selenium");

        // Akceptujemy prompt
        alert.Accept();

        // Sprawdzamy, czy wyświetlił się komunikat z wpisanym tekstem
        resultText = driver.FindElement(By.Id("result"));
        Assert.That(resultText.Text, Is.EqualTo("You entered: Test Selenium"), "Komunikat po akceptacji prompta jest nieprawidłowy!");
    }

    [Test]
    public void TestDynamicLoading()
    {
        driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/dynamic_loading/1");

        var startButton = driver.FindElement(By.XPath("//*[@id=\"start\"]/button"));
        startButton.Click();

        WebDriverWait wait = new(driver, TimeSpan.FromSeconds(10));

        var loadedElement = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("finish")));
        Assert.That(loadedElement.Text, Is.EqualTo("Hello World!"));
    }
}