using Automation.Selenium.SauceDemo.Helpers.Extensions;
using Automation.Selenium.SauceDemo.Helpers.Libraries.Configuration;
using Automation.Selenium.SauceDemo.Helpers.Libraries.Elements;
using Automation.Selenium.SauceDemo.Helpers.Scenarios.Setup.CheckOutStepOne;
using Automation.Selenium.SauceDemo.Helpers.Scenarios.Setup.Inventory;
using Automation.Selenium.SauceDemo.Helpers.Scenarios.Setup.Login;
using NUnit.Framework;
using FluentAssertions;

namespace Automation.Selenium.SauceDemo
{
  public class LoginPageTests
  {
    [SetUp]
    public virtual void Initialize()
    {
      WebDriverExtensions.GetDriver(TestConfiguration.SupportedBrowser);
      WebDriverExtensions.Driver.Manage().Cookies.DeleteAllCookies();
      WebDriverExtensions.Driver.Navigate().GoToUrl(TestConfiguration.SauceDemoUrl);

      WebDriverExtensions.WaitForItemDisplayed(LoginPage.TxtUsername);
      WebDriverExtensions.WaitForItemDisplayed(LoginPage.TxtPassword);
      WebDriverExtensions.WaitForItemDisplayed(LoginPage.BtnLogin);
    }

    [TearDown]
    public void EndTest()
    {
      WebDriverExtensions.Driver.Quit();
    }

    [Test]
    public void GivenCorrectCredentials_WhenLoginButtonClicked_ShouldRedirectToInventoryPage()
    {
      // Arrange
      var expected = InventoryPage.InventoryTitleClassName;

      // Act
      WebDriverExtensions.ResetElementText(LoginPage.TxtUsername, LoginPage.CorrectUsername);
      WebDriverExtensions.ResetElementText(LoginPage.TxtPassword, LoginPage.CorrectPassword);
      WebDriverExtensions.ClickAndWaitForUrl(LoginPage.BtnLogin, InventoryPage.Url);

      var pageTitle = WebDriverExtensions.ReadTextOnceSettled(expected, InventoryPage.Title);

      // Assert
      pageTitle.Should().Be(InventoryPage.Title);
    }

    [Test]
    public void GivenIncorrectCredentials_WhenLoginButtonClicked_ShouldDisplayErrorMessage()
    {
      // Arrange

      // Act
      WebDriverExtensions.ResetElementText(LoginPage.TxtUsername, LoginPage.IncorrectUsername);
      WebDriverExtensions.ResetElementText(LoginPage.TxtPassword, LoginPage.CorrectPassword);
      WebDriverExtensions.Click(LoginPage.BtnLogin);
      WebDriverExtensions.WaitForItemDisplayed(LoginPage.LblErrorMessageElement, 10);

      var lblErrorMessage = WebDriverExtensions.Driver.FindElement(LoginPage.LblErrorMessageElement).Text;

      // Assert
      lblErrorMessage.Should().Be(LoginPage.LblErrorMessageText);
    }
  }
  public class InventoryPageTests
  {
    [SetUp]
    public virtual void Initialize()
    {
      WebDriverExtensions.GetDriver(TestConfiguration.SupportedBrowser);
      WebDriverExtensions.Driver.Manage().Cookies.DeleteAllCookies();
      WebDriverExtensions.Driver.Navigate().GoToUrl(TestConfiguration.SauceDemoUrl);

      WebDriverExtensions.WaitForItemDisplayed(LoginPage.BtnLogin);

      LoginHelper.PerformLogin(TestConfiguration.validUserName, TestConfiguration.validPassword);
    }
    
    [TearDown]
    public void EndTest()
    {
      WebDriverExtensions.Driver.Quit();
    }

    [Test]
    public void GivenItemNotInShoppingCart_WhenItemAddToCartButtonClicked_ThenShouldIncreaseShoppingCartBadgeValueToOne()
    {
      // Arrange
      var expected = InventoryPage.ShoppingCartBadgeClassName;

      // Act
      WebDriverExtensions.Click(InventoryPage.AddToCartBackPackBtn);
      var cartBadgeState = WebDriverExtensions.ReadTextOnceSettled(expected, InventoryPage.ShoppingCartBadgeOneItem);

      // Assert
      cartBadgeState.Should().Be(InventoryPage.ShoppingCartBadgeOneItem);
    }

    [Test]
    public void GivenOneItemInShoppingCart_WhenItemRemoveFromCartButtonClicked_ThenShouldDecreaseShoppingCartBadgeValueToZero()
    {
      // Arrange
      var expected = InventoryPage.ShoppingCartLinkClassName;
      WebDriverExtensions.Click(InventoryPage.AddToCartBackPackBtn);

      // Act
      WebDriverExtensions.Click(InventoryPage.RemoveFromCartBackPackBtn);
      var cartLinkState = WebDriverExtensions.ReadTextOnceSettled(expected, InventoryPage.ShoppingCartLinkEmpty);

      // Assert
      cartLinkState.Should().Be(InventoryPage.ShoppingCartLinkEmpty);
    }

