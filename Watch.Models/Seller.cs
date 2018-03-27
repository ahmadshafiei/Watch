using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watch.Models
{
    public class Seller
    {
        public int Id { get; set; }
        public string StoreName { get; set; }
        public string Tell { get; set; }
        public string PhoneNumber { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string LogoPath { get; set; }
        public int User_Id { get; set; }
        [ForeignKey("User_Id")]
        public User User { get; set; }
        public List<StoreBookmark> StoreBookmarks { get; set; }
    }
}
