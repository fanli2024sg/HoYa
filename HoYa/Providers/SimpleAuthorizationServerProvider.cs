using HoYa.Entities;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HoYa.Repository;
using System.Web;
namespace HoYa.Providers
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        //1.

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            /*
           string clientId = string.Empty;
           string clientSecret = string.Empty;
           Client client = null;

           if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
           {
               context.TryGetFormCredentials(out clientId, out clientSecret);
           }

           if (context.ClientId == null)
           {
               //Remove the comments from the below line context.SetError, and invalidate context 
               //if you want to force sending clientId/secrects once obtain access tokens. 
               context.Validated();
               //context.SetError("invalid_clientId", "ClientId should be sent.");
               return Task.FromResult<object>(null);
           }

           using (AuthenticateRepository _repo = new AuthenticateRepository())
           {
               client = _repo.FindClient(context.ClientId);
           }

           if (client == null)
           {
               context.SetError("invalid_clientId", string.Format("Client '{0}' is not registered in the system.", context.ClientId));
               return Task.FromResult<object>(null);
           }

           if (client.HoYalicationType == Models.HoYalicationTypes.NativeConfidential)
           {
               if (string.IsNullOrWhiteSpace(clientSecret))
               {
                   context.SetError("invalid_clientId", "Client secret should be sent.");
                   return Task.FromResult<object>(null);
               }
               else
               {
                   if (client.Secret != Helper.GetHash(clientSecret))
                   {
                       context.SetError("invalid_clientId", "Client secret is invalid.");
                       return Task.FromResult<object>(null);
                   }
               }
           }

           if (!client.Active)
           {
               context.SetError("invalid_clientId", "Client is inactive.");
               return Task.FromResult<object>(null);
           }

           context.OwinContext.Set<string>("as:clientAllowedOrigin", client.AllowedOrigin);
           context.OwinContext.Set<string>("as:clientRefreshTokenLifeTime", client.RefreshTokenLifeTime.ToString());
           */
            context.Validated();
            return Task.FromResult<object>(null);
        }

        //2.
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            AspNetUser user;
            AspNetRole role;
            var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");
            if (allowedOrigin == null) allowedOrigin = "*";

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

            using (AuthRepository repository = new AuthRepository())
            {
                user = await repository.FindUser(context.UserName, context.Password);
                if(user!=null) role = await repository.FindRole(user.Roles.FirstOrDefault(r => r.UserId == user.Id).RoleId);
            }


            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Name, user.Id));
            identity.AddClaim(new Claim(ClaimTypes.WindowsAccountName, user.UserName));
            identity.AddClaim(new Claim("sub", user.UserName));

            ClaimsPrincipal principal = new ClaimsPrincipal(identity);
            HttpContext.Current.User = principal;


            var props = new AuthenticationProperties(new Dictionary<string, string>
                {
                    {
                        "userName", context.UserName
                    }
                });

            var ticket = new AuthenticationTicket(identity, props);
            context.Validated(ticket);

        }
        /*
        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            var originalClient = context.Ticket.Properties.Dictionary["as:client_id"];
            var currentClient = context.ClientId;

            if (originalClient != currentClient)
            {
                context.SetError("invalid_clientId", "Refresh token is issued to a different clientId.");
                return Task.FromResult<object>(null);
            }

            // Change auth ticket for refresh token requests
            var newIdentity = new ClaimsIdentity(context.Ticket.Identity);

            var newClaim = newIdentity.Claims.Where(c => c.Type == "newClaim").FirstOrDefault();
            if (newClaim != null)
            {
                newIdentity.RemoveClaim(newClaim);
            }
            newIdentity.AddClaim(new Claim("newClaim", "newValue"));

            var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);
            context.Validated(newTicket);

            return Task.FromResult<object>(null);
        }
        */
        //3.
        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

    }
}