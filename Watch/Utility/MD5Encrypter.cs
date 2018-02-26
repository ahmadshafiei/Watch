using System.Security.Cryptography;
using System.Text;

namespace Watch.Utility
{
    public static class MD5Encrypter
    {
        public static string GenerateMd5HashCode(string input)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            md5.ComputeHash(Encoding.ASCII.GetBytes(input));

            byte[] hash = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
                strBuilder.Append(hash[i].ToString("x2"));

            return strBuilder.ToString();
        }

    }
}