using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DataModels
{
    public class UserRating
    {
        public string user_id { get; set; }
        public string tconst { get; set; }
        public int rating { get; set; }
        public DateTime rated_date { get; set; }
        public NameBasics namebasics { get; set; }
        public title_basics Title_Basics { get; set; }
    }
}
