namespace CMSAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public ICollection<Document>? Documents { get; set; } // Documents associated with the user
        public ICollection<Folder>? Folders { get; set; } // Folders associated with the user
    }
}