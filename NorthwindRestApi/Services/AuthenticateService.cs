using Microsoft.Extensions.Options;
using NorthwindRestApi.Controllers;
using NorthwindRestApi.Models;
using NorthwindRestApi.Services.Interfaces;
using System;
using System.Text;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography.Xml;

namespace NorthwindRestApi.Services
{
    public class AuthenticateService : IAuthenticateService
    {
        private readonly NorthwindOriginalContext db;

        private readonly AppSettings _appSettings;
        public AuthenticateService(IOptions<AppSettings> appSettings, NorthwindOriginalContext nwc)
        {
            _appSettings = appSettings.Value;
            db = nwc;
        }

        //private NorthwindOriginalContext db = new NorthwindOriginalContext();

        public LoggedUser? Authenticate(string username, string password)
        {
            var foundUser = db.Users.SingleOrDefault(x => x.Username == username && x.Password == password);

            if (foundUser == null)
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, foundUser.UserId.ToString()),
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim("AccessLevel", foundUser.AccesslevelId.ToString()), // 🔑 Tämä mahdollistaa access-tarkistukset controllerissa
                new Claim(ClaimTypes.Version, "v3.1")
            }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            var loggedUser = new LoggedUser
            {
                Username = foundUser.Username,
                AccesslevelId = foundUser.AccesslevelId,
                Token = tokenHandler.WriteToken(token)
            };

            //loggedUser.Username = foundUser.Username;
            //loggedUser.AcceslevelId = foundUser.AcceslevelId;
            //loggedUser.Token = tokenHandler.WriteToken(token);

            return loggedUser;
        }
    }
}






//using Microsoft.Extensions.Options;
//using NorthwindRestApi.Models;
//using NorthwindRestApi.Services.Interfaces;
//using System;
//using System.Linq;
//using System.Text;
//using System.Security.Claims;
//using Microsoft.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
//using NorthwindRestApi.Controllers;

//namespace NorthwindRestApi.Services
//{
//    public class AuthenticateService : IAuthenticateService
//    {
//        private readonly NorthwindOriginalContext db;
//        private readonly AppSettings _appSettings;

//        public AuthenticateService(IOptions<AppSettings> appSettings, NorthwindOriginalContext nwc)
//        {
//            _appSettings = appSettings.Value;
//            db = nwc;
//        }

//        public LoggedUser? Authenticate(string username, string password)
//        {
//            // Etsitään käyttäjä tietokannasta käyttäjätunnuksella ja salasanalla
//            var foundUser = db.Users.SingleOrDefault(x => x.Username == username && x.Password == password);

//            if (foundUser == null)
//            {
//                return null; // Jos käyttäjää ei löydy, palautetaan null
//            }

//            // Luodaan JWT-token käyttäjälle
//            var tokenHandler = new JwtSecurityTokenHandler();
//            var key = Encoding.ASCII.GetBytes(_appSettings.Key); // Otetaan API:n salaustunnus
//            var tokenDescriptor = new SecurityTokenDescriptor
//            {
//                Subject = new ClaimsIdentity(new Claim[]
//                {
//                    new Claim(ClaimTypes.Name, foundUser.UserId.ToString()), // Käyttäjän ID Claim
//                    new Claim("AccessLevel", foundUser.AccesslevelId.ToString()) // Käyttäjän AccessLevel Claim
//                }),
//                Expires = DateTime.UtcNow.AddDays(1), // Tokenin voimassaoloaika on 1 päivä
//                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature) // Allekirjoitus
//            };

//            // Luodaan token
//            var token = tokenHandler.CreateToken(tokenDescriptor);

//            // Palautetaan kirjautunut käyttäjä ja token
//            var loggedUser = new LoggedUser
//            {
//                Username = foundUser.Username,
//                AccesslevelId = foundUser.AccesslevelId,
//                Token = tokenHandler.WriteToken(token) // Token muutetaan string-muotoon ja liitetään
//            };

//            return loggedUser; // Palautetaan kirjautunut käyttäjä
//        }
//    }
//}







