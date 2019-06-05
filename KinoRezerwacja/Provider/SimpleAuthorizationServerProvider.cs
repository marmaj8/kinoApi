using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using System.Web.Mvc;

namespace KinoRezerwacja.Provider
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    //[RequireHttps]
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated(); //   
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            using (var db = new Models.kinoEntities())
            {
                if (db != null)
                {
                    var users = db.klient.ToList();
                    if (users != null)
                    {
                        var user = users.Find(u => u.email == context.UserName && u.haslo == context.Password);
                        //if (!string.IsNullOrEmpty(users.Where(u => u.UserEmail == context.UserName && u.UserPassword == context.Password).FirstOrDefault().UserName))
                        if (user != null)
                        {
                            identity.AddClaim(new Claim("Id", user.id.ToString()));

                            var props = new AuthenticationProperties(new Dictionary<string, string>
                            {
                                {
                                    "userdisplayname", user.imie + " " + user.nazwisko
                                }
                             });

                            var ticket = new AuthenticationTicket(identity, props);
                            context.Validated(ticket);
                        }
                        else
                        {
                            context.SetError("invalid_grant", "Provided email or password is incorrect");
                            context.Rejected();
                        }
                    }
                }
                else
                {
                    context.SetError("invalid_grant", "Provided email or password is incorrect");
                    context.Rejected();
                }
                return;
            }
        }
    }
}