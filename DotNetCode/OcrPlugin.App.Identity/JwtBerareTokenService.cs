using Microsoft.IdentityModel.Tokens;
using OcrPlugin.App.Common;
using OcrPlugin.App.Db;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace OcrPlugin.App.Identity
{
    public class JwtBerareTokenService : IJwtBerareTokenService
    {
        public string CreateJwtBearerToken(ApplicationUser user)
        {
            var secretkey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("eb043e2f-c564-4db7-acf0-04356671d196"));
            var credentials = new SigningCredentials(secretkey, SecurityAlgorithms.HmacSha256);

            var claims = new[] // NOTE: could also use List<Claim> here
            {
                new Claim(ClaimTypes.Name, user.Email), // NOTE: this will be the "User.Identity.Name" value
                new Claim(CustomClaimTypes.Company, user.Company),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, user.Email) // NOTE: this could a unique ID assigned to the user by a database
            };

            var token = new JwtSecurityToken(
                issuer: "domain.com",
                audience: "domain.com",
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
