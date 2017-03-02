using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KrautsynchBot.classes
{
    class clsChatMessage
    {
        public DateTime timestamp;
        public string username;
        public string message;
        public string rawHtml;

        public override string ToString()
        {
            return string.Format("{0} {1}: {2}", timestamp.ToShortTimeString(), username, message);
        }
        public string GetMD5HashCode()
        {
            return CalculateMD5Hash(rawHtml);
        }


        private string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }

            return sb.ToString();
        }
    }
}
