﻿using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Сontinuer;
using Сontinuer.Controllers;

namespace Сontinuer.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        //TODO Написать тетсты, если будет время
        [TestMethod]
        public void Index()
        {
            // Arrange
            var controller = new HomeController();

            // Act
            var result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Home Page", result.ViewBag.Title);
        }
    }
}
