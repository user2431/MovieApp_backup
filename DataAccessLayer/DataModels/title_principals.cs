using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DataModels
{
    public class title_principals
    {
        public string tconst { get; set; }
        public string nconst { get; set; }
        public NameBasics nameBasics { get; set; }
        public title_basics titleBasics { get; set; }
    }
}
