using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watch.DataAccess.Repositories;
using Watch.Models;
using System.Data.Entity;

namespace Watch.DataAccess.Identity
{
    public class UserStore : IUserStore<User, int>, IUserPasswordStore<User, int>, IUserSecurityStampStore<User, int>, IUserRoleStore<User, int>, IUserLockoutStore<User, int> , IUserTwoFactorStore<User,int>
    {
        private readonly UserRepository userRepository;
        private readonly RoleRepository roleRepository;
        private readonly UserRoleRepository userRoleRespository;
        private readonly UnitOfWork.UnitOfWork unitOfWork;

        public UserStore(UserRepository userRepository, RoleRepository roleRepository, UserRoleRepository userRoleRespository, UnitOfWork.UnitOfWork unitOfWork)
        {
            this.userRepository = userRepository;
            this.roleRepository = roleRepository;
            this.userRoleRespository = userRoleRespository;
            this.unitOfWork = unitOfWork;
        }
        public Task CreateAsync(User user)
        {
            return Task.Run(() =>
            {
                userRepository.Insert(user);
                unitOfWork.Commit();
            });
        }

        public Task DeleteAsync(User user)
        {
            return Task.Run(() =>
            {
                userRepository.Delete(user);
                unitOfWork.Commit();
            });
        }

        public void Dispose()
        {

        }

        public Task<User> FindByIdAsync(int userId)
        {
            return Task.Run(() =>
            {
                return userRepository.GetById(userId);
            });
        }

        public Task<User> FindByNameAsync(string userName)
        {
            return Task.Run(() =>
            {
                var user = userRepository.Get().Include(u => u.UserRoles.Select(ur => ur.Role)).Where(u => u.UserName.Equals(userName)).FirstOrDefault();
                return user;
            });
        }

        public Task<string> GetPasswordHashAsync(User user)
        {
            return Task.FromResult(user.Password);
        }

        public Task<string> GetSecurityStampAsync(User user)
        {
            return Task.Run(() =>
            {
                return user.SecurityStamp;
            });
        }

        public Task SetSecurityStampAsync(User user, string stamp)
        {
            return Task.Run(() =>
            {
                user.SecurityStamp = stamp;
            });
        }

        public Task<bool> HasPasswordAsync(User user)
        {
            return Task.FromResult(!string.IsNullOrEmpty(user.Password));
        }

        public Task SetPasswordHashAsync(User user, string passwordHash)
        {
            user.Password = passwordHash;
            return Task.FromResult(0);
        }

        public Task UpdateAsync(User user)
        {
            return Task.Run(() =>
            {
                userRepository.Update(user);
                unitOfWork.Commit();
            });
        }

        public Task AddToRoleAsync(User user, string roleName)
        {
            return Task.Run(() =>
            {
                if (!roleRepository.Get().Any(r => r.Name.Equals(roleName)))
                    throw new Exception($"Role : {roleName} Not Found !!");

                user.UserRoles.Add(new UserRole
                {
                    Role = roleRepository.Get().First(r => r.Name == roleName)
                });

                userRepository.Update(user);
                unitOfWork.Commit();
            });
        }

        public Task RemoveFromRoleAsync(User user, string roleName)
        {
            throw new NotImplementedException();
        }

        public Task<IList<string>> GetRolesAsync(User user)
        {
            return Task.Run(() =>
            {
                IList<string> res = new List<string>();

                foreach (var role in userRoleRespository.Get().Where(ur => ur.UserId == user.Id).Select(ur => ur.Role.Name))
                    res.Add(role);

                return res;
            });
        }

        public Task<bool> IsInRoleAsync(User user, string roleName)
        {
            return Task.Run(() =>
            {
                if (userRoleRespository.Get().Where(ur => ur.UserId == user.Id).Any(ur => roleName == ur.Role.Name))
                    return true;

                return false;
            });
        }

        public Task<DateTimeOffset> GetLockoutEndDateAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task SetLockoutEndDateAsync(User user, DateTimeOffset lockoutEnd)
        {
            throw new NotImplementedException();
        }

        public Task<int> IncrementAccessFailedCountAsync(User user)
        {
            return Task.FromResult(0);
        }

        public Task ResetAccessFailedCountAsync(User user)
        {
            return Task.FromResult(0);
        }

        public Task<int> GetAccessFailedCountAsync(User user)
        {
            return Task.FromResult(0);
        }

        public Task<bool> GetLockoutEnabledAsync(User user)
        {
            return Task.FromResult(false);
        }

        public Task SetLockoutEnabledAsync(User user, bool enabled)
        {
            return Task.FromResult(0);
        }

        public Task SetTwoFactorEnabledAsync(User user, bool enabled)
        {
            return Task.FromResult(0);
        }

        public Task<bool> GetTwoFactorEnabledAsync(User user)
        {
            return Task.FromResult(false);
        }
    }
}
