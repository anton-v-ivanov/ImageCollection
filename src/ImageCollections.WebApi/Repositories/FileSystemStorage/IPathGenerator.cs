namespace ImageCollections.WebApi.Repositories.FileSystemStorage
{
    public interface IPathGenerator
    {
        FilePathInfo Generate(string fileName, int chunkSize);
    }
}