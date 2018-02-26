using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watch.Models.Enum
{
    public enum Condition
    {
        [Description("نو")]
        New = 1,
        [Description("دست دوم")]
        SecondHand = 2,
        [Description("دست سوم")]
        ThirdHand = 3,
        [Description("دست چهارم")]
        ForthHand = 4,
        [Description("دست پنجم")]
        FifthHand = 5,
        [Description("دست ششم")]
        SixthHand = 6,
        [Description("دست هفتم")]
        SeventhHand = 7,
    }
}
