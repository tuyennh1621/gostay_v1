using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
   public class quiz_categories
    {

		public int category_id { get; set; }


		public string? category_name { get; set; }

		public int is_active { get; set; }

		public string? created_at { get; set; }

		public int created_by { get; set; }

		public string? modified_at { get; set; }


		public int modified_by { get; set; }


		public int is_deleted { get; set; }

		//--Exra fields
		public string? ErrorMsg{ get; set; }
		public string? SuccessMsg { get; set; }
		public bool IsSuccess { get; set; }
	

		public string? ReturnURL { get; set; }

		public int TotalRecordCount { get; set; }
		public int pageId { get; set; }
		public int ItemsPerPage { get; set; }
        public string? created_by_username { get; set; }



    }
}
