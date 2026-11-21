<p align="center">
  <a href="" rel="noopener">
 <img width=200px height=200px src="Media\ProjectLogo.png" alt="Project logo"></a>
</p>

<h3 align="center">
Automation-Selenium-SauceDemo</h3>

<div align="center">

[![.NET tests](https://github.com/yushirm/Automation-Selenium-SauceDemo/actions/workflows/dotnet-core.yml/badge.svg?branch=main)](https://github.com/yushirm/Automation-Selenium-SauceDemo/actions/workflows/dotnet-core.yml)
[![GitHub Issues](https://img.shields.io/github/issues/yushirm/Automation-Selenium-SauceDemo.svg)](https://github.com/yushirm/Automation-Selenium-SauceDemo/issues)
[![GitHub Pull Requests](https://img.shields.io/github/issues-pr/yushirm/Automation-Selenium-SauceDemo.svg)](https://github.com/yushirm/Automation-Selenium-SauceDemo/pulls)

</div>

---

A test automation project using the Selenium Automation Framework that targets Sauce Labs [Swag Labs](https://www.saucedemo.com/) site.

This suite was later ported to Playwright Java: [automation-playwright-saucedemo](https://github.com/yushirm/automation-playwright-saucedemo).

> **Known issue: CI is intermittently red on the cart and checkout pages.** Some runs lose clicks and typing on those pages: the page is loaded and the control is on top, but the input never reaches it. A retry doesn't help. The browser and driver version doesn't explain it either. ChromeDriver's verbose log from a red run shows that a dropped click is sent to Chrome at the button's centre, with the same parameters as a click that works, and Chrome acknowledges it, but no `click` event reaches the page. So the input is lost inside Chrome, after the driver. [Evidence so far](https://github.com/yushirm/Automation-Selenium-SauceDemo/pull/11#issuecomment-7041514264); [verbose-log run](https://github.com/yushirm/Automation-Selenium-SauceDemo/actions/runs/44797671144). The investigation is parked. CI keeps ChromeDriver's verbose log for every test (`TestResults/chromedriver` in the `test-results` artifact), so a future red run can be read.

## 📝 Table of Contents

- [About](#about)
    - [Test Design Approach](#test-design-approach-a-name--testdesigna)
- [Getting Started](#getting_started)
- [Usage](#usage)
- [Tools Used](#built_using)
- [TODO](./TODO.md)
- [Authors](#authors)

## 🧐 About <a name = "about"></a>

The purpose of this project is to show how I would create and manage a Selenium Test Automation Framework. The project consists of functional tests for the SauceDemo website, specifically, using automation to simulate expected user behaviour (e.g. Login, Add to Cart, Checkout, etc). It's written in C# on .NET 10. NUnit and Fluent Assertions were used to manage test execution and assertion.

### Test Design Approach <a name = "test_design"></a>
Test were created using the [Page Object Model](https://www.browserstack.com/guide/page-object-model-in-selenium#:~:text=Page%20Object%20Model%2C%20also%20known,application%20as%20a%20class%20file.) in Selenium. It is a design pattern used in test automation that creates and Object Repository for the web UI elements of the different pages of a website. An outline of how this was implemented is shown below:

![Page Object Model](Media/PageObjectModel.png)

Tests are seperated into various classes depending on the page object to be tested. Isolation of tests helps for easier code management and makes it easier to determine Page Object Passed/Failed when running the tests.

![Test Classes](Media/TestClasses.png)

The tests were written in the style of <b>Given_When_Then</b>, which is a TDD/BDD approach that helps link test cases with end user interaction and behaviour. It makes the expected outcome more readable leading to test results being easier to interpret by non-technical stakeholders.

![Given When Then](Media/GivenWhenThenApproach.png)

The UserFlow test suite (SauceDemo.UserFlow.Tests.cs) contains and End-to-End user journey for the site, i.e. From <b>Logging In</b> to <b>Completing a Purchase</b>. The tests assert that the correct elements are present after the user performs an action (e.g. correct Page Header is present).

There are more scenarios that need to be covered in future so this solution is still a work in progress. Planned future work includes adding tests for the following:
- Asserting additional page elements and correct element functionality
- Tests for different user types
- Negative test cases

The scope of this work in outlined in [TODO.md](./TODO.md).

## 🏁 Getting Started <a name = "getting_started"></a>
This section outlines what you will need to run the tests locally.

### Prerequisites
This test solution was built and run on the following:
- Windows OS 
- Chrome Browser

The solution was first built in 2022 on .NET 6 with Selenium 4.5 and a pinned ChromeDriver package. In November 2026 it moved to:
- .NET 10
- Selenium.WebDriver and Selenium.Support 4.53
- NUnit 5 with NUnit3TestAdapter 6
- FluentAssertions 7.2 (version 8 needs a paid licence, so it stays on 7)

*There's no ChromeDriver package any more. Selenium Manager, built into Selenium since 4.6, downloads the driver that matches the Chrome installed on the machine, so the old version-mismatch problem goes away.*

### Installing
<b>Cloning the repository</b>

Create clone of the repository on your local machine via command-line or other management tool.

```
$ git clone https://github.com/yushirm/Automation-Selenium-SauceDemo/
```


<b>Installing Visual Studio Professional 2022</b>

You can download Visual Studio Professional 2022 from [Here](https://visualstudio.microsoft.com/vs/).
- Run the installer
- At the Work Loads screen make sure to select the following:
    - ASP.NET and Web Development
    - .NET Desktop development

![WorkFlows](Media/VisualStudioInstaller-WorkFlows.png)

- Confirm that the .NET 10 SDK is installed by checking the Individual Components tab

![IndividualComponents](Media/VisualStudioInstallerIndividualComponents.png)

- Complete the installation


<b>Package Management</b>

The other dependencies can be managed via the Nuget package manager in Visual Studio. This will only be available when the solution is loaded. These do not need to be installed by default as they are included in the solutionl.

- You can confirm they are installed by navigating to the Nuget Package Manager on Visual Studio

![NPMNavigate](Media/NugetNavigate.png)

- Click on the Installed tab to see packages and manage versions. You can also change versions here.

![NPMInstalled](Media/NugetPM.png)


## 🔧 Running the tests <a name = "tests"></a>
### Visual Studio Test Runner
Follow the steps below to run the tests in Visual Studio Professional 2022.

- Select <b>File > Open > Project/Solution</b> in Visual Studio

![VSOpenMenu](Media/VSOpenMenu.png)

- Navigate to your checked out repository and open <b>Automation.Selenium.SauceDemo.sln</b>
- Select <b>Test > Run All Tests</b> to start all tests (or can start them in Test Explorer: <b>Test > Test Explorer</b>)

![VSTestNavigate](Media/VSTestNavigate.png)

- Build and Test will begin in the Test Explorer window. Drop down to view the outcomes for each test.

![VSTestExplorer](Media/VSTestExplorer.png)

### Command-Line using Dotnet
You can also execute the test using the command-line via dotnet

- From the repository root, run:

```
dotnet test
```

You need Google Chrome installed. The tests run headless.
The test run will display as follows in the command window:

![VSTestExplorer](Media/dotnetOutput.png)

### Disable Chrome Headless Mode
The tests are desiged to run in Chrome Headless by default. You can disable setting to view test execution in the Browser.

- Open the file <b>".\Helpers\Libraries\Configuration\TestConfiguration.cs"</b>
- Change the attribute <b>SupportedBrowser</b> to "GoogleChrome" as shown:

![ChromeHeadless](Media/ChromeHeadless.png) ![ChromeUI](Media/ChromeUI.png)

The tests wil now run with Chrome UI enabled

## ⛏️ Tools Used <a name = "built_using"></a>
- [Visual Studio Professional](https://visualstudio.microsoft.com/vs/) - 2022 V17.3.5
- [.NET 10](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)
- Testing Frameworks
    - [Selenium](https://www.selenium.dev/)
    - [NUnit](https://nunit.org/)
    - [Fluent Assertions](https://fluentassertions.com/)

## ✍️ Authors <a name = "authors"></a>
- [yushirm](https://www.linkedin.com/in/yushir-rugnath-maharaj-44b86222/) - Initial design and coding