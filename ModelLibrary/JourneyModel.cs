using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLibrary
{
    public class JourneyModel
    {
        public int JourneyId { get; set; }
        public string JourneyName { get; set; }
        public string JourneyDescription { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string CoverImage { get; set; }
    }
}
