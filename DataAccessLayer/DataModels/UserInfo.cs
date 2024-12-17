using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DataModels
{
    public class user_info
    {
        public string user_id { get; set; }
        public  string username { get; set; }
        public  string email { get; set; }
        public  string user_psw { get; set; }
        public DateTime created_date { get; set; }
        public  ICollection<Notes> Notes { get; set; }
        public  ICollection<BookMark> BookMarks { get; set; }
    }
}