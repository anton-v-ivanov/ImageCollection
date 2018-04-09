namespace ImageCollections.Contracts.ImageInfos
{
    public class UpdateImageInfoRequestInternal
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public UpdateImageInfoRequestInternal(long id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}