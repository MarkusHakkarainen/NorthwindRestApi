//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using NorthwindRestApi.Models;

//namespace NorthwindRestApi.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class UsersController : ControllerBase
//    {
//        //NorthwindOriginalContext db = new NorthwindOriginalContext();
//        private NorthwindOriginalContext db;

//        public UsersController(NorthwindOriginalContext dbparametri)
//        {
//            db = dbparametri;
//        }

//        [HttpPost]

//        public ActionResult PostCreateNew([FromBody] User u)
//        {
//            try
//            {
//                db.Users.Add(u);
//                db.SaveChanges();
//                return Ok("Lisättiin käyttäjä " + u.Username);
//            }
//            catch (Exception e)
//            {
//                return BadRequest("Lisääminen ei onnistunut. Tässä lisätietoa: " + e);
//            }
//        }
//    }
//}








//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using NorthwindRestApi.Models;
//using System;

//using System.Collections.Generic;
//using System.Linq;

//namespace NorthwindRestApi.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class UsersController : ControllerBase
//    {
//        private NorthwindOriginalContext db;

//        public UsersController(NorthwindOriginalContext dbparametri)
//        {
//            db = dbparametri;
//        }

//        // POST: api/users
//        [HttpPost]
//        public ActionResult PostCreateNew([FromBody] User u)
//        {
//            try
//            {
//                db.Users.Add(u);
//                db.SaveChanges();
//                return Ok("Lisättiin käyttäjä " + u.Username);
//            }
//            catch (Exception e)
//            {
//                return BadRequest("Lisääminen ei onnistunut. Tässä lisätietoa: " + e);
//            }
//        }

//        // GET: api/users
//        [HttpGet]
//        public ActionResult<IEnumerable<User>> GetAllUsers()
//        {
//            try
//            {
//                var users = db.Users.ToList();
//                return Ok(users); // Return the list of users as JSON
//            }
//            catch (Exception e)
//            {
//                return BadRequest("Virhe käyttäjien haussa: " + e);
//            }
//        }
//    }
//}







using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NorthwindRestApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NorthwindRestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private NorthwindOriginalContext db;

        public UsersController(NorthwindOriginalContext dbparametri)
        {
            db = dbparametri;
        }

        // POST: api/users
        [HttpPost]
        public ActionResult PostCreateNew([FromBody] User u)
        {
            try
            {
                // Add new user to the database
                db.Users.Add(u);
                db.SaveChanges();
                return Ok("Lisättiin käyttäjä " + u.Username);
            }
            catch (Exception e)
            {
                return BadRequest("Lisääminen ei onnistunut. Tässä lisätietoa: " + e);
            }
        }

        // GET: api/users
        [HttpGet]
        public ActionResult<IEnumerable<User>> GetAllUsers()
        {
            try
            {
                var users = db.Users.ToList();
                return Ok(users); // Return the list of users as JSON
            }
            catch (Exception e)
            {
                return BadRequest("Virhe käyttäjien haussa: " + e);
            }
        }

        // PUT: api/users/{id}
        [HttpPut("{id}")]
        public ActionResult UpdateUser(int id, [FromBody] User updatedUser)
        {
            try
            {
                // Find the existing user by id
                var existingUser = db.Users.FirstOrDefault(u => u.UserId == id);

                if (existingUser == null)
                {
                    return NotFound($"Käyttäjää ei löytynyt id:llä {id}");
                }

                // Update the fields (make sure to keep the original fields intact)
                existingUser.Firstname = updatedUser.Firstname;
                existingUser.Lastname = updatedUser.Lastname;
                existingUser.Email = updatedUser.Email;
                existingUser.AccesslevelId = updatedUser.AccesslevelId;
                existingUser.Username = updatedUser.Username;

                // Only update the password if it's provided
                if (!string.IsNullOrEmpty(updatedUser.Password))
                {
                    existingUser.Password = updatedUser.Password;
                }

                // Save changes to the database
                db.SaveChanges();
                return Ok($"Käyttäjä {existingUser.Username} päivitettiin.");
            }
            catch (Exception e)
            {
                return BadRequest("Päivitys ei onnistunut. Tässä lisätietoa: " + e);
            }
        }
    }
}















//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using NorthwindRestApi.Models;
//using System;
//using System.Linq;
//using System.Security.Claims;

//namespace NorthwindRestApi.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class UsersController : ControllerBase
//    {
//        private readonly NorthwindOriginalContext db;

//        public UsersController(NorthwindOriginalContext dbparametri)
//        {
//            db = dbparametri;
//        }

//        // 1. Käyttäjän luominen
//        [HttpPost]
//        public ActionResult PostCreateNew([FromBody] User u)
//        {
//            try
//            {
//                if (string.IsNullOrWhiteSpace(u.Username) || string.IsNullOrWhiteSpace(u.Password))
//                {
//                    return BadRequest("Käyttäjänimi ja salasana ovat pakollisia.");
//                }

