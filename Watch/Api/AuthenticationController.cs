using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Watch.Business;
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
        private readonly AuthenticationBusiness authenticationBusiness;

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.Current.GetOwinContext().Authentication;
            }
        }

        public AuthenticationController(UserRepository userRepository, UserStore userStore, AuthenticationBusiness authenticationBusiness)
        {
            this.userStore = userStore;
            this.userRepository = userRepository;
            this.userManager = new UserManager(userStore);
            this.authenticationBusiness = authenticationBusiness;
        }

        public async Task<IResponse> Register(User user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    StringBuilder stringBuilder = new StringBuilder();

                    foreach (var error in ModelState.Keys)
                        stringBuilder.Append($"{error} : {ModelState[error]} , ");

                    throw new Exception(stringBuilder.ToString());
                }

                await authenticationBusiness.Register(user);
                return new Response<User>();
            }
            catch (Exception e)
            {
                return new Response<User>
                {
                    Success = false,
                    Message = e.Message
                };
            }
        }

        [HttpGet]
        public async Task<IResponse> ResetPassword(string email)
        {
            try
            {
                await authenticationBusiness.ResetPassword(email);
                return new Response<User>();
            }
            catch (Exception e)
            {
                return new Response<User>
                {
                    Success = false,
                    Message = e.Message
                };
            }
        }
    }
}
