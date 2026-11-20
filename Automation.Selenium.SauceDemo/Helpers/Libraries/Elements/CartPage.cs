using OpenQA.Selenium;

namespace Automation.Selenium.SauceDemo.Helpers.Libraries.Elements
{
  internal class CartPage
  {
    public static string Url => "/cart.html";

    public static readonly By CartTitleClassName = By.ClassName("title");
    public static string Title => "Your Cart";

    public static readonly By CheckOutBtn = By.Name("checkout");
    public static readonly By ContinueShoppingBtn = By.Name("continue-shopping");
  }
}
