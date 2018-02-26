using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watch.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public string Type { get; set; }
        public int WatchId { get; set; }
        [ForeignKey("WatchId")]
        public Watch Watch { get; set; }
    }
}
