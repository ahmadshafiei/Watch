
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Watch.Models;

namespace Watch.DataAccess.Identity
{
    public class TokenProvider : OAuthAuthorizationServerProvider
    {
        public UserManager userManager;
        private readonly string _publicClientId;

        public TokenProvider(string publicClientId)
        {
            _publicClientId = publicClientId ?? throw new ArgumentNullException("publicClientId");
            userManager = new UserManager(new UserStore(new Repositories.UserRepository(new WatchContext()), new UnitOfWork.UnitOfWork(new WatchContext())));
        }
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            if (context.ClientId == null)
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            User user = await userManager.FindAsync(context.UserName, context.Password);

            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }

            ClaimsIdentity oAuthIdentity = await userManager.CreateIdentityAsync(user,
               OAuthDefaults.AuthenticationType);
            ClaimsIdentity cookiesIdentity = await userManager.CreateIdentityAsync(user,
                CookieAuthenticationDefaults.AuthenticationType);

            cookiesIdentity.AddClaim(new Claim(ClaimTypes.Role, String.Join(",", user.UserRoles.Select(ur => ur.RoleId).ToArray())));

            AuthenticationProperties properties = CreateProperties(user);
            AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
            context.Validated(ticket);
            context.Request.Context.Authentication.SignIn(cookiesIdentity);
        }


        public static AuthenticationProperties CreateProperties(User user)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", user.UserName },
            };
            return new AuthenticationProperties(data);
        }
    }
}
