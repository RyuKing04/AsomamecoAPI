using System.Security.Cryptography;
using System.Text;

namespace AsomamecoAPI.Service
{
    public class Utils
    {
        public static string Encrypt (string contraseña)
        {
            StringBuilder sb = new StringBuilder();
            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(contraseña));
                foreach (Byte b in result)
                    sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }
    
    }
}
