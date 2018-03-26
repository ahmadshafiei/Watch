using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watch.Models
{
    [Table("Store")]
    public class Store
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int User_Id { get; set; }
        [ForeignKey("User_Id")]
        public User User { get; set; }
        public string LogoPath { get; set; }
        public List<StoreBookmark> StoreBookmarks { get; set; }
    }
}
