using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Net.Http;
using System.Web.Helpers;
using BAL;
using GardenViewModel;

namespace GardenWaalaAPI.Security
{
    public class CustomAuthorizationAttribute : AuthorizeAttribute
    {
        private const string BasicAuthResponseHeader = "WWW-Authenticate";
        private const string BasicAuthResponseHeaderValue = "Basic";

        public string AddOnFeatures { get; set; }
        public string SubRole { get; set; }
        public string WebPlan { get; set; }
        protected CustomPrincipal CurrentUser
        {
            get
            {
                return Thread.CurrentPrincipal as CustomPrincipal;
            }
            set
            {
                Thread.CurrentPrincipal = value as CustomPrincipal;
            }
        }

        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            try
            {
                AuthenticationHeaderValue authValue = actionContext.Request.Headers.Authorization;
                if (authValue != null && !string.IsNullOrWhiteSpace(authValue.Parameter) && authValue.Scheme == BasicAuthResponseHeaderValue)
                {
                    Credential parsedCredentials = ParseAuthorizationHeader(authValue.Parameter);
                    if (parsedCredentials != null)
                    {
                        IUserRepository repoUser = new UserRepository();
                        CurrentUserViewModel userapimodel = null;

                        userapimodel = repoUser.ValidateEmail(parsedCredentials.Username, Crypto.SHA1(parsedCredentials.Password), parsedCredentials.Username);

                        if (userapimodel != null)
                        {
                            CurrentUser = new CustomPrincipal(parsedCredentials.Username) { UserId = userapimodel.UserId, Email = userapimodel.Email, Role = userapimodel.Role, Username = userapimodel.Name, EmailId = userapimodel.Email };
                            if (!CurrentUser.IsInRole(this.Roles) && !CurrentUser.Features.Contains(AddOnFeatures))
                            {

                                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden);
                                actionContext.Response.Headers.Add(BasicAuthResponseHeader, BasicAuthResponseHeaderValue);
                                return;
                            }
                        }
                        else
                        {
                            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden);
                            actionContext.Response.Headers.Add(BasicAuthResponseHeader, BasicAuthResponseHeaderValue);
                            return;
                        }
                    }
                    else
                    {
                        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden);
                        actionContext.Response.Headers.Add(BasicAuthResponseHeader, BasicAuthResponseHeaderValue);
                        return;
                    }
                }
                else
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden);
                    actionContext.Response.Headers.Add(BasicAuthResponseHeader, BasicAuthResponseHeaderValue);
                    return;
                }
            }

            catch (Exception)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                actionContext.Response.Headers.Add(BasicAuthResponseHeader, BasicAuthResponseHeaderValue);
                return;
            }
        }

        private Credential ParseAuthorizationHeader(string authHeader)
        {
            string[] credentials = Encoding.ASCII.GetString(Convert.FromBase64String(authHeader)).Split(new[] { ':' });



            if (credentials.Length == 3 && !string.IsNullOrEmpty(credentials[0]) && !string.IsNullOrEmpty(credentials[1]) && !string.IsNullOrEmpty(credentials[2]))
                return new Credential() { Username = credentials[0], Password = credentials[1], UserEmail = credentials[2] };
            else if (credentials.Length == 2 && !string.IsNullOrEmpty(credentials[0]) && !string.IsNullOrEmpty(credentials[1]))
                return new Credential() { Username = credentials[0], Password = credentials[1], UserEmail = "" };
            else
                return null;

        }
    }
}