    [Test]
    public void GivenItemInShoppingCart_WhenShoppingCartButtonClicked_ThenShouldRedirectToShoppingCart()
    {
      // Arrange
      var expected = CartPage.CartTitleClassName;
      WebDriverExtensions.Click(InventoryPage.AddToCartBackPackBtn);

      // Act
      WebDriverExtensions.ClickAndWaitForUrl(InventoryPage.ShoppingCartLinkClassName, CartPage.Url);
      var pageTitle = WebDriverExtensions.ReadTextOnceSettled(expected, CartPage.Title);

      // Assert
      pageTitle.Should().Be(CartPage.Title);
    }
  }
  public class CartPageTests
  {
    [SetUp]
    public virtual void Initialize()
    {
      WebDriverExtensions.GetDriver(TestConfiguration.SupportedBrowser);
      WebDriverExtensions.Driver.Manage().Cookies.DeleteAllCookies();
      WebDriverExtensions.Driver.Navigate().GoToUrl(TestConfiguration.SauceDemoUrl);

      WebDriverExtensions.WaitForItemDisplayed(LoginPage.BtnLogin);

      LoginHelper.PerformLogin(TestConfiguration.validUserName, TestConfiguration.validPassword);

      InventoryHelper.AddItemToCart(TestConfiguration.AddBackPackToCartButton);
      InventoryHelper.OpenCart();
    }

    [TearDown]
    public void EndTest()
    {
      WebDriverExtensions.Driver.Quit();
    }

    [Test]
    public void GivenShoppingCartOpen_WhenCheckoutButtonClicked_ThenShouldRedirectToCheckoutStepOne()
    {
      // Arrange
      var expected = CheckoutStepOnePage.CheckoutStepOneTitleClassName;
      
      // Act
      WebDriverExtensions.ClickAndWaitForUrl(CartPage.CheckOutBtn, CheckoutStepOnePage.Url);
      var pageTitle = WebDriverExtensions.ReadTextOnceSettled(expected, CheckoutStepOnePage.Title);

      // Assert
      pageTitle.Should().Be(CheckoutStepOnePage.Title);
    }

    [Test]
    public void GivenShoppingCartOpen_WheContinueShoppingButtonClicked_ThenShouldRedirectToInventoryPage()
    {
      // Arrange
      var expected = InventoryPage.InventoryTitleClassName;

      // Act
      WebDriverExtensions.ClickAndWaitForUrl(CartPage.ContinueShoppingBtn, InventoryPage.Url);
      var pageTitle = WebDriverExtensions.ReadTextOnceSettled(expected, InventoryPage.Title);

      // Assert
      pageTitle.Should().Be(InventoryPage.Title);
    }
  }
  public class CheckoutPageOneTests
  {
    [SetUp]
    public virtual void Initialize()
    {
      WebDriverExtensions.GetDriver(TestConfiguration.SupportedBrowser);
      WebDriverExtensions.Driver.Manage().Cookies.DeleteAllCookies();
      WebDriverExtensions.Driver.Navigate().GoToUrl(TestConfiguration.SauceDemoUrl);

      WebDriverExtensions.WaitForItemDisplayed(LoginPage.BtnLogin);

      LoginHelper.PerformLogin(TestConfiguration.validUserName, TestConfiguration.validPassword);

      InventoryHelper.AddItemToCart(TestConfiguration.AddBackPackToCartButton);
      InventoryHelper.OpenCart();

      WebDriverExtensions.ClickAndWaitForUrl(CartPage.CheckOutBtn, CheckoutStepOnePage.Url);
    }

    [TearDown]
    public void EndTest()
    {
      WebDriverExtensions.Driver.Quit();
    }

    [Test]
    public void GivenCheckOutPageOneOpen_WhenContinueButtonClicked_ThenShouldRedirectToCheckoutStepTwo()
    {
      var expected = CheckoutStepTwoPage.CheckoutStepTwoTitleClassName;

      // Act
      WebDriverExtensions.ResetElementText(CheckoutStepOnePage.TxtFirstName, CheckoutStepOnePage.ValidFirstName);
      WebDriverExtensions.ResetElementText(CheckoutStepOnePage.TxtLastName, CheckoutStepOnePage.ValidLastName);
      WebDriverExtensions.ResetElementText(CheckoutStepOnePage.TxtPostalCode, CheckoutStepOnePage.ValidPostalCode);
      WebDriverExtensions.ClickAndWaitForUrl(CheckoutStepOnePage.ContinueBtn, CheckoutStepTwoPage.Url);

      var pageTitle = WebDriverExtensions.ReadTextOnceSettled(expected, CheckoutStepTwoPage.Title);

      // Assert
      pageTitle.Should().Be(CheckoutStepTwoPage.Title);
    }

