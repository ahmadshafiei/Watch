using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watch.Business.Exceptions;
using Watch.DataAccess.Identity;
using Watch.DataAccess.Repositories;
using Watch.Models;

namespace Watch.Business
{
    public class AuthenticationBusiness
    {
        private readonly UserManager userManager;
        private readonly UserRepository userRepository;

        public AuthenticationBusiness(UserManager userManager, UserRepository userRepository)
        {
            this.userManager = userManager;
            this.userRepository = userRepository;
        }

        public async Task Register(User user)
        {
            var identity = await userManager.CreateAsync(user, user.Password);

            if (!identity.Succeeded)
                throw new Exception(String.Join(" - ", identity.Errors));

            identity = await userManager.AddToRoleAsync(user.Id, "User");

            if (!identity.Succeeded)
                throw new Exception(String.Join(" - ", identity.Errors));
        }

        public async Task ResetPassword(string email)
        {
            User user = await userManager.FindByNameAsync(email);

            if (user == null)
                throw new NotFoundException("کاربر");

            string code = await userManager.GeneratePasswordResetTokenAsync(user.Id);

            //Redirect to webpage
            await userManager.SendEmailAsync(user.Id, "باز یابی رمز عبور", "");
        }

        public List<string> GetCurrentRoles(string username)
        {
            User user = userRepository.Get().Include(u => u.UserRoles.Select(ur => ur.Role)).Where(u => u.UserName == username).SingleOrDefault();

            if (user == null)
                throw new NotFoundException("کاربر");

            return user.UserRoles.Select(ur => ur.Role.Name).ToList();

        }
    }
}
