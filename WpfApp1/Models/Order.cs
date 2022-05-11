using System;
using System.Collections.Generic;

#nullable disable

namespace WpfApp1
{
    public partial class Order
    {
        public int Id { get; set; }
        public int Custid { get; set; }
        public int Carid { get; set; }

        public virtual Car Car { get; set; }
        public virtual Customer Cust { get; set; }
    }
}
