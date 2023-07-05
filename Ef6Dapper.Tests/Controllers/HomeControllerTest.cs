﻿using Ef6Dapper;
using Ef6Dapper.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;

namespace Ef6Dapper.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // 排列
            HomeController controller = new HomeController();

            // 作用
            ViewResult result = controller.Index() as ViewResult;

            // 判斷提示
            Assert.IsNotNull(result);
            Assert.AreEqual("Home Page", result.ViewBag.Title);
        }
    }
}
