﻿namespace CMSAPI.DTOs
{
    public class FolderDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? ParentFolderId { get; set; }
        public int UserId { get; set; }
    }

    public class CreateFolderDto
    {
        public string? Name { get; set; }
        public int? ParentFolderId { get; set; }
        public int UserId { get; set; }
    }

    public class UpdateFolderDto
    {
        public string? Name { get; set; }
        public int? ParentFolderId { get; set; }
    }
}