﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NUnit.Framework;
using BabelTrout;
using BabelTrout.Controllers;

namespace BabelTrout.Tests.Controllers
{
  [TestFixture]
  public class HomeControllerTest
  {
    [Test]
    public void Index()
    {
      // Arrange
      HomeController controller = new HomeController();

      // Act
      ViewResult result = controller.Index() as ViewResult;

      // Assert
      Assert.That("Welcome to ASP.NET MVC!", Is.EqualTo(result.ViewData["Message"]));
    }

    [Test]
    public void About()
    {
      // Arrange
      HomeController controller = new HomeController();

      // Act
      ViewResult result = controller.About() as ViewResult;

      // Assert
      Assert.That(result, Is.Not.Null);
    }
  }
}
