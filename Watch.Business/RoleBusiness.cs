using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watch.Business.Exceptions;
using Watch.DataAccess.Repositories;
using Watch.Models;

namespace Watch.Business
{
    public class RoleBusiness
    {
        private readonly RoleRepository roleRepository;
        private readonly UserRepository userRepository;
        private readonly UserRoleRepository userRoleRepository;

        public RoleBusiness(RoleRepository roleRepository, UserRepository userRepository, UserRoleRepository userRoleRepository)
        {
            this.roleRepository = roleRepository;
            this.userRepository = userRepository;
            this.userRoleRepository = userRoleRepository;
        }

        public List<Role> GetAllRoles(out int count)
        {
            count = roleRepository.GetAll().Count;
            return roleRepository.GetAll().ToList();
        }

        public List<string> GetUserRole(string username)
        {
            User user = userRepository.Get().Where(u => u.UserName == username).SingleOrDefault();

            if (user == null)
                throw new NotFoundException("کاربر");

            return userRoleRepository.Get().Where(ur => ur.UserId == user.Id).Include(ur => ur.Role).Select(ur => ur.Role.Name).ToList();
        }
    }
}
