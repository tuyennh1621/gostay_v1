using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
  public  class courses
    {

		public int course_id { get; set; }
		public string? title { get; set; }
		public int? status { get; set; }
		public string? status_name { get; set; }
		
		public int? course_category_id { get; set; }
		public string? cousre_category_name { get; set; }
		public string? description { get; set; }
		public string? guid { get; set; }

		public string? created_at { get; set; }

		public int? created_by { get; set; }

		public string? modified_at { get; set; }
		public int? modified_by { get; set; }
		public int? is_deleted { get; set; }

		public string? resultMsg { get; set; }

		public bool IsSuccess { get; set; }

		public string? ReturnURL { get; set; }

		public int TotalRecordCount { get; set; }
		public int pageId { get; set; }
		public int ItemsPerPage { get; set; }
		public string? created_by_username { get; set; }

		//--not mapped to database table(courses) properties
		[NotMapped]
		public int student_course_id { get; set; }
		[NotMapped]
		public int teacher_assign_id { get; set; }
	}
}
