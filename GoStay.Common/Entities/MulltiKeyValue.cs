using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Entities
{
	public class MulltiKeyValue
	{
		#region Properties
		public int Id { get; set; }
		public string Title { get; set; }
		public double? FloatValue { get; set; }
		public string TextValue { get; set; }
		public int? IdGroup { get; set; }
		public string? Icon { get; set; }
        #endregion


	}
}