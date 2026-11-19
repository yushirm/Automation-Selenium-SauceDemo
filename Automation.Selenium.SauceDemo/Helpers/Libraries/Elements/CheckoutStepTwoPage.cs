using OpenQA.Selenium;

namespace Automation.Selenium.SauceDemo.Helpers.Libraries.Elements
{
  internal class CheckoutStepTwoPage
  {
    public static readonly By CheckoutStepTwoTitleClassName = By.ClassName("title");
    public static string Title => "Checkout: Overview";

    public static readonly By FinishBtn = By.Name("finish");
    public static readonly By CancelBtn = By.Name("cancel");
  }
}
