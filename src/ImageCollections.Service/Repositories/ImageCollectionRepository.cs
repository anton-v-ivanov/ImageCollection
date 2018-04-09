using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Dapper;
using ImageCollections.Contracts.ImageCollections;
using ImageCollections.Contracts.ImageInfos;
using ImageCollections.Service.Configuration;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;

namespace ImageCollections.Service.Repositories
{
    public class ImageCollectionRepository : IImageCollectionRepository
    {
        private readonly string _sqliteConnectionString;

        public ImageCollectionRepository(IOptions<ConnectionStrings> connectionStringsOptions)
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder
            {
                DataSource = connectionStringsOptions.Value.SqliteFile
            };
            _sqliteConnectionString = connectionStringBuilder.ConnectionString;
            if (!File.Exists(connectionStringsOptions.Value.SqliteFile))
            {
                CreateDb(connectionStringsOptions.Value.SqliteFile);
            }
        }

        private void CreateDb(string dbFile)
        {
            var fs = File.Create(dbFile);
            fs.Close();
            using (var connection = new SqliteConnection(_sqliteConnectionString))
            {
                connection.Open();
                connection.Execute("CREATE TABLE \"Collections\" ( `Id` INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, `Name` TEXT NOT NULL )");
                connection.Execute("CREATE TABLE \"Images\" ( `Id` INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, `Name` TEXT, `FilePath` TEXT NOT NULL, `ContentType` TEXT NOT NULL, `Height` INTEGER, `Width` INTEGER )");
                connection.Execute("CREATE TABLE \"Images2Collections\" ( `CollectionId` INTEGER NOT NULL, `ImageId` INTEGER NOT NULL, FOREIGN KEY(`CollectionId`) REFERENCES `Collections`(`Id`), FOREIGN KEY(`ImageId`) REFERENCES `Images`(`Id`) )");
            }
        }

        public async Task<IEnumerable<ImageCollection>> Search(string name, int fetch, int offset)
        {
            using (var connection = new SqliteConnection(_sqliteConnectionString))
            {
                connection.Open();
                return await connection.QueryAsync<ImageCollection>("select * from Collections where (Name = @name or @name is null) limit @fetch offset @offset", new { name, fetch, offset });
            }
        }

        public async Task<ImageCollection> GetCollection(long id)
        {
            using (var connection = new SqliteConnection(_sqliteConnectionString))
            {
                connection.Open();
                return await connection.QueryFirstOrDefaultAsync<ImageCollection>("select * from Collections where Id = @id", new { id });
            }
        }

        public async Task<ImageCollection> Create(string name)
        {
            using (var connection = new SqliteConnection(_sqliteConnectionString))
            {
                connection.Open();
                var id = await connection.ExecuteScalarAsync<long>("insert into Collections (Name) values (@name); select last_insert_rowid();", new { name });
                return new ImageCollection
                {
                    Id = id,
                    Name = name
                };
            }
        }

        public async Task<ImageCollection> Update(long id, string name)
        {
            using (var connection = new SqliteConnection(_sqliteConnectionString))
            {
                connection.Open();
                await connection.ExecuteAsync("update Collections set Name = @name where Id = @id", new { name, id });
                return new ImageCollection
                {
                    Id = id,
                    Name = name
                };
            }
        }

        public async Task<CollectionDeleteResponse> Delete(long id)
        {
            using (var connection = new SqliteConnection(_sqliteConnectionString))
            {
                connection.Open();
                await connection.ExecuteAsync("delete from Collections where Id = @id", new { id });
                return new CollectionDeleteResponse();
            }
        }

        public async Task<ImageInfo> UploadImage(string name, string path, string contentType, int height, int width)
        {
            using (var connection = new SqliteConnection(_sqliteConnectionString))
            {
                connection.Open();
                var id = await connection.ExecuteScalarAsync<long>("insert into Images (Name, FilePath, ContentType, Height, Width) values (@name, @path, @contentType, @height, @width); select last_insert_rowid();", 
                    new { name, path, contentType, height, width });
                return new ImageInfo
                {
                    Id = id,
                    Name = name,
                    FilePath = path,
                    ContentType = contentType,
                    Height = height,
                    Width = width
                };
            }
        }

        public async Task<ImageInfo> GetImage(long id)
        {
            using (var connection = new SqliteConnection(_sqliteConnectionString))
            {
                connection.Open();
                return await connection.QueryFirstOrDefaultAsync<ImageInfo>("select * from Images where Id = @id", new { id });
            }
        }

        public async Task<IEnumerable<ImageInfo>> GetImageList(string name, int fetch, int offset)
        {
            using (var connection = new SqliteConnection(_sqliteConnectionString))
            {
                connection.Open();
                return await connection.QueryAsync<ImageInfo>("select * from Images where (Name = @name or @name is null) limit @fetch offset @offset", new { name, fetch, offset });
            }
        }
    }
}