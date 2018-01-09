using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Mosan;

namespace MosqueService.Controllers
{
    /// <summary>
    /// creates Type Jwt 
    /// </summary>
    public class JwtPacket
    {
        public string Token { get; set; }
        public string FirstName { get; set; }
    }

    public class LoginData
    {
        [Required]
        public string Password { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }

    [Produces("application/json")]
    [Route("api/auth")]
    public class AuthController : Controller
    {
        //[HttpGet]
        //public IActionResult Get()
        //{
        //    return Ok("welcome to auth");
        //}

        //initializes database
        readonly ApiContext db;
        public AuthController(ApiContext db)
        {
            this.db = db;
        }

        // [HttpPost("login")]//Tested Hash passwords b4 store
        [HttpPost]
        public ActionResult Login([FromBody]LoginData loginData)
        {
            if (!ModelState.IsValid)
            {
                return NotFound("Wtf is going on !!!! ");
            }

            var user = new User();

            user.Id = "100"; ///!!!ID is for test needed the user Id sqlite auto generates  it  but  
            user.Email = loginData.Email;
            user.Password = loginData.Password;
            user.FirstName = "SuperGuyTestName";
            user.LastName = "TestLastName";
            var x = CreateJwtPacket(user);

            //user = db.Users.SingleOrDefault(x => x.Email == loginData.Email
            //&& x.Password == loginData.Password);
            //if (user == null)
            //{
            //    return NotFound("Email or password incorrect");
            //}
            //return Ok(CreateJwtPacket(user));

            return Ok(x);
        }

        [HttpPost("register")]
        public ActionResult Register([FromBody]User user)

        {
            if (ModelState.IsValid)
            {
               // var userNew = db.Users.Add(user);
              //  db.SaveChangesAsync();

                return Ok(CreateJwtPacket(user));
            }

            return NotFound("Something is wrong reg");

        }


        public JwtPacket CreateJwtPacket(User user)
        {
            //made is startup class
            var signInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is the secret phrase"));
            var signingCredentials = new SigningCredentials(signInKey, SecurityAlgorithms.HmacSha256);

            var claims = new Claim[]
            {
              new Claim(JwtRegisteredClaimNames.Sub,user.Id)

            };

            //pass in claim and signin creds
            var jwt = new JwtSecurityToken(
                claims: claims,
                signingCredentials: signingCredentials
                );


            var encodedJwt = new JwtSecurityTokenHandler()
                .WriteToken(jwt);

            return new JwtPacket() { Token = encodedJwt, FirstName = user.FirstName };

        }

    }
}