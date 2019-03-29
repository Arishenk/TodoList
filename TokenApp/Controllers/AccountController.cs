using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Newtonsoft.Json;
using System.Security.Claims;
using TodoItems.Models.Users;
using System.Data.SQLite;
using System.Data.Common;
using System.Threading;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;
using TodoItems.Models.Users.Repositories;
using TodoItems.Client.Users;

namespace TokenApp.Controllers
{
    public class AccountController : Controller
    {
                
        private readonly DatabaseUserRepository userRepository = new DatabaseUserRepository();

        [Route("/token2")]
        public Task Token2()
        {
            return Task.FromResult<int>(0);
        }

        [HttpPost("/token")]
        public async Task Token([FromBody]UserRegistrationInfo registrationInfo, CancellationToken cancellationToken)
        {
            
            var username = registrationInfo.Login;
            var password = registrationInfo.Password;

            var identity = GetIdentity(username, password);
            if (identity == null)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("Invalid username or password.");
                return;
            }

            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name
            };

            // сериализация ответа
            Response.ContentType = "application/json";
            await Response.WriteAsync(JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented }));
        }

        private ClaimsIdentity GetIdentity(string username, string pass/*UserRegistrationInfo userInfo*/)
        {
            if (pass == null)
            {
                throw new ArgumentNullException(nameof(pass));
            }

            User1 person = null;

            try
            {
                person = this.userRepository.Get(username);
            }
            catch (UserNotFoundException)
            {
                throw new AuthenticationException();
            }

            var currentHash = this.HashPassword(pass);
            person.Role = "admin";

            if (!person.PasswordHash.Equals(currentHash))
            {
                person = null;
            }
            
            if (person != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, person.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, person.Role)
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            return null;
        }

        [HttpPost("/token1")]
        public async Task Token1([FromBody]UserRegistrationInfo registrationInfo, CancellationToken cancellationToken)
        {
            var username = registrationInfo.Login;
            var password = registrationInfo.Password;
            
            var creationInfo = new UserCreationInfo(username, this.HashPassword(password));
            var getUser = this.userRepository.Get(username);

            if (getUser != null)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("user already exists");
                throw new UserDuplicationException(registrationInfo.Login);            }

            try
            {
                this.userRepository.Create(creationInfo);
            }
            catch (UserNotFoundException)
            {
                throw new AuthenticationException();
            }

            var identity = this.GetIdentity(username, password);
            if (identity == null)
            {
                this.Response.StatusCode = 400;
                await this.Response.WriteAsync("Invalid username or password.");
                return;
            }

            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name
            };

            // сериализация ответа
            this.Response.ContentType = "application/json";
            await this.Response.WriteAsync(JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented }));
        }
                               
        private string HashPassword(string password)
        {
            using (var md5 = MD5.Create())
            {
                var passwordBytes = Encoding.UTF8.GetBytes(password);
                var hashBytes = md5.ComputeHash(passwordBytes);
                var hash = BitConverter.ToString(hashBytes);
                return hash;
            }
        }
    }
}