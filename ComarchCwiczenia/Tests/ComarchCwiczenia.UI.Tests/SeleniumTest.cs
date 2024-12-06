using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

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

        driver = new ChromeDriver();
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
    }

    [TearDown]
    public void TearDown()
    {
        driver.Quit();
    }



    [Test]
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
        // Otwieramy stronê z JavaScript Alerts
        driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/javascript_alerts");

        // Znajdujemy i klikamy przycisk wywo³uj¹cy alert
        var alertButton = driver.FindElement(By.XPath("//button[text()='Click for JS Alert']"));
        alertButton.Click();

        // Przechwytujemy alert
        var alert = driver.SwitchTo().Alert();

        // Weryfikujemy treœæ alertu
        Assert.That(alert.Text, Is.EqualTo("I am a JS Alert"), "Tekst alertu jest nieprawid³owy!");

        // Akceptujemy alert
        alert.Accept();

        // Sprawdzamy, czy wyœwietli³ siê komunikat o akceptacji
        var resultText = driver.FindElement(By.Id("result"));
        Assert.That(resultText.Text, Is.EqualTo("You successfully clicked an alert"), "Komunikat po zaakceptowaniu alertu jest nieprawid³owy!");

        // Test dla JS Confirm
        var confirmButton = driver.FindElement(By.XPath("//button[text()='Click for JS Confirm']"));
        confirmButton.Click();

        // Przechwytujemy alert confirm
        alert = driver.SwitchTo().Alert();
        Assert.That(alert.Text, Is.EqualTo("I am a JS Confirm"), "Tekst confirm jest nieprawid³owy!");

        // Odrzucamy confirm
        alert.Dismiss();

        // Sprawdzamy, czy wyœwietli³ siê komunikat o odrzuceniu
        resultText = driver.FindElement(By.Id("result"));
        Assert.That(resultText.Text, Is.EqualTo("You clicked: Cancel"), "Komunikat po odrzuceniu confirm jest nieprawid³owy!");

        // Test dla JS Prompt
        var promptButton = driver.FindElement(By.XPath("//button[text()='Click for JS Prompt']"));
        promptButton.Click();

        // Przechwytujemy prompt
        alert = driver.SwitchTo().Alert();
        Assert.That(alert.Text, Is.EqualTo("I am a JS prompt"), "Tekst prompt jest nieprawid³owy!");

        // Wprowadzamy tekst do prompta
        alert.SendKeys("Test Selenium");

        // Akceptujemy prompt
        alert.Accept();

        // Sprawdzamy, czy wyœwietli³ siê komunikat z wpisanym tekstem
        resultText = driver.FindElement(By.Id("result"));
        Assert.That(resultText.Text, Is.EqualTo("You entered: Test Selenium"), "Komunikat po akceptacji prompta jest nieprawid³owy!");
    }
}