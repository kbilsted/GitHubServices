using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using GitHubServices.Models;

namespace GitHubServices.Controllers
{
  public class TocController : ApiController
  {
    public HttpResponseMessage CreateToc(string content)
    {
      Console.WriteLine("Content_Console: {0}", content);
      Debug.WriteLine("Content_Debug: {0}", content);

      var fakeTocLines = content
        .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
        .Where(x => x.StartsWith("#"))
        .Select(x => x.Replace("#", ""))
        .ToList();

      var tocString = "# Table of Content";
      if (fakeTocLines.Any())
        tocString += Environment.NewLine + "*" + String.Join(Environment.NewLine + "*", fakeTocLines);
      return Request.CreateResponse(new Toc { ToCValueForPasting = tocString });
    }
  }
}
