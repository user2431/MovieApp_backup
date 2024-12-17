using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DataModels
{
    public class NameBasics
    {
        public string nconst { get; set; }
        public string PrimaryName { get; set; }
        public string BirthYear { get; set; }
        public string DeathYear { get; set; }
        public string PrimaryProfession { get; set; }
        public string KnownforTitles { get; set; }
        public ICollection<title_principals> Title_Principals { get; set; }
        public ICollection<UserRating> UserRatings { get; set; }
    }
}
