using Automation.Selenium.SauceDemo.Helpers.Libraries.Elements;
using Automation.Selenium.SauceDemo.Helpers.Extensions;

namespace Automation.Selenium.SauceDemo.Helpers.Scenarios.Setup.CheckOutStepOne
{
  internal class CheckOutStepOneHelper
  {
    public static void PerformValidCheckOutStepOne(string firstName, string lastName, string postalCode)
    {
      WebDriverExtensions.ResetElementText(CheckoutStepOnePage.TxtFirstName, firstName);
      WebDriverExtensions.ResetElementText(CheckoutStepOnePage.TxtLastName, lastName);
      WebDriverExtensions.ResetElementText(CheckoutStepOnePage.TxtPostalCode, postalCode);
      WebDriverExtensions.Driver.FindElement(CheckoutStepOnePage.ContinueBtn).Click();
    }
  }
}
