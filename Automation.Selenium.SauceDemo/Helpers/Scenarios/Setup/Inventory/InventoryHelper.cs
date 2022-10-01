using Automation.Selenium.SauceDemo.Helpers.Extensions;
using Automation.Selenium.SauceDemo.Helpers.Libraries.Elements;
using OpenQA.Selenium;


namespace Automation.Selenium.SauceDemo.Helpers.Scenarios.Setup.Inventory
{
  internal class InventoryHelper
  {
    public static void AddItemToCart(By itemToAdd)
    {
      WebDriverExtensions.Driver.FindElement(itemToAdd).Click();
    }

    public static void OpenCart()
    {
      WebDriverExtensions.Driver.FindElement(InventoryPage.ShoppingCartLinkClassName).Click();
    }
  }
}
