using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using BOSAPI.Configurations;
using BOSAPI.Contexts;
using BOSAPI.Entities;
using BOSAPI.Helpers;
using BOSAPI.Interfaces;
using BOSAPI.Models;
using BOSAPI.Utils;

namespace BOSAPI.Services
{
    public class UserService : IUserService
    {
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
        private readonly IOptions<AppSettingsConfiguration> _appSettingsConfiguration;
        private readonly BOSDBContext _bosdbContext;

        public UserService(IOptions<AppSettingsConfiguration> appSettingsConfiguration, BOSDBContext bosdbContext)
        {
            _appSettingsConfiguration = appSettingsConfiguration;
            _bosdbContext = bosdbContext;
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var users = _bosdbContext.User.Where(x => x.UserName == model.Username).ToList();

            if (users.Count == 0)
                throw new Exception("User Is not exist");

            if (users.Count > 1)
                throw new Exception("Has two user with the same name");

            var user = users.FirstOrDefault();

            string hashedPassword = Misc.ComputeSHA512(model.Password, user?.UserPasswordSalt);

            // return null if user not found
            if (user?.UserPassword != hashedPassword) return null;

            // authentication successful so generate jwt token
            var token = generateJwtToken(user);

            return new AuthenticateResponse(user, token);
        }

        public IEnumerable<User> GetAll()
        {
            return _bosdbContext.User.ToList();
        }

        public User GetById(int id)
        {
            // return _users.FirstOrDefault(x => x.Id == id);

            return new User();
        }

        // helper methods

        private string generateJwtToken(User user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettingsConfiguration.Value.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.UserId.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
