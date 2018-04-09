namespace ImageCollections.WebApi.Managers.HashGenerator
{
    public interface IHashGenerator
    {
        byte[] Generate(string text);
        byte[] Generate(byte[] content);
    }
}