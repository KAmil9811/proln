using CGC.Funkcje.UserFuncFolder.UserReturn;
using CGC.Helpers;
using CGC.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using CGC.Entities;

namespace CGC.Services
{
    public interface IUserService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        IEnumerable<Entities.User> GetAll();
        Entities.User GetById(int id);
    }

    public class UserService : IUserService
    {
        static UserBaseReturn userBaseReturn = new UserBaseReturn();
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications

        private readonly AppSettings _appSettings;

        public UserService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var user = userBaseReturn.GetUsersToLogin().SingleOrDefault(x => x.Login == model.Login && x.Password == model.Password);

            // return null if user not found
            if (user == null) return null;

            // authentication successful so generate jwt token
            var token = generateJwtToken(user);

            return new AuthenticateResponse(user, token);
        }

        public IEnumerable<Entities.User> GetAll()
        {
            return userBaseReturn.GetUsersToLogin();
        }

        public Entities.User GetById(int id)
        {
            return userBaseReturn.GetUsersToLogin().FirstOrDefault(x => x.Id == id);
        }

        // helper methods

        private string generateJwtToken(Entities.User user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}

