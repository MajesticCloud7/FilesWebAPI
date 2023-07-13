namespace FilesWebAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public int UserGroupId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
    }
}