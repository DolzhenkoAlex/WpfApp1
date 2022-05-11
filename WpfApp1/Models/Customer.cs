using System;
using System.Collections.Generic;

#nullable disable

namespace WpfApp1
{
    public partial class Customer: ICloneable
    {
        public Customer()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Customer(int id, string firstName, string lastname)
        {
            this.Id = id;
            this.FirstName = firstName;
            this.LastName = lastname;
            Orders = new HashSet<Order>();
        }

        public virtual ICollection<Order> Orders { get; set; }

        public object Clone()
        {
            return new Customer(Id, FirstName, LastName);
        }
    }
}
