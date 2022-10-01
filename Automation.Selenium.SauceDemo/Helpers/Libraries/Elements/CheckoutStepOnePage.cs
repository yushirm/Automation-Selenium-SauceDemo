using OpenQA.Selenium;

namespace Automation.Selenium.SauceDemo.Helpers.Libraries.Elements
{
  internal class CheckoutStepOnePage
  {
    public static readonly By CheckoutStepOneTitleClassName = By.ClassName("title");
    public static string Title => "CHECKOUT: YOUR INFORMATION.";

    public static readonly By TxtFirstName = By.Id("first-name");
    public static readonly By TxtLastName = By.Id("last-name");
    public static readonly By TxtPostalCode = By.Id("postal-code");

    public static string ValidFirstName => "Michael";
    public static string ValidLastName => "Scott";
    public static string ValidPostalCode => "4090";

    public static readonly By ContinueBtn = By.Name("continue");
    public static readonly By CancelBtn = By.Name("cancel");
  }
}
