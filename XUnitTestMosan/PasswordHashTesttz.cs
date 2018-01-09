using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Mosan;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Xunit;

namespace XUnitTestMosan
{
    public class PasswordHashTesttz
    {
        [Fact]
        public void PassTestPass()
        {
            var email = "g@mail.com";
            var password = PassHasher("12356");

            var email2 = "g@mail.com";
            var password2 = PassHasher("12356");

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

            var actual = user.Password;
            var expected = user2.Password;

            //pass
          //  Assert.Matches(user.Email, user2.Email);
            Assert.Matches(user.Password, user2.Password);

         
        }

        [Fact]
        public void PassTestFail()
        {
            var email = "g@mail.com";
            var password = PassHasher("1235");

            var email2 = "g@mail.com";
            var password2 = PassHasher("12356");

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

            var actual = user.Password;
            var expected = user2.Password;

            //pass
           // Assert.Matches(user.Email, user2.Email);
            Assert.Matches(user.Password, user2.Password);


        }

        public string PassHasher(string password)
        {
            // generate a 128-bit salt using a secure PRNG
            byte[] salt = new byte[128 / 8];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(

                password:password,
                salt: salt,

                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8)

                );

            return hashed;
           // return Convert.ToBase64String(salt);

        }

    }
}
