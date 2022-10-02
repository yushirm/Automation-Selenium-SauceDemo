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
      WebDriverExtensions.Driver.FindElement(LoginPage.BtnLogin).Click();

      var pageTitle = WebDriverExtensions.Driver.FindElement(expected).Text;

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
      WebDriverExtensions.Driver.FindElement(LoginPage.BtnLogin).Click();

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

      WebDriverExtensions.WaitForItemDisplayed(InventoryPage.InventoryTitleClassName);


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
      WebDriverExtensions.Driver.FindElement(InventoryPage.AddToCartBackPackBtn).Click();
      var cartBadgeState = WebDriverExtensions.Driver.FindElement(expected).Text;

      // Assert
      cartBadgeState.Should().Be(InventoryPage.ShoppingCartBadgeOneItem);
    }

    [Test]
    public void GivenOneItemInShoppingCart_WhenItemRemoveFromCartButtonClicked_ThenShouldDecreaseShoppingCartBadgeValueToZero()
    {
      // Arrange
      var expected = InventoryPage.ShoppingCartLinkClassName;
      WebDriverExtensions.Driver.FindElement(InventoryPage.AddToCartBackPackBtn).Click();

      // Act
      WebDriverExtensions.Driver.FindElement(InventoryPage.RemoveFromCartBackPackBtn).Click();
      var cartLinkState = WebDriverExtensions.Driver.FindElement(expected).Text;

      // Assert
      cartLinkState.Should().Be(InventoryPage.ShoppingCartLinkEmpty);
    }

    [Test]
    public void GivenItemInShoppingCart_WhenShoppingCartButtonClicked_ThenShouldRedirectToShoppingCart()
    {
      // Arrange
      var expected = CartPage.CartTitleClassName;
      WebDriverExtensions.Driver.FindElement(InventoryPage.AddToCartBackPackBtn).Click();

      // Act
      WebDriverExtensions.Driver.FindElement(InventoryPage.ShoppingCartLinkClassName).Click();
      var pageTitle = WebDriverExtensions.Driver.FindElement(expected).Text;

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

      WebDriverExtensions.WaitForItemDisplayed(InventoryPage.InventoryTitleClassName);

      InventoryHelper.AddItemToCart(TestConfiguration.AddBackPackToCartButton);
      InventoryHelper.OpenCart();

      WebDriverExtensions.WaitForItemDisplayed(CartPage.CartTitleClassName);
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
      WebDriverExtensions.Driver.FindElement(CartPage.CheckOutBtn).Click();
      var pageTitle = WebDriverExtensions.Driver.FindElement(expected).Text;

      // Assert
      pageTitle.Should().Be(CheckoutStepOnePage.Title);
    }

    [Test]
    public void GivenShoppingCartOpen_WheContinueShoppingButtonClicked_ThenShouldRedirectToInventoryPage()
    {
      // Arrange
      var expected = InventoryPage.InventoryTitleClassName;

      // Act
      WebDriverExtensions.Driver.FindElement(CartPage.ContinueShoppingBtn).Click();
      var pageTitle = WebDriverExtensions.Driver.FindElement(expected).Text;

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

      WebDriverExtensions.WaitForItemDisplayed(InventoryPage.InventoryTitleClassName);

      InventoryHelper.AddItemToCart(TestConfiguration.AddBackPackToCartButton);
      InventoryHelper.OpenCart();

      WebDriverExtensions.WaitForItemDisplayed(CartPage.CartTitleClassName);

      WebDriverExtensions.Driver.FindElement(CartPage.CheckOutBtn).Click();

      WebDriverExtensions.WaitForItemDisplayed(CheckoutStepOnePage.CheckoutStepOneTitleClassName);


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
      WebDriverExtensions.Driver.FindElement(CheckoutStepOnePage.ContinueBtn).Click();

      var pageTitle = WebDriverExtensions.Driver.FindElement(expected).Text;

      // Assert
      pageTitle.Should().Be(CheckoutStepTwoPage.Title);
    }

    [Test]
    public void GivenCheckOutPageOneOpen_WhenContinueShoppingButtonClicked_ThenShouldReturnToCart()
    {
      // Arrange
      var expected = CartPage.CartTitleClassName;

      // Act
      WebDriverExtensions.Driver.FindElement(CheckoutStepOnePage.CancelBtn).Click();
      var pageTitle = WebDriverExtensions.Driver.FindElement(expected).Text;

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

      WebDriverExtensions.WaitForItemDisplayed(InventoryPage.InventoryTitleClassName);

      InventoryHelper.AddItemToCart(TestConfiguration.AddBackPackToCartButton);
      InventoryHelper.OpenCart();

      WebDriverExtensions.WaitForItemDisplayed(CartPage.CartTitleClassName);

      WebDriverExtensions.Driver.FindElement(CartPage.CheckOutBtn).Click();

      WebDriverExtensions.WaitForItemDisplayed(CheckoutStepOnePage.CheckoutStepOneTitleClassName);

      CheckOutStepOneHelper.PerformValidCheckOutStepOne(TestConfiguration.ValidFirstName, TestConfiguration.ValidLastName, TestConfiguration.ValidPostalCode);

      WebDriverExtensions.WaitForItemDisplayed(CheckoutStepTwoPage.CheckoutStepTwoTitleClassName);


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
      WebDriverExtensions.Driver.FindElement(CheckoutStepTwoPage.FinishBtn).Click();
      var pageTitle = WebDriverExtensions.Driver.FindElement(expected).Text;

      // Assert
      pageTitle.Should().Be(CheckoutCompletePage.Title);
    }

    [Test]
    public void GivenCheckOutPageTwoOpen_WhenCancelButtonClicked_ThenShouldReturnToInventory()
    {
      // Arrange
      var expected = InventoryPage.InventoryTitleClassName;

      // Act
      WebDriverExtensions.Driver.FindElement(CheckoutStepTwoPage.CancelBtn).Click();
      var pageTitle = WebDriverExtensions.Driver.FindElement(expected).Text;

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

      WebDriverExtensions.WaitForItemDisplayed(InventoryPage.InventoryTitleClassName);

      InventoryHelper.AddItemToCart(TestConfiguration.AddBackPackToCartButton);
      InventoryHelper.OpenCart();

      WebDriverExtensions.WaitForItemDisplayed(CartPage.CartTitleClassName);

      WebDriverExtensions.Driver.FindElement(CartPage.CheckOutBtn).Click();

      WebDriverExtensions.WaitForItemDisplayed(CheckoutStepOnePage.CheckoutStepOneTitleClassName);

      CheckOutStepOneHelper.PerformValidCheckOutStepOne(TestConfiguration.ValidFirstName, TestConfiguration.ValidLastName, TestConfiguration.ValidPostalCode);

      WebDriverExtensions.WaitForItemDisplayed(CheckoutStepTwoPage.CheckoutStepTwoTitleClassName);

      WebDriverExtensions.Driver.FindElement(CheckoutStepTwoPage.FinishBtn).Click();

      WebDriverExtensions.WaitForItemDisplayed(CheckoutCompletePage.CheckoutCompleteTitleClassName);

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
      WebDriverExtensions.Driver.FindElement(CheckoutCompletePage.BackToHomeBtn).Click();
      var pageTitle = WebDriverExtensions.Driver.FindElement(expected).Text;

      // Assert
      pageTitle.Should().Be(InventoryPage.Title);
    }
  }
}