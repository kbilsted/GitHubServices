using System.Net.Http;
using System.Web.Http;
using GitHubServices.Controllers;
using GitHubServices.Models;
using NUnit.Framework;
using PowerAssert;
using StatePrinter;
using StatePrinter.Configurations;

namespace GitHubServices.Test.Controllers
{
  [TestFixture]
  [Category("UnitTest")]
  public class TocControllerTests
  {
    private readonly TocController controller = new TocController
                                                {
                                                  Request = new HttpRequestMessage(),
                                                  Configuration = new HttpConfiguration()
                                                };

    [Test]
    public void ContentDoesNotContainAnyHeaders()
    {
      // Arrange
      var expectedToc = new Toc { ToCValueForPasting = "# Table of Content" };
      const string content = "There is something here, but no headers";

      // Act
      var response = controller.CreateToc(content);

      // Assert
      var printer = GetTestPrinter();
      Toc actualToc = null;
      PAssert.IsTrue(() => response.TryGetContentValue(out actualToc));
      PAssert.IsTrue(() => printer.PrintObject(expectedToc, "") == printer.PrintObject(actualToc, ""));
    }

    [Test]
    public void ContentContainsSingleHeader()
    {
      // Arrange
      var expectedToc = new Toc { ToCValueForPasting = @"# Table of Content
* Right Here" 
      };
      const string content = @"There is a single header
# Right Here
But nothing more";

      // Act
      var response = controller.CreateToc(content);

      // Assert
      var printer = GetTestPrinter();
      Toc actualToc = null;
      PAssert.IsTrue(() => response.TryGetContentValue(out actualToc));
      PAssert.IsTrue(() => printer.PrintObject(expectedToc, "") == printer.PrintObject(actualToc, ""));
    }

    [Test]
    public void ContentContainsMultipleHeaders()
    {
      // Arrange
      var expectedToc = new Toc { ToCValueForPasting = @"# Table of Content
* One
* Two
* Three" 
      };
      const string content = @"There contains multiple headers
# One
But nothing more
# Two
# Three";

      // Act
      var response = controller.CreateToc(content);

      // Assert
      var printer = GetTestPrinter();
      Toc actualToc = null;
      PAssert.IsTrue(() => response.TryGetContentValue(out actualToc));
      PAssert.IsTrue(() => printer.PrintObject(expectedToc, "") == printer.PrintObject(actualToc, ""));
    }

    private static Stateprinter GetTestPrinter()
    {
      var cfg = ConfigurationHelper.GetStandardConfiguration();
      var printer = new Stateprinter(cfg);

      return printer;
    }
  }
}
