using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CityInfo.API.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration configuration;

        public class AuthenticationRequestBody 
        {
            public string? Username { get; set; }
            public string? Password { get; set; }
        }

        private class CityUserInfo
        {

            public int UserId { get; set; }
            public string Username { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string City { get; set; }

            public CityUserInfo(int userId, string username, string firstName, string lastName, string city)
            {
                UserId = userId;
                Username = username;
                FirstName = firstName;
                LastName = lastName;
                City = city;
            }
        }

        public AuthenticationController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }


        [HttpPost("authenticate")]
        public IActionResult Authenticate(AuthenticationRequestBody requestBody)
        {
            // Step 1: Validate User Credntials
            var user = ValidateUserCredentials(requestBody.Username, requestBody.Password);

            if(user == null)
            {
                return Unauthorized();
            }

            var key = configuration["Authentication:SecretForKey"];
            // Step 2: Create Token
            var securityKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(key)
                );
            // Step 2.1: Create 
            var signinCredentials = new SigningCredentials( securityKey, SecurityAlgorithms.HmacSha256 );

            // Step 2.3 create claims
            var claimsForToken = new List<Claim>();
            claimsForToken.Add(new Claim("sub", user.UserId.ToString()));
            claimsForToken.Add(new Claim("given_name", user.FirstName));
            claimsForToken.Add(new Claim("family_name", user.LastName));
            claimsForToken.Add(new Claim("city", user.City));

            // Step 2.4 create token
            var jwtSecurityToken = new JwtSecurityToken(
                configuration["Authentication:Issuer"],
                configuration["Authentication:Audience"],
                claimsForToken,
                DateTime.UtcNow,
                DateTime.UtcNow.AddHours(1),
                signinCredentials
                );

            var tokenToReturn = new JwtSecurityTokenHandler()
                .WriteToken(jwtSecurityToken);

            return Ok(tokenToReturn);
        }

        private CityUserInfo ValidateUserCredentials(string username, string password)
        {
            return new CityUserInfo(  
                1,
                username ?? "",
                "Mohamed",
                "Samir",
                "Cairo"
            );

        }
    }
}
