﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLibrary
{
    public class PaymentPerMemberModel
    {
        public int MemberId { get; set; }
        public string MemberName { get; set; }
        public int PaymentValue { get; set; }
    }
}
