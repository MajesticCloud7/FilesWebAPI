namespace FilesWebAPI.Models
{
    public class File
    {
        public int Id { get; set; }
        public int FileGroupId { get; set; }
        public string Name { get; set; }
        public byte[] FileBlob { get; set; }
    }
}