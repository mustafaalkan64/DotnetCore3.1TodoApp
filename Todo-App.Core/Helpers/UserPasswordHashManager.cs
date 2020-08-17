using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ToDo_App.Core.Helpers
{
    public static class UserPasswordHashManager
    {
        /// <summary>
        /// Create Salt For Password Hashing With 
        /// </summary>
        /// <param name="size">Salt Size</param>
        /// <returns></returns>
        public static string CreateSalt(int size)
        {
            //Generate a cryptographic random number.
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[size];
            rng.GetBytes(buff);
            return Convert.ToBase64String(buff);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input">Password Value</param>
        /// <param name="salt">Salt Parameter</param>
        /// <returns></returns>
        public static string GenerateHash(string input, string salt)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input + salt);
            SHA256Managed sHA256ManagedString = new SHA256Managed();
            byte[] hash = sHA256ManagedString.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plainTextInput">Password</param>
        /// <param name="hashedInput">Hash Value</param>
        /// <param name="salt">Salt Value</param>
        /// <returns></returns>
        public static bool AreEqual(string plainTextInput, string hashedInput, string salt)
        {
            string newHashedPin = GenerateHash(plainTextInput, salt);
            return newHashedPin.Equals(hashedInput);
        }
    }
}