//                if (db.Users.Any(user => user.Username == u.Username))
//                {
//                    return Conflict($"Käyttäjänimi '{u.Username}' on jo käytössä.");
//                }

//                db.Users.Add(u);
//                db.SaveChanges();

//                return Ok($"Lisättiin käyttäjä {u.Username}.");
//            }
//            catch (Exception e)
//            {
//                return BadRequest("Lisääminen ei onnistunut. Virhe: " + e.Message);
//            }
//        }

//        // 2. Kaikkien käyttäjien haku (vain accesslevelId = 1)
//        [HttpGet]
//        [Authorize(AuthenticationSchemes = "Bearer")]
//        public ActionResult GetUsers()
//        {
//            try
//            {
//                if (GetUserAccessLevelId() != 1)
//                {
//                    return Unauthorized("Ei riittäviä oikeuksia nähdä käyttäjätietoja.");
//                }

//                return Ok(db.Users.ToList());
//            }
//            catch (Exception e)
//            {
//                return BadRequest("Virhe käyttäjien hakemisessa: " + e.Message);
//            }
//        }

//        // 3. Käyttäjän haku ID:llä (vain accesslevelId = 1)
//        [HttpGet("{id}")]
//        [Authorize(AuthenticationSchemes = "Bearer")]
//        public ActionResult GetUserById(int id)
//        {
//            try
//            {
//                if (GetUserAccessLevelId() != 1)
//                {
//                    return Unauthorized("Ei riittäviä oikeuksia nähdä käyttäjätietoja.");
//                }

//                var user = db.Users.Find(id);
//                if (user == null)
//                {
//                    return NotFound($"Käyttäjää ID:llä {id} ei löytynyt.");
//                }

//                return Ok(user);
//            }
//            catch (Exception e)
//            {
//                return BadRequest("Virhe käyttäjän hakemisessa: " + e.Message);
//            }
//        }

//        // 4. Käyttäjän muokkaus (vain accesslevelId = 1)
//        [HttpPut("{id}")]
//        [Authorize(AuthenticationSchemes = "Bearer")]
//        public ActionResult PutUpdateUser(int id, [FromBody] User updatedUser)
//        {
//            try
//            {
//                if (GetUserAccessLevelId() != 1)
//                {
//                    return Unauthorized("Sinulla ei ole oikeuksia muokata käyttäjiä.");
//                }

//                var user = db.Users.Find(id);
//                if (user == null)
//                {
//                    return NotFound("Käyttäjää ei löytynyt.");
//                }

//                user.Username = updatedUser.Username ?? user.Username;
//                user.Password = updatedUser.Password ?? user.Password;
//                user.AccesslevelId = updatedUser.AccesslevelId;
//                user.Email = updatedUser.Email ?? user.Email;

//                db.SaveChanges();
//                return Ok($"Käyttäjä {user.Username} on päivitetty.");
//            }
//            catch (Exception e)
//            {
//                return BadRequest("Muokkaaminen ei onnistunut. Virhe: " + e.Message);
//            }
//        }

//        // 5. Käyttäjän poisto (vain accesslevelId = 1)
//        [HttpDelete("{id}")]
//        [Authorize(AuthenticationSchemes = "Bearer")]
//        public ActionResult DeleteUser(int id)
//        {
//            try
//            {
//                if (GetUserAccessLevelId() != 1)
//                {
//                    return Unauthorized("Sinulla ei ole oikeuksia poistaa käyttäjiä.");
//                }

//                var user = db.Users.Find(id);
//                if (user == null)
//                {
//                    return NotFound("Käyttäjää ei löytynyt.");
//                }

//                db.Users.Remove(user);
//                db.SaveChanges();
//                return Ok($"Käyttäjä {user.Username} on poistettu.");
//            }
//            catch (Exception e)
//            {
//                return BadRequest("Poistaminen ei onnistunut. Virhe: " + e.Message);
//            }
//        }

//        // 🔐 Apumetodi AccessLevelId:n tarkistamiseen käyttäjän tiedoista
//        private int GetUserAccessLevelId()
//        {
//            // Hae käyttäjän käyttäjätunnus (username) tokenista
//            var usernameClaim = User.FindFirst(ClaimTypes.Name);
//            if (usernameClaim == null)
//                return -1; // Palautetaan -1, jos käyttäjänimi puuttuu tokenista

//            // Etsi käyttäjä tietokannasta käyttäjätunnuksen avulla
//            var user = db.Users.FirstOrDefault(u => u.Username == usernameClaim.Value);
//            if (user == null)
//                return -1; // Palautetaan -1, jos käyttäjää ei löydy

//            return user.AccesslevelId; // Palautetaan käyttäjän accesslevelId
//        }
//    }
//}








