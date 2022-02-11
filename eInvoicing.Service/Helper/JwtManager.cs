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
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = getClaimsIdentity(obj),
                Expires = DateTime.UtcNow.AddDays(1),
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
                    ClockSkew = TimeSpan.Zero,
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(hmac.Key)
                };
                var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
                return principal;
            }

            catch (Exception ex)
            {
                throw ex;
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
                claims.Add(new Claim("BusinessGroup", obj.BusinessGroup));
                claims.Add(new Claim("IsDBSync", obj.IsDBSync.ToString()));
                claims.Add(new Claim("BusinessGroupId", obj.BusinessGroupId));
                claims.Add(new Claim("Token", obj.Token));
                claims.Add(new Claim("SRN", obj.SRN));
                //claims.Add(new Claim("RIN", obj.RIN));
                //foreach (var item in obj.stringfiedRoles)
                //{
                //    claims.Add(new Claim(ClaimTypes.Role, item));
                //}
                //foreach (var item in obj.stringfiedPermissions)
                //{
                //    claims.Add(new Claim("Permission", item));
                //}
                return claims.ToArray();
            }

        }
    }

}