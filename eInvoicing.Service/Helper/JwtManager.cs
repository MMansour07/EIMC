using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using eInvoicing.DTO;
using Microsoft.IdentityModel.Tokens;

namespace eInvoicing.Service.Helper
{
    public static class JwtManager
    {
        /// <summary>
        /// Use the below code to generate symmetric Secret Key
        ///     var hmac = new HMACSHA256();
        ///     var key = Convert.ToBase64String(hmac.Key);
        /// </summary>
        /// 

        public static string GenerateToken(UserDTO obj, int expireMinutes = 60)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["Client_Id"]));
            //var symmetricKey = Convert.FromBase64String("db3OIsj+BXE9NZDy0t8W3TcNekrF+2d/1sFnWG4HnV8TZY30iTOdtVWJG8abWvB1GlOgJuQZdcF2Luqm/hccMw==");
            var tokenHandler = new JwtSecurityTokenHandler();
            var now = DateTime.UtcNow;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = getClaimsIdentity(obj),
                //Expires = now.AddMinutes(Convert.ToInt32(30)),
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
            };
            SecurityToken securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(securityToken);
            return token;
        }

        public static ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

                if (jwtToken == null)
                    return null;
                var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["Client_Id"]));
                var validationParameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(hmac.Key)
                };
                var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
                return principal;
            }

            catch (Exception excep)
            {
                return null;
            }
        }

        private static ClaimsIdentity getClaimsIdentity(UserDTO obj)
        {
            return new ClaimsIdentity(getClaims());

            Claim[] getClaims()
            {
                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Name, obj.UserName));
                claims.Add(new Claim(ClaimTypes.NameIdentifier, obj.Id));
                foreach (var item in obj.stringfiedRoles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, item));
                }
                foreach (var item in obj.stringfiedPrivileges)
                {
                    claims.Add(new Claim("Page", item));
                }
                foreach (var item in obj.stringfiedPermissions)
                {
                    claims.Add(new Claim("Permission", item));
                }
                return claims.ToArray();
            }

        }
    }

}