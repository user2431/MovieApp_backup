using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DataModels
{
    public class Notes
    {
        public int NoteId { get; set; }
        public string UserId { get; set; }
        public string Tconst { get; set; }
        public string Note { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? CreatedDate { get; set; }

        public user_info User { get; set; }
        public title_basics Movie { get; set; }
    }
}
