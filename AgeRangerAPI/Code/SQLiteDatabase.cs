using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Data.SQLite;

namespace AgeRangerAPI.Code
{
	public class SQLiteDatabase : Database
	{
		public SQLiteDatabase() : base()
		{
		}

		public SQLiteDatabase(string connectionString) : base(connectionString)
		{
		}

		protected override IDbConnection GetConnection(int timeOut = 0)
		{
			var conn = new SQLiteConnection(_StrConn);
			if (timeOut > 0)
				conn.BusyTimeout = timeOut;

			return conn;
		}

		protected override IDbDataAdapter GetDataAdapter()
		{
			return new SQLiteDataAdapter(((SQLiteConnection)_Conn).CreateCommand());
		}

		public override IDbDataParameter CreateParameter(string parameterName, object value)
		{
			return new SQLiteParameter(parameterName, value);
		}

		public override long GetLastInsertID()
		{
			IDbCommand cmd = _Conn.CreateCommand();
			cmd.CommandText = "SELECT last_insert_rowid()";

			object lastID = cmd.ExecuteScalar();

			if (lastID != null)
				return Convert.ToInt64(lastID);

			return -1;
		}
	}
}