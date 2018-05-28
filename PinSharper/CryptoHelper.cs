using System;
using System.Security.Cryptography;
using System.Text;

namespace PinSharper
{
    internal static class CryptoHelper
    {
        public static string GenerateHMAC(string key, string data)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            return BitConverter.ToString(new HMACSHA256(keyBytes).ComputeHash(dataBytes)).Replace("-", "").ToLower();
        }

        public static string GenerateMD5(string data)
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            return BitConverter.ToString(MD5.Create().ComputeHash(dataBytes)).Replace("-", "").ToLower();
        }
    }
}
