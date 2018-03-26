using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watch.Models.Enum;

namespace Watch.Models
{
    [Table("Address")]
    public class Address
    {
        [Key]
        public int Id { get; set; }
        public string City { get; set; }
        public string FullAddress { get; set; }
        public string PhoneNumber { get; set; }

        #region [Personal Information Part Of Adding Addresses]
        public string Name { get; set; }
        public string Family { get; set; }
        public string NationalCode { get; set; }
        public Gender Gender { get; set; }
        #endregion

        //Determine address type for user or store : NotNullable
        public AddressType Type { get; set; }
        public int User_Id { get; set; }
        [ForeignKey("User_Id")]
        public User User { get; set; }    
    }
}
