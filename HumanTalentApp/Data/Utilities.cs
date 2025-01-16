using System.Security.Cryptography;
using System.Text;

namespace HumanTalentApp.Data
{
    public class Utilities
    {
        public static string EncryptPassword(string password)
        {

            StringBuilder stringBuilder = new StringBuilder();
            using (SHA256 hash = SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(password));
                foreach (byte b in result)
                {
                    stringBuilder.Append(b.ToString("x2"));
                }
                return stringBuilder.ToString();
            }
        }
    }
}
