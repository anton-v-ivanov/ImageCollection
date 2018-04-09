using System;
using System.Linq;
using System.Text;

namespace ImageCollections.WebApi.Managers
{
    public static class HashHelper
    {
        public static string ToHashString(this byte[] byteArray)
        {
            if (byteArray == null || !byteArray.Any())
                return string.Empty;

            var sBuilder = new StringBuilder();
            foreach (var t in byteArray)
            {
                sBuilder.Append(t.ToString("x2"));
            }
            return sBuilder.ToString();
        }

        public static byte[] FromHashString(this string hashString)
        {
            if (string.IsNullOrWhiteSpace(hashString))
                return null;

            return Enumerable.Range(0, hashString.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(hashString.Substring(x, 2), 16))
                .ToArray();
        }
    }
}