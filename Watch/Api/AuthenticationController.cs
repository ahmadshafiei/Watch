using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Watch.DataAccess.Identity;
using Watch.DataAccess.Repositories;
using Watch.Models;

namespace Watch.Api
{
    public class AuthenticationController : ApiController
    {
        private readonly UserManager userManager;
        private readonly UserStore userStore;
        private readonly UserRepository userRepository;

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.Current.GetOwinContext().Authentication;
            }
        }

        public AuthenticationController(UserRepository userRepository , UserStore userStore)
        {
            this.userStore = userStore;
            this.userRepository = userRepository;
            this.userManager = new UserManager(userStore);
        }

        public async Task<IHttpActionResult> Register(User user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

           var identity = await  userManager.CreateAsync(user, user.Password);

            if (!identity.Succeeded)
                return BadRequest(String.Join(" - ",identity.Errors));

            return Ok(identity);
        }

        [HttpGet]
        public Task ResetPassword(string email)
        {
            return Task.FromResult(0);
        }

        [Authorize]
        public List<User> test()
        {
            return userRepository.GetAll();
        }
    }
}
