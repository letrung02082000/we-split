using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLibrary
{
    public class DestinationModel
    {
        public int DesId { get; set; }
        public string DesName { get; set; }
        public string Province { get; set; }
        public bool OldDestination { get; set; } = false;
    }
}
