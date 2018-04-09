using System;
using System.Linq;

namespace ImageCollections.WebApi.Repositories.FileSystemStorage
{
    public class PathGenerator : IPathGenerator
    {
        public FilePathInfo Generate(string fileName, int chunkSize)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return null;

            var directory = GenerateDirectory(fileName, chunkSize);

            return new FilePathInfo
            {
                DirectoryPath = directory,
                Name = fileName
            };
        }

        private string GenerateDirectory(string str, int chunkSize)
        {
            var chunkCount = (int)Math.Ceiling(str.Length / (double)chunkSize);
            var strChunks = Enumerable.Range(0, chunkCount)
                .Select(i => str.Substring(i * chunkSize, Math.Min(str.Length - i * chunkSize, chunkSize)))
                .ToArray();

            return string.Join('/', strChunks);
        }
    }
}