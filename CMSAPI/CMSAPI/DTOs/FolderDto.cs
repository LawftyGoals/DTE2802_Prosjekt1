using CMSAPI.Models;

namespace CMSAPI.DTOs {
    public class FolderDto {
        public int Id { get; set; }
        public string? Name { get; set; }
        public Folder? ParentFolder { get; set; }
        public List<DocumentDto>? Documents { get; set; }
    }

    public class CreateFolderDto {
        public string? Name { get; set; }
        public int? ParentFolderId { get; set; }
        public string? IdentityUserId { get; set; }
    }

    public class UpdateFolderDto {
        public string? Name { get; set; }
        public int? ParentFolderId { get; set; }
    }
}