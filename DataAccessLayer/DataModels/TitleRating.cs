using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DataModels
{
    public class TitleRating
    {
        public string tconst { get; set; }
        public double averagerating { get; set; }
        public int numvotes { get; set; }
    }
}
