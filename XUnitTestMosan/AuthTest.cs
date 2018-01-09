using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;
using Mosan;
using MosqueService.Controllers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Xunit;

namespace XUnitTestMosan
{
    public class AuthTest
    {
        [Fact]
        public void LoginTestPass()
        {
            var email = "g@mail.com";
            var password = "12356";

            var email2 = "g@mail.com";
            var password2 = "12356";

            User user = new User();
            user.Id = "100";
            user.FirstName = "foo";
            user.LastName = "bar";
            user.Email = email;
            user.Password = password;

            User user2 = new User();
            user2.Id = "100";
            user2.FirstName = "foo";
            user2.LastName = "bar";
            user2.Email = email2;
            user2.Password = password2;

            var actual = CreateJwtPacket(user);
            var expected = CreateJwtPacket(user2);

            //user
            Assert.Matches(user.Email,user2.Email);
            Assert.Matches(user.Password, user2.Password);

            //jwt
            Assert.Matches(expected.FirstName, actual.FirstName);
            Assert.Matches(expected.Token, actual.Token);
        }

        [Fact]
        public void LoginTestFail()
        {
            var email = "g@mail.com";
            var password = "1235";//-------------missing 6 doesnt matter

            var email2 = "g@mail.com";
            var password2 = "12356";

            User user = new User();
            user.Id = "100";//-------------------this is what generates the key
            user.FirstName = "foo";
            user.LastName = "bar";
            user.Email = email;
            user.Password = password;

            User user2 = new User();
            user2.Id = "101";//-----------101 != 100--------this is what generates wrong key
            user2.FirstName = "foo";
            user2.LastName = "bar";
            user2.Email = email2;
            user2.Password = password2;

            var actual = CreateJwtPacket(user);
            var expected = CreateJwtPacket(user2);

            //user
            Assert.Matches(user.Email, user2.Email);
            Assert.Matches(user.Password, user2.Password);

            //jwt
            Assert.Matches(expected.FirstName, actual.FirstName);
            Assert.Matches(expected.Token, actual.Token);

        }
        //user = db.Users.SingleOrDefault(x => x.Email == loginData.Email
        //&& x.Password == loginData.Password);
        //if (user == null)
        //{
        //    return NotFound("Email or password incorrect");
        //}
        //return Ok(CreateJwtPacket(user));


        //regular 
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

        //hashes and randomizes the sskey -heavy cpu - returns false for all tests
        public JwtPacket CreateJwtPacketGuidCollisionKILLA(User user)
        {
            // generate a 128-bit salt using a secure PRNG
            byte[] salt = new byte[128 / 8];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
          
            //Console.WriteLine($"Salt: {Convert.ToBase64String(salt)}");
           
            // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(

                password: user.Password,
                salt: salt,

                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8)

                );
            //Console.WriteLine($"Hashed: {hashed}");


            //made is startup class
            var signInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(hashed));
            //var signInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is the secret phrase"));
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




