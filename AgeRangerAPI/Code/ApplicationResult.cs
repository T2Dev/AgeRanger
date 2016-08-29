using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgeRangerAPI.Code
{
	public class ApplicationResultBase
	{
		public bool Success { get; set; }
		public string Message { get; set; }
		public Exception Ex { get; set; }

		public static bool IsSuccess(ApplicationResultBase result)
		{
			return (result != null) && result.Success;
		}

		public ApplicationResultBase SetFailure(string message, Exception ex = null)
		{
			Success = false;
			Message = message;
			Ex = ex;
			return this;
		}

		public ApplicationResultBase SetSuccess()
		{
			Success = true;
			Message = null;
			Ex = null;
			return this;
		}
	}

	public class ApplicationResultSuccess : ApplicationResultBase
	{
		public ApplicationResultSuccess()
		{
			SetSuccess();
		}
	}

	public class ApplicationResultFailure : ApplicationResultBase
	{
		public ApplicationResultFailure(string message)
		{
			this.SetFailure(message);
		}

		public ApplicationResultFailure(string message, Exception ex)
		{
			this.SetFailure(message, ex);
		}
	}

	public class ApplicationResultRecords : ApplicationResultBase
	{
		public Models.AgeRanger[] Records = null;

		public ApplicationResultRecords(Models.AgeRanger[] records)
		{
			SetSuccess();
			Records = records;
		}

		public ApplicationResultRecords(string message)
		{
			this.SetFailure(message, null);
		}

		public ApplicationResultRecords(string message, Exception ex)
		{
			this.SetFailure(message, ex);
		}
	}

	public class ApplicationResultRecord : ApplicationResultBase
	{
		public Models.AgeRanger Record = null;

		public ApplicationResultRecord(Models.AgeRanger record)
		{
			SetSuccess();
			Record = record;
		}

		public ApplicationResultRecord(string message)
		{
			this.SetFailure(message, null);
		}

		public ApplicationResultRecord(string message, Exception ex)
		{
			this.SetFailure(message, ex);
		}
	}
}