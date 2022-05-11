using System;
using System.Collections.Generic;

#nullable disable

namespace WpfApp1
{
    public partial class Car
    {
        public Car()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string Make { get; set; }
        public string Color { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
