using System.Security.Cryptography;
using System.Text;

namespace ImageCollections.WebApi.Managers.HashGenerator
{
    public class HashGenerator : IHashGenerator
    {
        private readonly SHA256Managed _sha256Managed;

        public HashGenerator()
        {
            _sha256Managed = new SHA256Managed();
        }

        public byte[] Generate(string text)
        {
            return _sha256Managed.ComputeHash(Encoding.ASCII.GetBytes(text), 0, Encoding.ASCII.GetByteCount(text));
        }

        public byte[] Generate(byte[] content)
        {
            return _sha256Managed.ComputeHash(content);
        }
    }
}