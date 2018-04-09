using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImageCollections.WebApi.Configuration;
using Microsoft.Extensions.Options;
using Serilog;

namespace ImageCollections.WebApi.Repositories.FileSystemStorage
{
    public class FileSystemStorage : IFileSystemStorage
    {
        private readonly IPathGenerator _pathGenerator;
        private readonly string _rootPath;
        private readonly int _chankSize;
        private const string RootPathKey = "Path";
        private const string ChankSizeKey = "ChunkSize";
        private const int DefaultChankSize = 8;

        public FileSystemStorage(IPathGenerator pathGenerator, IOptions<FileRepositorySetting> fileRepositorySettings)
        {
            _pathGenerator = pathGenerator;
            _rootPath = GetSettingValue(fileRepositorySettings, RootPathKey);
            var chankSizeSetting = GetSettingValue(fileRepositorySettings, ChankSizeKey);
            _chankSize = string.IsNullOrWhiteSpace(chankSizeSetting)
                ? DefaultChankSize
                : int.Parse(chankSizeSetting);
        }

        public async Task<byte[]> Get(string path)
        {
            if (!File.Exists(path))
                return null;

            var fileContent = await File.ReadAllBytesAsync(path);
            return fileContent;
        }

        public async Task<string> Save(byte[] content, string contentHash)
        {
            var pathInfo = _pathGenerator.Generate(contentHash, _chankSize);
            if (pathInfo == null)
                throw new Exception("Unable to generate path");

            var path = GetPath(pathInfo.DirectoryPath, pathInfo.Name);
            
            Log.Debug("Writing file: {file} ", path);
            var directoryPath = GetPath(pathInfo.DirectoryPath);
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            await File.WriteAllBytesAsync(path, content);
            Log.Information("File saved to storage: {file}", path);
            return path;
        }

        private string GetPath(string directory, string fileName)
        {
            return $"{GetPath(directory)}/{fileName}";
        }

        private string GetPath(string directory)
        {
            return $"{_rootPath}/{directory}";
        }

        private string GetSettingValue(IOptions<FileRepositorySetting> fileStorageSetting, string key)
        {
            var setting = fileStorageSetting.Value.Storages.FirstOrDefault(x => x.Type == StorageType.FileSystem);

            return setting != null && setting.Args.ContainsKey(key)
                ? setting.Args[key]
                : string.Empty;
        }
    }
}