using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace AgeRangerAPI.Code
{
	public abstract class Database : IDisposable
	{
		protected string _StrConn = null;
		protected IDbConnection _Conn = null;


		public static bool IsDataSetEmpty(DataSet ds)
		{
			return (ds == null) || (ds.Tables == null) || (ds.Tables[0].Rows == null) || (ds.Tables[0].Rows.Count <= 0);
		}


		public Database(): this(null)
		{
		}

		public Database(string connectionString)
		{
			if (!string.IsNullOrEmpty(connectionString))
				_StrConn = connectionString;
			else
				_StrConn = GetDefaultConnectionString();
		}

		public void Dispose()
		{
			if (_Conn != null)
			{
				_Conn.Dispose();
				_Conn = null;
			}
		}


		protected abstract IDbConnection GetConnection(int timeOut = 0);

		protected abstract IDbDataAdapter GetDataAdapter();

		public abstract IDbDataParameter CreateParameter(string parameterName, object value);

		public abstract long GetLastInsertID();


		public string GetDefaultConnectionString()
		{
			if (System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"] != null)
				return System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

			return null;
		}

		public Exception Open(int timeOut = 0)
		{
			Exception result = null;

			try
			{
				if (_Conn == null)
				{
					_Conn = GetConnection(timeOut);

					if (!string.IsNullOrEmpty(_StrConn))
						_Conn.ConnectionString = _StrConn;

					_Conn.Open();

					result = CheckConnection();
				}
			}
			catch (Exception ex)
			{
				result = ex;

				Dispose();
			}

			return result;
		}

		public Exception CheckConnection()
		{
			Exception result = null;

			if (_Conn == null)
				result = Open();
			else if (_Conn.State != ConnectionState.Open)
			{
				Dispose();

				result = new Exception("Connection not open.");
			}

			return result;
		}

		public DataSet GetDataSet(string command, out Exception error)
		{
			DataSet result = null;

			error = CheckConnection();
			if (error == null)
			{
				try
				{
					IDbDataAdapter da = GetDataAdapter();
					da.SelectCommand.CommandText = command;

					result = new DataSet();
					da.Fill(result);
				}
				catch (Exception ex)
				{
					error = ex;
				}
			}

			return result;
		}

		public DataRow GetDataRow(string command, out Exception error)
		{
			DataRow result = null;
			DataSet ds = GetDataSet(command, out error);
			if ((error == null) && !IsDataSetEmpty(ds))
				result = ds.Tables[0].Rows[0];

			return result;
		}

		public int ExecuteNonQuery(string command, out Exception error)
		{
			int result = 0;

			error = CheckConnection();
			if (error == null)
			{
				try
				{
					IDbCommand cmd = _Conn.CreateCommand();
					cmd.CommandText = command;
					result = cmd.ExecuteNonQuery();
				}
				catch (Exception ex)
				{
					error = ex;
				}
			}

			return result;
		}

		public object ExecuteScalar(string command, out Exception error)
		{
			object result = null;

			error = CheckConnection();
			if (error == null)
			{
				try
				{
					IDbCommand cmd = _Conn.CreateCommand();
					cmd.CommandText = command;
					result = cmd.ExecuteScalar();
				}
				catch (Exception ex)
				{
					error = ex;
				}
			}

			return result;
		}

		public IDbCommand GetCommand(out Exception error)
		{
			IDbCommand result = null;

			error = CheckConnection();
			if (error == null)
				result = _Conn.CreateCommand();

			return result;
		}

		public IDbTransaction BeginTransaction(out Exception error)
		{
			IDbTransaction result = null;

			error = CheckConnection();
			if (error == null)
				result = _Conn.BeginTransaction();

			return result;
		}
	}
}