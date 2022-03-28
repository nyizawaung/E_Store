using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using EStore.Model;
using LazyCache;
using Microsoft.IdentityModel.Tokens;
using EStore.Helper;

namespace EStore.Services
{
    public class TokenService : ITokenService
    {
        IAppCache cache;
        
        public TokenService()
        { 
            //cache  = new CachingService(); 
        }

        private const double EXPIRY_DAY = 1;
        public string BuildToken(string key, string issuer, admin user)
        {
            var claims = new[] {
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(ClaimTypes.NameIdentifier,
            Guid.NewGuid().ToString())
        };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new JwtSecurityToken(issuer, issuer, claims,
                expires: DateTime.Now.AddDays(EXPIRY_DAY), signingCredentials: credentials);
            
            var token= new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
            var lastString = token.Split('.')[token.Split('.').Length - 1];
            token = token.Substring(0, token.LastIndexOf('.')+1);
            var redisCache = RedisHelper.Connection.GetDatabase();
            redisCache.StringSet($"{user.ID}",token);
            //cache.Add<string>($"{user.ID}", token, DateTimeOffset.UtcNow.AddDays(1));
            return lastString;
        }
        public bool IsTokenValid(string key, string issuer, string token,int userID=1)
        {
            var redisCache = RedisHelper.Connection.GetDatabase();
            var hiddenpart = redisCache.StringGet($"{userID}").ToString();
            //var hiddenpart = cache.Get<string>($"{userID}");
            if (string.IsNullOrEmpty(hiddenpart))
            {
                return false;
            }
            token =hiddenpart + token;
            var mySecret = Encoding.UTF8.GetBytes(key);
            var mySecurityKey = new SymmetricSecurityKey(mySecret);
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token,
                new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = issuer,
                    ValidAudience = issuer,
                    IssuerSigningKey = mySecurityKey,
                }, out SecurityToken validatedToken);
            }
            catch
            {
                return false;
            }
            return true;
        }
    }

}
