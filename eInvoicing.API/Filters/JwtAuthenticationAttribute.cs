using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using eInvoicing.Service.Helper;
using WebApi.Jwt.Filters;


namespace eInvoicing.API.Filters
{
    public class JwtAuthenticationAttribute : Attribute, IAuthenticationFilter
    {
        public string Realm { get; set; }
        public bool AllowMultiple => false;

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            var request = context.Request;
            var authorization = request.Headers.Authorization;

            if (authorization == null || authorization.Scheme != "Bearer")
                return;

            if (string.IsNullOrEmpty(authorization.Parameter))
            {
                context.ErrorResult = new AuthenticationFailureResult("Missing Jwt Token", request);
                return;
            }

            var token = authorization.Parameter;
            var principal = await AuthenticateJwtToken(context, token);

            if (principal == null)
                context.ErrorResult = new AuthenticationFailureResult("Invalid token", request);

            else
                context.Principal = principal;
        }

        private static bool ValidateToken(HttpAuthenticationContext context, string token, out Claim[] claims)
        {
            claims = null;

            var simplePrinciple = JwtManager.GetPrincipal(token);
            var identity = simplePrinciple?.Identity as ClaimsIdentity;

            if (identity == null)
                return false;

            if (!identity.IsAuthenticated)
                return false;

            claims = identity?.Claims.ToArray();
            var usernameClaim = identity?.FindFirst(ClaimTypes.Name);
            var username = usernameClaim?.Value;

            if (string.IsNullOrEmpty(username))
                return false;

            var pages = identity?.FindAll("Page").Select(i => i.Value).ToList();
            var permissions = identity?.FindAll("Permission").Select(i => i.Value).ToList();
            if (!permissions.Contains(context.Request.Method.Method.ToString()) || !pages.Select(x => x.ToLower()).Contains(context.Request.RequestUri.Segments[2]?.Replace("/", "")))
                return false;

            // More validate to check whether username exists in system

            return true;
        }

        protected Task<IPrincipal> AuthenticateJwtToken(HttpAuthenticationContext context, string token)
        {
            if (ValidateToken(context, token, out Claim[] claims ))
            {
                //// based on username to get more information from database in order to build local identity
                //var claims = new List<Claim>
                //{
                //    new Claim(ClaimTypes.Name, username)
                //    // Add more claims if needed: Roles, ...
                //};
                var identity = new ClaimsIdentity(claims, "Jwt");
                IPrincipal user = new ClaimsPrincipal(identity);

                return Task.FromResult(user);
            }

            return Task.FromResult<IPrincipal>(null);
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            Challenge(context);
            return Task.FromResult(0);
        }

        private void Challenge(HttpAuthenticationChallengeContext context)
        {
            string parameter = null;

            if (!string.IsNullOrEmpty(Realm))
                parameter = "realm=\"" + Realm + "\"";

            context.ChallengeWith("Bearer", parameter);
        }
    }
}
