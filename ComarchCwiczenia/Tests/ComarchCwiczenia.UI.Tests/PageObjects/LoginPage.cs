using OpenQA.Selenium;

namespace ComarchCwiczenia.UI.Tests.PageObjects;

public class LoginPage
{
    private IWebDriver driver;

    public LoginPage(IWebDriver driver)
    {
        this.driver = driver;
    }

    private IWebElement UserNameField => driver.FindElement(By.Id("username"));
    private IWebElement PasswordField => driver.FindElement(By.Id("password"));
    private IWebElement LoginButton => driver.FindElement(By.XPath("//*[@id=\"login\"]/button"));
    private IWebElement ErrorMessage => driver.FindElement(By.Id("flash"));

    public void EnterUserName(string userName)
    {
        UserNameField.SendKeys(userName);
    }

    public void EnterPassword(string password)
    {
        PasswordField.SendKeys(password);
    }

    public void ClickLoginButton() => LoginButton.Click();

    public bool IsErrorMessageDisplayed() => ErrorMessage.Displayed;
}