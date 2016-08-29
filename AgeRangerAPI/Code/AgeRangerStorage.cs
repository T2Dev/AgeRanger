using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace AgeRangerAPI.Code
{
	public class AgeRangerStorage : IDisposable
	{
		private SQLiteDatabase _DB = null;

		public AgeRangerStorage(string connectionString = null)
		{
			_DB = new SQLiteDatabase(connectionString);
		}

		public void Dispose()
		{
			_DB.Dispose();
		}

		public string List(out IEnumerable<Models.AgeRanger> list, string firstName = null, string lastName = null)
		{
			list = null;
			string where = null;

			if (!string.IsNullOrEmpty(firstName))
				where += $"firstName = '{firstName.Replace("'", "''")}'";

			if (!string.IsNullOrEmpty(lastName))
				where += (where != null ? " OR " : null) + $"lastName = '{lastName.Replace("'", "''")}'";

			Exception error;
			DataSet ds = _DB.GetDataSet(@"
				SELECT p.*, a.Description AS AgeGroup FROM Person p
				LEFT JOIN AgeGroup a ON (a.MaxAge IS NULL OR a.MaxAge >= p.Age) AND (a.MinAge IS NULL OR a.MinAge <= p.Age)" + (where != null ? $" WHERE {where}" : null), out error);

			if (error == null)
			{
				if (!Database.IsDataSetEmpty(ds))
					list = ds.Tables[0].Rows.Cast<DataRow>().Select(x => new Models.AgeRanger(x)).ToList<Models.AgeRanger>();
			}
			else
				return error.Message;

			return null;
		}

		public string GetAgeGroup(int age)
		{
			Exception error;
			object data = _DB.ExecuteScalar($"SELECT a.Description FROM AgeGroup a WHERE (a.MaxAge IS NULL OR a.MaxAge >= {age}) AND (a.MinAge IS NULL OR a.MinAge <= {age})", out error);

			if ((error == null) && (data is string))
				return (string)data;

			return null;
		}

		public string Get(long id, out Models.AgeRanger ageRanger)
		{
			ageRanger = null;

			Exception error;
			DataRow dr = _DB.GetDataRow($"SELECT * FROM Person WHERE Id = {id}", out error);

			if (error == null)
			{
				if (dr != null)
					ageRanger = new Models.AgeRanger(dr);
			}
			else
				return error.Message;

			return null;
		}

		public string Save(ref Models.AgeRanger record)
		{
			string result = null;

			try
			{
				if (record != null)
				{
					result = record.Validate();

					if (string.IsNullOrEmpty(result))
					{
						Exception error;
						IDbCommand cmd = _DB.GetCommand(out error);

						if (error == null)
						{
							int lastAge = record.Age;

							cmd.Parameters.Add(_DB.CreateParameter("@F1", record.FirstName));
							cmd.Parameters.Add(_DB.CreateParameter("@F2", record.LastName));
							cmd.Parameters.Add(_DB.CreateParameter("@F3", record.Age));

							if (record.ID > 0)
							{
								cmd.CommandText = $"UPDATE Person SET FirstName = @F1, LastName = @F2, Age = @F3 WHERE ID = {record.ID}";
								if (cmd.ExecuteNonQuery() <= 0)
									result = "Fail to update!";
							}
							else
							{
								cmd.Transaction = _DB.BeginTransaction(out error);
								cmd.CommandText = $"INSERT INTO Person (FirstName, LastName, Age) VALUES (@F1, @F2, @F3)";

								int rows = cmd.ExecuteNonQuery();
								if (rows == 1)
									record.ID = _DB.GetLastInsertID();

								if (record.ID > 0)
									cmd.Transaction.Commit();
								else
								{
									cmd.Transaction.Rollback();
									result = "Fail to insert!";
								}
							}

							if (string.IsNullOrEmpty(result))
								record.AgeGroup = GetAgeGroup(record.Age);
						}
						else
							result = error.Message;
					}
				}
				else
					result = "Fail to save!";
			}
			catch (Exception ex)
			{
				result = ex.Message;
			}

			return result;
		}

		public string Delete(long id)
		{
			try
			{
				Exception error;
				int rows = _DB.ExecuteNonQuery($"DELETE FROM Person WHERE ID = {id}", out error);

				if (error != null)
					return error.Message;

				return null;
			}
			catch (Exception ex)
			{
				return ex.Message;
			}
		}
	}
}