using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GraphAlgorithms.Shared
{
    public static class GuidCombiner
    {
        public static Guid GenerateCombinedGuid(Guid guid, int additionalData)
        {
            // Combine the graph GUID and the node ID into a single string
            string combined = $"{guid}_{additionalData}";

            // Use SHA-256 to hash the combined string
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(combined));

                // Use the first 16 bytes of the hash to create a new GUID
                byte[] guidBytes = new byte[16];
                Array.Copy(hashBytes, guidBytes, 16);

                return new Guid(guidBytes);
            }
        }
    }
}
