using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class ChartData
    {
        public string? Label { get; set; }
        public string? Data { get; set; }
    }
    public class FooterData
    {
        public string? TotalStudents { get; set; }
        public string? TotalCourses { get; set; }
    }

}
