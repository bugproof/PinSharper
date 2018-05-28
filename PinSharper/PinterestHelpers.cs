using System;

namespace PinSharper
{
    public static class PinterestHelpers
    {
        public static string GenerateSignature(string method, string requestUrl, string data)
        {
            string message = $"{method}&{Uri.EscapeDataString(requestUrl)}&{data}";
            return CryptoHelper.GenerateHMAC(PinterestConstants.CLIENT_SECRET, message);
        }

        public static string GenerateTimestamp()
        {
            return DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
        }

        public static string GenerateInstallId()
        {
            return CryptoHelper.GenerateMD5(new Random().Next(0, 0xffffff).ToString()).Substring(0, 31);
        }
    }
}
