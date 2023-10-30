using DAL.Helpers.PageHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
   public class BasicDataModels
    {
        #region Declare Data Class Objects
        public quiz_categories quiz_categories_obj { get; set; }
        public IEnumerable<quiz_categories> quiz_categories_list { get; set; }

        public quiz quiz_obj { get; set; }
        public IEnumerable<quiz> quiz_list { get; set; }
    
       public courses courses_obj { get; set; }
        public IEnumerable<courses> courses_list { get; set; }

      
    
        public course_categories course_categories_obj { get; set; }
        public IEnumerable<course_categories> course_categories_list { get; set; }

   
        public users user_obj { get; set; }
        public MulltiKeyValue MulltiKeyValue_obj { get; set; }
        public IEnumerable<MulltiKeyValue> MulltiKeyValue_list { get; set; }
        public GeneralData generalDataObj { get; set; }
        public IEnumerable<users> user_list { get; set; }
        public IEnumerable<ChartData> ChartDataList { get; set; }
        public PagerHelper? pageHelperObj { get; set; }
        public List<MulltiKeyValue>? DataList { get; set; }
        #endregion
    }
}
