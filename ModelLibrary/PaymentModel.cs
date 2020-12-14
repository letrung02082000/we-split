using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLibrary
{
    public class PaymentModel
    {
        public int PaymentId { get; set; }
        public int JourneyId { get; set; }
        public int MemberId { get; set; }
        public string PaymentContent { get; set; }
        public int PaymentValue { get; set; }
        public string MemberName { get; set; }
    }
}
