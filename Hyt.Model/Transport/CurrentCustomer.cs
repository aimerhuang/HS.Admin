using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transport
{
    public class CurrentCustomer
    {
        public DsWhCustomer CurrentCus { get; set; }
        public bool IsCustomer { get; set; }

        public bool IsDealer { get; set; }
    }
}
