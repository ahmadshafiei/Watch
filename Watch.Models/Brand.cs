using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watch.Models
{
    [Table("Brand")]
    public class Brand
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Watch> Watches { get; set; }
    }
}
