using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watch.Models;
using System.Security.Claims;

namespace Watch.DataAccess.Identity
{
    public class UserManager : UserManager<User, int>
    {
        public UserManager(IUserStore<User, int> store) : base(store)
        {
            EmailService = new EmailService();
        }

        public override Task SendSmsAsync(int userId, string message)
        {
            throw new NotImplementedException();
        }

        public override Task SendEmailAsync(int userId, string subject, string body)
        {
            throw new NotImplementedException();
        }

        public override Task<User> FindAsync(string userName, string password)
        {
            return Task.Run(() =>
            {
                var passwordHasher = new PasswordHasher();
                var user = Store.FindByNameAsync(userName).Result;
                if (passwordHasher.VerifyHashedPassword(user.Password, password) == PasswordVerificationResult.Failed)
                    return null;
                return user;
            });
        }

        public override Task<User> FindByNameAsync(string userName)
        {
            return Task.Run(() =>
            {
                return Store.FindByNameAsync(userName).Result;
            });
        }

        public async override Task<ClaimsIdentity> CreateIdentityAsync(User user, string authenticationType)
        {
            var identity = await base.CreateIdentityAsync(user, authenticationType);
            identity.AddClaim(new Claim("username", user.UserName));
            foreach (var role in user.UserRoles)
                identity.AddClaim(new Claim(ClaimTypes.Role, role.RoleId.ToString()));
            return identity;

        }

        public override Task<IdentityResult> CreateAsync(User user, string password)
        {
            return Task.Run(() =>
            {
                var identity = base.CreateAsync(user, password).Result;
                return identity;
            });
        }

        //public override Task<User> FindByNameAsync(string userName)
        //{
        //    return Task.Run();
        //}

    }
}
