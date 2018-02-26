using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watch.Business.Exceptions
{
    public class NotAllowedException:Exception
    {
        public NotAllowedException(string actionName):base(string.Format(Exceptionlist.NotAllowed,actionName))
        {
                
        }
    }
}
