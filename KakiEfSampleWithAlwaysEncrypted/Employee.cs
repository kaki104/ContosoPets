using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KakiEfSampleWithAlwaysEncrypted
{
    /// <summary>
    /// Employee
    /// </summary>
    public class Employee
    {
        public int EmployeeID { get; set; }
        public string SSN { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal Salary { get; set; }
    }
}
