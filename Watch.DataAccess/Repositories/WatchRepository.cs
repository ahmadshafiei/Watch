using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watch.DataAccess.Repositories
{
    public class WatchRepository:GenericRepository<Models.Watch>
    {
        public WatchRepository(WatchContext context):base(context)
        {

        }
    }
}
