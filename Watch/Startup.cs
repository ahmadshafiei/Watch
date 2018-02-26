using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Watch.DataAccess.Identity;

namespace Watch
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseOAuthBearerTokens(new OAuthAuthorizationServerOptions()
            {
                TokenEndpointPath = new PathString("/Token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
                Provider = new TokenProvider("self"),
                AllowInsecureHttp = true //Set to false on deployment
            });
        }
    }
}