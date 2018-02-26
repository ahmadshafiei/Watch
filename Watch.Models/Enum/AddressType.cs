using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watch.Models.Enum
{
    public enum AddressType
    {
        [Description("آدرس شخص")]
        UserAddress = 0 ,
        [Description("آدرس فروشگاه")]
        StoreAddress = 1,
    }
}
