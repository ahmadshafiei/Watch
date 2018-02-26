using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watch.Business.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string entityName):base(string.Format(Exceptionlist.NotFound,entityName))
        {

        }
    }
}
