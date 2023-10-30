using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Entities
{
    public class Menu
    {
        #region Properties
        public int ID { get; set; }
        public int? ParentID { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }
        public string Links { get; set; }
        public int? STT { get; set; }
        public int? LANGID { get; set; }
        public int? TabID { get; set; }
        public int? PageID { get; set; }
        #endregion


    }
}