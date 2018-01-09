using Microsoft.IdentityModel.Tokens;
using Mosan;
using MosqueService.Controllers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Xunit;

namespace XUnitTestMosan
{
    public class LoginTestBoyy
    {
        //this test is for the jwt packet

        [Fact]
        public void LoginTestPass()
        {
            var email = "g@mail.com";
            var password = "12356";

            var email2 = "g@mail.com";
            var password2 = "12356";

            User user = new User();
            user.Id = "100"; //-------------This is the number the jwt token turns to Id 
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

            Assert.Matches(expected.Token, actual.Token);
        }


        [Fact]
        public void LoginTestFail()
        {
            var email = "g@mail.com";
            var password = "12356";

            var email2 = "g@mail.com";
            var password2 = "12356";

            User user = new User();
            user.Id = "101111"; //-------------This is the number the jwt token turns to Id 
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

            Assert.Matches(expected.Token, actual.Token);
        }






        //jwt method
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
