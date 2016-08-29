using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AgeRangerAPI;
using AgeRangerAPI.Code;
using AgeRangerAPI.Controllers;
using AgeRangerAPI.Models;

namespace AgeRangerAPI.Tests.Controllers
{
	[TestClass]
	public class RestControllerTest
	{
		public string GetConnectionString()
		{
			return $"Data Source={AppDomain.CurrentDomain.BaseDirectory}\\AgeRangerTest.db";
		}

		[TestMethod]
		public void A_Post()
		{
			RestController api = new RestController(GetConnectionString());

			AgeRanger ar1 = new AgeRanger() { FirstName = "Test", LastName = "One", Age = 10 };
			AgeRanger ar2 = new AgeRanger() { FirstName = "Test", LastName = "Two", Age = 20 };

			ApplicationResultRecord result1 = api.Post(ar1);
			Assert.IsTrue(result1.Success);
			Assert.IsNotNull(result1.Record);
			Assert.IsTrue(result1.Record.ID > 0);

			ApplicationResultRecord result2 = api.Post(ar2);
			Assert.IsTrue(result2.Success);
			Assert.IsNotNull(result2.Record);
			Assert.IsTrue(result2.Record.ID > 0);
		}

		[TestMethod]
		public void B_Get()
		{
			RestController api = new RestController(GetConnectionString());

			ApplicationResultRecords result1 = api.Get();
			Assert.IsTrue(result1.Success);
			Assert.IsTrue(result1.Records.Count() >= 2);

			ApplicationResultRecord result2 = api.Get(result1.Records[0].ID);
			Assert.IsTrue(result2.Success);
			Assert.IsNotNull(result2.Record);
			Assert.IsTrue(result2.Record.ID == result1.Records[0].ID);

			ApplicationResultRecords result3 = api.Get("Test", null);
			Assert.IsTrue(result3.Success);
			Assert.IsNotNull(result3.Records);
			Assert.IsTrue(result3.Records.Count() == 2);
			Assert.IsTrue(result3.Records[0].FirstName == "Test");

			ApplicationResultRecords result4 = api.Get(null, "Two");
			Assert.IsTrue(result4.Success);
			Assert.IsNotNull(result4.Records);
			Assert.IsTrue(result4.Records.Count() == 1);
			Assert.IsTrue(result4.Records[0].LastName == "Two");
		}

		[TestMethod]
		public void C_Delete()
		{
			RestController api = new RestController(GetConnectionString());

			ApplicationResultRecords result1 = api.Get();

			ApplicationResultBase result2 = api.Delete(result1.Records[0].ID);
			Assert.IsTrue(result2.Success);

			ApplicationResultBase result3 = api.Delete(result1.Records[1].ID);
			Assert.IsTrue(result3.Success);

			ApplicationResultRecords result4 = api.Get();
			Assert.IsTrue(result4.Success);
			Assert.IsNull(result4.Records);
		}
	}
}