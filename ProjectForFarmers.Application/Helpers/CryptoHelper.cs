using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProjectForFarmers.Application.Helpers
{
    public static class CryptoHelper
    {
        public static string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(rawData);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);

                StringBuilder builder = new StringBuilder();

                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }

        public static string GenerateCacheKey<T>(T obj)
        {
            var typeName = typeof(T).FullName;
            var serializedFilter = JsonSerializer.Serialize(obj);

            ulong hash = CityHash.CityHash.CityHash64(serializedFilter+typeName);

            return hash.ToString("X16"); 
        }

    }

}
