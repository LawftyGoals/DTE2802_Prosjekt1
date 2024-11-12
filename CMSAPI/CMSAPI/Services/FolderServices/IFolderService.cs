using CMSAPI.DTOs;
using CMSAPI.Models;

namespace CMSAPI.Services.FolderServices;
public interface IFolderService {
    Task<List<FolderDto>> GetAllFoldersAsync(string userId);

    Task<FolderDto?> GetFolderByNameAsync(string userId, string path);

    Task<FolderDto?> GetFolderByIdAsync(string userId, int id);

    Task Save (string userId, Folder folder);

    Task Delete (string userId, int id);
}
