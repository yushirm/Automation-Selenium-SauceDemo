using OpenQA.Selenium;

namespace Automation.Selenium.SauceDemo.Helpers.Libraries.Elements
{
  internal class InventoryPage
  {
    public static readonly By InventoryTitleClassName = By.ClassName("title");
    
    public static string Title => "PRODUCTS";
    
    public static readonly By AddToCartBackPackBtn = By.Name("add-to-cart-sauce-labs-backpack");
    public static readonly By RemoveFromCartBackPackBtn = By.Name("remove-sauce-labs-backpack");
    public static readonly By ShoppingCartBadgeClassName = By.ClassName("shopping_cart_badge");
    public static readonly By ShoppingCartLinkClassName = By.ClassName("shopping_cart_link");

    public static string ShoppingCartBadgeOneItem => "1";
    public static string ShoppingCartLinkEmpty => "";

  }
}