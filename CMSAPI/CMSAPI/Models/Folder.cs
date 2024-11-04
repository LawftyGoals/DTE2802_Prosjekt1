namespace CMSAPI.Models
{
    public class Folder
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? ParentFolderId { get; set; }
        public Folder? ParentFolder { get; set; } // Navigation property to the parent folder
        public ICollection<Folder>? SubFolders { get; set; } // Subfolders
        public int UserId { get; set; } // ID of the user who owns the folder
        public User? User { get; set; } // Navigation property to the user
        public ICollection<Document>? Documents { get; set; } // Documents in the folder
    }
}