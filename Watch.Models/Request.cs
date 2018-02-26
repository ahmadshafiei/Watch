using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watch.Models
{
    public class Request
    {
        public int Id { get; set; }
        public int? Address_Id { get; set; }
        [ForeignKey("Address_Id")]
        public Address Address { get; set; }
        public bool IsPaid { get; set; }
        public int User_Id { get; set; }
        [ForeignKey("User_Id")]
        public User User { get; set; }
        public int Watch_Id { get; set; }
        [ForeignKey("Watch_Id")]
        public Watch Watch { get; set; }
    }
}
