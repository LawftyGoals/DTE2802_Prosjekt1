using System.Collections.Generic;
using System.Threading.Tasks;
using CMSAPI.DTOs;

namespace CMSAPI.Services.FolderServices;
public interface IFolderService {
    Task<List<FolderDto>> GetAllFoldersAsync(string userId);

    Task<FolderDto?> GetFolderByNameAsync(string userId, string path);

    Task<FolderDto?> GetFolderByIdAsync(string userId, int id);

    Task Save (string userId, CreateFolderDto folder);

    Task Delete (string userId, int id);

    Task CreateRootFolder(string userId);
}
