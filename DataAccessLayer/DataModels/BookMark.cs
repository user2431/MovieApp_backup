using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DataModels
{
    public class BookMark
    {
        public int bookmark_id { get; set; }
        public string userid { get; set; }
        public string title_id { get; set; }
        public user_info User { get; set; }
        public title_basics Movie { get; set; }
    }
}
