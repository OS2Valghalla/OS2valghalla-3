using System.Security.Cryptography;
using System.Text;

namespace Valghalla.Tools.Utils
{
    public static class CertificateUtils
    {
        public static byte[] ConvertToByteArray(string filepath)
        {
            var certBytes = File.ReadAllBytes(filepath);

            return certBytes;
        }

        public static byte[] ConvertToBytes(Stream stream)
        {
            stream.Position = 0;

            using var bufferedStream = new BufferedStream(stream);
            using var memoryStream = new MemoryStream();
            bufferedStream.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }

        public static string ComputeSha256Hash(Stream stream)
        {
            stream.Position = 0;

            using var sha256Hash = SHA256.Create();
            var bytes = sha256Hash.ComputeHash(stream);

            var builder = new StringBuilder();

            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }

            return builder.ToString();
        }
    }
}
