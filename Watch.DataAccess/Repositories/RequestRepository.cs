using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watch.Models;

namespace Watch.DataAccess.Repositories
{
    public class RequestRepository:GenericRepository<Request>
    {
        public RequestRepository(WatchContext context):base(context)
        {
            
        }
    }
}
