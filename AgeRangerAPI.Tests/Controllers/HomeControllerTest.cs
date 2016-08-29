using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AgeRangerAPI;
using AgeRangerAPI.Controllers;

namespace AgeRangerAPI.Tests.Controllers
{
	[TestClass]
	public class HomeControllerTest
	{
		[TestMethod]
		public void Index()
		{
			// Arrange
			HomeController controller = new HomeController();

			// Act
			ViewResult result = controller.Index() as ViewResult;

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual("Api Ranger Home", result.ViewBag.Title);
		}
	}
}
