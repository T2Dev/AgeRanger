using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AgeRangerAPI.Code;
using AgeRangerAPI.Models;

namespace AgeRangerAPI.Controllers
{
	public class RestController : ApiController
	{
		protected AgeRangerStorage _Storage = null;

		public RestController()
		{
			_Storage = new AgeRangerStorage();
		}

		public RestController(string connectionString)
		{
			_Storage = new AgeRangerStorage(connectionString);
		}

		public ApplicationResultRecords Get()
		{
			ApplicationResultRecords result = null;

			try
			{
				IEnumerable<AgeRanger> list;
				string error = _Storage.List(out list);

				if (string.IsNullOrEmpty(error))
					result = new ApplicationResultRecords(list?.ToArray());
				else
					result = new ApplicationResultRecords("Fail to read records!");
			}
			catch (Exception ex)
			{
				result = new ApplicationResultRecords("Fail to read records!", ex);
			}

			return result;
		}

		public ApplicationResultRecords Get(string firstName, string lastName)
		{
			ApplicationResultRecords result = null;

			try
			{

				IEnumerable<AgeRanger> list;
				string error = _Storage.List(out list, firstName, lastName);

				if (string.IsNullOrEmpty(error))
					result = new ApplicationResultRecords(list?.ToArray());
				else
					result = new ApplicationResultRecords("Fail to read records!");
			}
			catch (Exception ex)
			{
				result = new ApplicationResultRecords("Fail to read record!", ex);
			}

			return result;
		}

		public ApplicationResultRecord Get(long id)
		{
			ApplicationResultRecord result = null;

			try
			{

				Models.AgeRanger record;
				string error = _Storage.Get(id, out record);

				if (record != null)
					result = new ApplicationResultRecord(record);
				else if (string.IsNullOrEmpty(error))
					result = new ApplicationResultRecord("Record not found!");
				else
					result = new ApplicationResultRecord("Fail to read record!");
			}
			catch (Exception ex)
			{
				result = new ApplicationResultRecord("Fail to read record!", ex);
			}

			return result;
		}

		public ApplicationResultRecord Post([FromBody]AgeRanger value)
		{
			ApplicationResultRecord result = null;

			try
			{
				string error = _Storage.Save(ref value);
				if (string.IsNullOrEmpty(error))
					result = new ApplicationResultRecord(value);
				else
					result = new ApplicationResultRecord(error, null);
			}
			catch (Exception ex)
			{
				result = new ApplicationResultRecord("Fail to save record!", ex);
			}

			return result;
		}

		public ApplicationResultBase Delete(long id)
		{
			ApplicationResultBase result = null;

			try
			{
				string error = _Storage.Delete(id);

				if (string.IsNullOrEmpty(error))
					result = new ApplicationResultSuccess();
				else
					result = new ApplicationResultFailure(error);
			}
			catch (Exception ex)
			{
				result = new ApplicationResultFailure("Fail to erase record!", ex);
			}

			return result;
		}

		public HttpResponseMessage Options()
		{
			return new HttpResponseMessage { StatusCode = HttpStatusCode.OK };
		}
	}
}
