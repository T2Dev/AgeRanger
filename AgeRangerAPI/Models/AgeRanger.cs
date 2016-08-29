using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Data;

namespace AgeRangerAPI.Models
{
	public class AgeRanger
    {
		public long ID { get; set; }

		public string FirstName { get; set; }
		public string LastName { get; set; }
		public int Age { get; set; }
		public string AgeGroup { get; set; }

		public AgeRanger()
        {
			Default();
		}

		public AgeRanger(DataRow dr)
		{
			ID = dr.Field<long>(nameof(ID));
			FirstName = dr.Field<string>(nameof(FirstName));
			LastName = dr.Field<string>(nameof(LastName));
			Age = (int)dr.Field<long>(nameof(Age));

			if (dr.Table.Columns.Contains(nameof(AgeGroup)))
				AgeGroup = dr.Field<string>(nameof(AgeGroup));
		}

		public void Default()
		{
			ID = 0;
			FirstName = null;
			LastName = null;
			Age = 0;
			AgeGroup = null;

		}

		public string Validate()
		{
			if (string.IsNullOrWhiteSpace(FirstName))
				return $"Field {nameof(FirstName)} is mandatory!";

			if (string.IsNullOrWhiteSpace(LastName))
				return $"Field {nameof(LastName)} is mandatory!";

			if (Age <= 0)
				return $"Field {nameof(Age)} must be greater than 0!";

			return null;
		}

		public new string ToString()
		{
			return $"{this.GetType().Name}(FirstName: {FirstName}, LastName: {LastName}, Age: {Age})";
		}
	}
}