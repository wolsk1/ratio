namespace VolskSoft.Alfa
{
    using System;
    using System.Text;

    public class CryptoUtils
    {
        /// <summary>
        /// Encrypts the specified clear text.
        /// </summary>
        /// <param name="clearText">The clear text.</param>
        /// <param name="password">The password.</param>
        /// <returns>Returns encrypted string.</returns>
        public static string Encrypt(string clearText, string password)
        {
            if (String.IsNullOrEmpty(clearText))
            {
                throw new ArgumentNullException("clearText");
            }

            if (String.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException("password");
            }

            StringBuilder rezult = new StringBuilder();

            int index = 0;
            foreach (char c in clearText)
            {
                rezult.Append((char)(c - password[index]));

                index++;
                if (index >= password.Length)
                {
                    index = 0;
                }
            }

            return rezult.ToString();
        }

        /// <summary>
        /// Decrypt a string into a string using a password
        /// </summary>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="password">The password.</param>
        /// <returns>Returns decrypted stirng.</returns>
        public static string Decrypt(string cipherText, string password)
        {
            if (String.IsNullOrEmpty(cipherText))
            {
                throw new ArgumentNullException("cipherText");
            }

            if (String.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException("password");
            }

            StringBuilder rezult = new StringBuilder();

            int index = 0;
            foreach (char c in cipherText)
            {
                rezult.Append((char)(c + password[index]));

                index++;
                if (index >= password.Length)
                {
                    index = 0;
                }
            }

            return rezult.ToString();
        }
    }
}
