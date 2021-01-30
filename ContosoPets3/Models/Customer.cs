using System.Collections.Generic;

namespace ContosoPets3.Models
{
    public class Customer
    {
#nullable enable 
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
#nullable disable
        public string Email { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
