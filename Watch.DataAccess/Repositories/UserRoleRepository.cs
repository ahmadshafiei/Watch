using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watch.Models;

namespace Watch.DataAccess.Repositories
{
    public class UserRoleRepository : GenericRepository<UserRole>
    {
        public UserRoleRepository(WatchContext dbContext) : base(dbContext)
        {
        }
    }
}
