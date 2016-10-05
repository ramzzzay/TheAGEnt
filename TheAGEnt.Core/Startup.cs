using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using TheAGEnt.Core.Util;

[assembly: OwinStartup(typeof(TheAGEnt.Core.Startup))]

namespace TheAGEnt.Core
{
    public class Startup
    {
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        public static string PublicClientId { get; private set; }

        public void Configuration(IAppBuilder app)
        {
            AutofacConfig.Configure(app);

            // Configure & enable the application for OAuth based flow
            PublicClientId = "self";
            app.UseOAuthBearerTokens(
                OAuthOptions = new OAuthAuthorizationServerOptions
                {
                    TokenEndpointPath = new PathString("/Token"),
                    Provider = new ApplicationOAuthProvider(PublicClientId),
                    AuthorizeEndpointPath = new PathString("/api/Account/ExternalLogin"),
                    AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
                    // In production mode set AllowInsecureHttp = false
                    AllowInsecureHttp = true
                });

            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalBearer);
        }
    }
}
