using Automation.Selenium.SauceDemo.Helpers.Extensions;
using Automation.Selenium.SauceDemo.Helpers.Libraries.Elements;
using OpenQA.Selenium;


namespace Automation.Selenium.SauceDemo.Helpers.Scenarios.Setup.Inventory
{
  internal class InventoryHelper
  {
    public static void AddItemToCart(By itemToAdd)
    {
      WebDriverExtensions.Click(itemToAdd);
      WebDriverExtensions.WaitForItemDisplayed(InventoryPage.ShoppingCartBadgeClassName, 10);
    }

    public static void OpenCart()
    {
      WebDriverExtensions.ClickAndWaitForUrl(InventoryPage.ShoppingCartLinkClassName, CartPage.Url);
    }
  }
}
