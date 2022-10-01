using OpenQA.Selenium;

namespace Automation.Selenium.SauceDemo.Helpers.Libraries.Elements
{
  internal class CheckoutCompletePage
  {
    public static readonly By CheckoutCompleteTitleClassName = By.ClassName("title");
    public static string Title => "CHECKOUT: COMPLETE!";

    public static readonly By BackToHomeBtn = By.Name("back-to-products");
  }
}
