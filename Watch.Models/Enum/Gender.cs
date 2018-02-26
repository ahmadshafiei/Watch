using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watch.Models.Enum
{
    public enum Gender
    {
        [Description("مرد")]
        Male = 0,
        [Description("زن")]
        Female = 1,
        [Description("هر دو")]
        Both = 2
    }
}
