using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                connection.Execute("CREATE TABLE \"Images\" ( `Id` INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, `Name` TEXT, `FilePath` TEXT NOT NULL, `ContentType` TEXT NOT NULL, `Height` INTEGER, `Width` INTEGER, `XResolution` TEXT, `YResolution` TEXT, `DateTime` TEXT )");
                connection.Execute("CREATE TABLE \"Images2Collections\" ( `CollectionId` INTEGER NOT NULL, `ImageId` INTEGER NOT NULL, FOREIGN KEY(`ImageId`) REFERENCES `Images`(`Id`) ON DELETE CASCADE, FOREIGN KEY(`CollectionId`) REFERENCES `Collections`(`Id`) ON DELETE CASCADE )");
                connection.Execute("CREATE INDEX `NameSearchIndex` ON `Collections` (`Name` ASC);");
                connection.Execute("CREATE UNIQUE INDEX `UniqueRelationIndex` ON `Images2Collections` (`CollectionId`, `ImageId`);");
            }
        }

        public async Task<IEnumerable<ImageCollectionInternal>> GetCollectionsList(string name, int fetch, int offset)
        {
            IEnumerable<dynamic> results;
            using (var connection = new SqliteConnection(_sqliteConnectionString))
            {
                connection.Open();
                results = await connection.QueryAsync(@"select Collections.*, Images2Collections.ImageId from Collections 
left join Images2Collections on Collections.Id = Images2Collections.CollectionId 
where(Collections.Name = @name or @name is null) limit @fetch offset @offset", new { name, fetch, offset });
            }

            var collections = new List<ImageCollectionInternal>();
            foreach (var result in results)
            {
                var collection = collections.FirstOrDefault(s => s.Id.Equals(result.Id));
                if (collection == null)
                {
                    collection = new ImageCollectionInternal
                    {
                        Id = result.Id,
                        Name = result.Name,
                        Images = new List<long>
                        {
                            result.ImageId
                        }
                    };
                    collections.Add(collection);
                }
                else
                {
                    if(result.ImageId != null)
                        collection.Images.Add(result.ImageId);
                }
            }

            return collections;
        }

        public async Task<ImageCollectionInternal> GetCollection(long id)
        {
            IEnumerable<dynamic> result;
            using (var connection = new SqliteConnection(_sqliteConnectionString))
            {
                connection.Open();
                result = await connection.QueryAsync(@"select Collections.*, Images2Collections.ImageId from Collections 
left join Images2Collections on Collections.Id = Images2Collections.CollectionId 
where Id = @id", new { id });
            }

            if (result == null || !result.Any())
                return null;

            var collection = new ImageCollectionInternal
            {
                Id = result.First().Id,
                Name = result.First().Name,
                Images = new List<long>()
            };
            foreach (var row in result.Where(s => s.ImageId != null))
            {
                collection.Images.Add(row.ImageId);
            }

            return collection;
        }

        public async Task<ImageCollectionInternal> CreateCollection(string name)
        {
            using (var connection = new SqliteConnection(_sqliteConnectionString))
            {
                connection.Open();
                var id = await connection.ExecuteScalarAsync<long>("insert into Collections (Name) values (@name); select last_insert_rowid();", new { name });
                return new ImageCollectionInternal
                {
                    Id = id,
                    Name = name
                };
            }
        }

        public async Task UpdateCollection(long id, string name)
        {
            using (var connection = new SqliteConnection(_sqliteConnectionString))
            {
                connection.Open();
                await connection.ExecuteAsync("update Collections set Name = @name where Id = @id", new { name, id });
            }
        }

        public async Task DeleteCollection(long id)
        {
            using (var connection = new SqliteConnection(_sqliteConnectionString))
            {
                connection.Open();
                await connection.ExecuteAsync("delete from Collections where Id = @id", new { id });
            }
        }

        public async Task<ImageInfoInternal> UploadImage(string name, string path, string contentType, int height, int width, 
            string xResolution, string yResolution, string dateTime)
        {
            using (var connection = new SqliteConnection(_sqliteConnectionString))
            {
                connection.Open();
                var id = await connection.ExecuteScalarAsync<long>(@"insert into Images (Name, FilePath, ContentType, Height, Width, XResolution, YResolution, DateTime) 
values (@name, @path, @contentType, @height, @width, @xResolution, @yResolution, @dateTime);
select last_insert_rowid();", 
                    new { name, path, contentType, height, width, xResolution, yResolution, dateTime });

                return await GetImage(id);
            }
        }

        public async Task<ImageInfoInternal> GetImage(long id)
        {
            using (var connection = new SqliteConnection(_sqliteConnectionString))
            {
                connection.Open();
                return await connection.QueryFirstOrDefaultAsync<ImageInfoInternal>("select * from Images where Id = @id", new { id });
            }
        }

        public async Task<IEnumerable<ImageInfoInternal>> GetImageList(string name, int fetch, int offset)
        {
            using (var connection = new SqliteConnection(_sqliteConnectionString))
            {
                connection.Open();
                return await connection.QueryAsync<ImageInfoInternal>("select * from Images where (Name = @name or @name is null) limit @fetch offset @offset", new { name, fetch, offset });
            }
        }

        public async Task UpdateImageInfo(long id, string name)
        {
            using (var connection = new SqliteConnection(_sqliteConnectionString))
            {
                connection.Open();
                await connection.ExecuteAsync("update Images set Name = @name where Id = @id", new { name, id });
            }
        }

        public async Task AddImageToCollection(long collectionId, long imageId)
        {
            using (var connection = new SqliteConnection(_sqliteConnectionString))
            {
                connection.Open();
                await connection.ExecuteAsync(@"insert into Images2Collections 
select @collectionId, @imageId
where not exists (select 1 from Images2Collections where CollectionId = @collectionId and ImageId = @imageId)", 
                    new { collectionId, imageId});
            }
        }

        public async Task RemoveImageFromCollection(long collectionId, long imageId)
        {
            using (var connection = new SqliteConnection(_sqliteConnectionString))
            {
                connection.Open();
                await connection.ExecuteAsync("delete from Images2Collections where CollectionId = @collectionId and ImageId = @imageId", new { collectionId, imageId });
            }
        }

        public async Task DeleteImage(long id)
        {
            using (var connection = new SqliteConnection(_sqliteConnectionString))
            {
                connection.Open();
                await connection.ExecuteAsync("delete from Images where Id = @id", new { id });
            }
        }
    }
}