using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watch.Models
{
    [Table("WatchBookmark")]
    public class WatchBookmark
    {
        [Key]
        public int Id { get; set; }
        public int User_Id { get; set; }
        [ForeignKey("User_Id")]
        public User User { get; set; }
        public int Watch_Id { get; set; }
        [ForeignKey("Watch_Id")]
        public Watch Watch { get; set; }
    }
}
