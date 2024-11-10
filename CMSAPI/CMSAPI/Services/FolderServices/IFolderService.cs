using CMSAPI.DTOs;

namespace CMSAPI.Services.FolderServices;
public interface IFolderService {
    Task<List<FolderDto>> GetAllFoldersAsync(string userId);

    Task<FolderDto?> GetFolderByNameAsync(string userId, string path);

    Task<FolderDto?> GetFolderByIdAsync(string userId, int id);


}
