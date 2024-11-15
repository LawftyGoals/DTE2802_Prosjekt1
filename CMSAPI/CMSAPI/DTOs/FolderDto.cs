using System;
using System.Collections.Generic;
using CMSAPI.Models;

namespace CMSAPI.DTOs {
    public class FolderDto {
        public int Id { get; set; }
        public string? Name { get; set; }
        public Folder? ParentFolder { get; set; }
        public List<DocumentDto>? Documents { get; set; }
        public String Url { get; set; }
    }

    public class CreateFolderDto {
        public int? Id { get; set; }
        public string Name { get; set; } = null!;
        public int ParentFolderId { get; set; }
        public string? IdentityUserId { get; set; }
    }

}