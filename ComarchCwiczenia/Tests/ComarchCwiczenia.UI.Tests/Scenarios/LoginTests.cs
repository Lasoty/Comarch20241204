using ComarchCwiczenia.UI.Tests.PageObjects;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace ComarchCwiczenia.UI.Tests.Scenarios;

public class LoginTests : ScenarioBase
{
    [Test]
    public void LoginWithInvalidCredentialsShowsErrorMessage()
    {
        driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/login");
        LoginPage loginPage = new(driver);

        loginPage.EnterUserName("tomsmith");
        loginPage.EnterPassword("WrongPassword");
        loginPage.ClickLoginButton();

        Assert.That(loginPage.IsErrorMessageDisplayed(), Is.True);
    }
}