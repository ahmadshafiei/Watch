using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watch.DataAccess.Repositories;
using Watch.Models;

namespace Watch.Business
{
    public class RoleBusiness
    {
        private readonly RoleRepository roleRepository;

        public RoleBusiness(RoleRepository roleRepository)
        {
            this.roleRepository = roleRepository;
        }

        public List<Role> GetAllRoles(out int count)
        {
            count = roleRepository.GetAll().Count;
            return roleRepository.GetAll().ToList();
        }
    }
}