    [Test]
    public void GivenCheckOutPageOneOpen_WhenContinueShoppingButtonClicked_ThenShouldReturnToCart()
    {
      // Arrange
      var expected = CartPage.CartTitleClassName;

      // Act
      WebDriverExtensions.ClickAndWaitForUrl(CheckoutStepOnePage.CancelBtn, CartPage.Url);
      var pageTitle = WebDriverExtensions.ReadTextOnceSettled(expected, CartPage.Title);

      // Assert
      pageTitle.Should().Be(CartPage.Title);
    }
  }
  public class CheckoutPageTwoTests
  {
    [SetUp]
    public virtual void Initialize()
    {
      WebDriverExtensions.GetDriver(TestConfiguration.SupportedBrowser);
      WebDriverExtensions.Driver.Manage().Cookies.DeleteAllCookies();
      WebDriverExtensions.Driver.Navigate().GoToUrl(TestConfiguration.SauceDemoUrl);

      WebDriverExtensions.WaitForItemDisplayed(LoginPage.BtnLogin);

      LoginHelper.PerformLogin(TestConfiguration.validUserName, TestConfiguration.validPassword);

      InventoryHelper.AddItemToCart(TestConfiguration.AddBackPackToCartButton);
      InventoryHelper.OpenCart();

      WebDriverExtensions.ClickAndWaitForUrl(CartPage.CheckOutBtn, CheckoutStepOnePage.Url);

      CheckOutStepOneHelper.PerformValidCheckOutStepOne(TestConfiguration.ValidFirstName, TestConfiguration.ValidLastName, TestConfiguration.ValidPostalCode);
    }

    [TearDown]
    public void EndTest()
    {
      WebDriverExtensions.Driver.Quit();
    }

    [Test]
    public void GivenCheckOutPageTwoOpen_WhenFinishButtonClicked_ThenShouldRedirectToCheckoutComplete()
    {
      var expected = CheckoutCompletePage.CheckoutCompleteTitleClassName;

      // Act
      WebDriverExtensions.ClickAndWaitForUrl(CheckoutStepTwoPage.FinishBtn, CheckoutCompletePage.Url);
      var pageTitle = WebDriverExtensions.ReadTextOnceSettled(expected, CheckoutCompletePage.Title);

      // Assert
      pageTitle.Should().Be(CheckoutCompletePage.Title);
    }

    [Test]
    public void GivenCheckOutPageTwoOpen_WhenCancelButtonClicked_ThenShouldReturnToInventory()
    {
      // Arrange
      var expected = InventoryPage.InventoryTitleClassName;

      // Act
      WebDriverExtensions.ClickAndWaitForUrl(CheckoutStepTwoPage.CancelBtn, InventoryPage.Url);
      var pageTitle = WebDriverExtensions.ReadTextOnceSettled(expected, InventoryPage.Title);

      // Assert
      pageTitle.Should().Be(InventoryPage.Title);
    }
  }
  public class CheckoutCompleteTests
  {
    [SetUp]
    public virtual void Initialize()
    {
      WebDriverExtensions.GetDriver(TestConfiguration.SupportedBrowser);
      WebDriverExtensions.Driver.Manage().Cookies.DeleteAllCookies();
      WebDriverExtensions.Driver.Navigate().GoToUrl(TestConfiguration.SauceDemoUrl);

      WebDriverExtensions.WaitForItemDisplayed(LoginPage.BtnLogin);

      LoginHelper.PerformLogin(TestConfiguration.validUserName, TestConfiguration.validPassword);

      InventoryHelper.AddItemToCart(TestConfiguration.AddBackPackToCartButton);
      InventoryHelper.OpenCart();

      WebDriverExtensions.ClickAndWaitForUrl(CartPage.CheckOutBtn, CheckoutStepOnePage.Url);

      CheckOutStepOneHelper.PerformValidCheckOutStepOne(TestConfiguration.ValidFirstName, TestConfiguration.ValidLastName, TestConfiguration.ValidPostalCode);

      WebDriverExtensions.ClickAndWaitForUrl(CheckoutStepTwoPage.FinishBtn, CheckoutCompletePage.Url);
    }

    [TearDown]
    public void EndTest()
    {
      WebDriverExtensions.Driver.Quit();
    }

    [Test]
    public void GivenCheckCompletePageTwoOpen_WhenBackToHomeButtonClicked_ThenShouldRedirectToInventory()
    {
      var expected = InventoryPage.InventoryTitleClassName;

      // Act
      WebDriverExtensions.ClickAndWaitForUrl(CheckoutCompletePage.BackToHomeBtn, InventoryPage.Url);
      var pageTitle = WebDriverExtensions.ReadTextOnceSettled(expected, InventoryPage.Title);

      // Assert
      pageTitle.Should().Be(InventoryPage.Title);
    }
  }
}