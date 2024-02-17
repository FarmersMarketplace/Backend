using ProjectForFarmers.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProjectForFarmers.Application.Helpers
{
    public static class CacheHelper
    {
        public static string EncryptObject<T>(T obj)
        {
            var typeName = typeof(T).FullName;
            var serializedFilter = JsonSerializer.Serialize(obj);

            ulong hash = CityHash.CityHash.CityHash64(serializedFilter + typeName);

            return hash.ToString("X16");
        }

        public static string GenerateCacheKey<T>(Guid? id = null, Producer? producer = null, string? name = null, T obj = default)
        {
            var parts = new List<string>();

            if(id != null)
                parts.Add(id.ToString());
            if(producer != null)
                parts.Add(producer.ToString());
            if (name != null)
                parts.Add(name);
            if (!EqualityComparer<T>.Default.Equals(obj, default(T)))
                parts.Add(EncryptObject(obj));

            return string.Join("-", parts);
        }

        public static string GenerateCacheKey(Guid? id = null, Producer? producer = null, string? name = null)
        {
            var parts = new List<string>();

            if (id != null)
                parts.Add(id.ToString());
            if (producer != null)
                parts.Add(producer.ToString());
            if (name != null)
                parts.Add(name);

            return string.Join("-", parts);
        }
    }

}
