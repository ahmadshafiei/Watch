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
    public class UserStore : IUserStore<User, int> , IUserPasswordStore<User,int> , IUserSecurityStampStore<User,int>
    {
        private readonly UserRepository userRepository;
        private readonly UnitOfWork.UnitOfWork unitOfWork;

        public UserStore(UserRepository userRepository, UnitOfWork.UnitOfWork unitOfWork)
        {
            this.userRepository = userRepository;
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
            return Task.Run(()=> {
                return userRepository.GetById(userId);
            });
        }

        public Task<User> FindByNameAsync(string userName)
        {
            return Task.Run(()=> {
                var user = userRepository.Get().Include(u => u.UserRoles).Where(u => u.UserName.Equals(userName)).FirstOrDefault();
                return user;
            });
        }

        public Task<string> GetPasswordHashAsync(User user)
        {
            return Task.FromResult(user.Password);
        }

        public Task<string> GetSecurityStampAsync(User user)
        {
            return Task.Run(() => {
                return user.SecurityStamp;
            });
        }

        public Task SetSecurityStampAsync(User user, string stamp)
        {
            return Task.Run(()=> {
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
            return Task.Run(()=> {
                userRepository.Update(user);
                unitOfWork.Commit();
            });
        }
    }
}
