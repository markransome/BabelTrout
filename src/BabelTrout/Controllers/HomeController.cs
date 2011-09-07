using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BabelTrout.Controllers
{
  public class HomeController : Controller
  {
    public ActionResult Index()
    {
        this.ViewData["Message"] = "Welcome to ASP.NET MVC!";

        return View();
    }

    public ActionResult About()
    {
        return View();
    }
  }
}
