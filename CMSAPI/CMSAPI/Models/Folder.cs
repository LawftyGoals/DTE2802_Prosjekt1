using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace CMSAPI.Models
{
    public class Folder
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? ParentFolderId { get; set; }
        public Folder? ParentFolder { get; set; } // Navigation property to the parent folder
        public ICollection<Folder>? SubFolders { get; set; } // Subfolders
        public string IdentityUserId { get; set; } // ID of the IdentityUser who owns the folder
        public IdentityUser? IdentityUser { get; set; } // Navigation property to the IdentityUser
        public ICollection<Document>? Documents { get; set; } // Documents in the folder
    }
}