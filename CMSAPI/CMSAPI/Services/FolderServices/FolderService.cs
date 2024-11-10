using CMSAPI.Data;
using CMSAPI.DTOs;
using CMSAPI.Models;
using CMSAPI.Services.DocumentServices;
using Microsoft.EntityFrameworkCore;

namespace CMSAPI.Services.FolderServices;
public class FolderService : IFolderService {

    private readonly CMSAPIDbContext _context;
    private readonly IDocumentService _documentService;

    public FolderService(CMSAPIDbContext context, IDocumentService documentService) {
        _documentService = documentService;
        _context = context;

    }

    public async Task<List<FolderDto>> GetAllFoldersAsync(string userId) {
        try {
            var folders = await _context.Folders.Where(f => f.IdentityUserId == userId).ToListAsync();

            var documents = await _documentService.GetAllDocumentsAsync(userId);

            var foldersDto = folders.Select(f => {

                var parent = folders.FirstOrDefault(p => p.Id == f.ParentFolderId);
                var foldersDocuments = documents.Where(d => d.FolderId == f.Id).ToList();

                return new FolderDto() {
                    Id = f.Id,
                    Name = f.Name,
                    ParentFolder = parent,
                    Documents = foldersDocuments
                };
            }).ToList();

            return foldersDto;
        }

        catch (Exception ex) {
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);

            return new List<FolderDto>();

        }
    }

    public async Task<FolderDto?> GetFolderByNameAsync(string userId, string name) {
        try {
            var folders = await _context.Folders.Where(f => f.IdentityUserId == userId).ToListAsync();

            var folder = folders
                .FirstOrDefault(f => f.Name == name);

            if (folder == null) { return null; }

            var documents = await _documentService.GetAllDocumentsAsync(userId);

            var parent = folder.ParentFolderId != null ? folders.FirstOrDefault(p => p.Id == p.ParentFolderId) : null;
            var foldersDocuments = documents.Where(d => d.FolderId == d.Id).ToList();

            var folderDto = new FolderDto() {
                Id = folder.Id,
                Name = folder.Name,
                ParentFolder = parent,
                Documents = foldersDocuments
            };

            return folderDto;
        }
        catch (Exception e) {
            Console.WriteLine(e);
            throw;
        }

    }

    public async Task<FolderDto?> GetFolderByIdAsync(string userId, int id) {
        try {
            var folders = await _context.Folders.Where(f => f.IdentityUserId == userId).ToListAsync();

            var folder = folders
                .FirstOrDefault(f => f.Id == id);

            if (folder == null) { return null; }

            var documents = await _documentService.GetAllDocumentsAsync(userId);
            var parent = folder.ParentFolderId != null ? folders.FirstOrDefault(p => p.Id == p.ParentFolderId) : null;

            var foldersDocuments = documents.Where(d => d.FolderId == d.Id).ToList();

            var folderDto = new FolderDto() {
                Id = folder.Id,
                Name = folder.Name,
                ParentFolder = parent,
                Documents = foldersDocuments
            };

            return folderDto;
        }
        catch (Exception e) {
            Console.WriteLine(e);
            throw;
        }

    }

    public async Task Save(string userId, Folder folder) {
        var existingFolder = await _context.Folders.FindAsync(folder.Id);
        if (existingFolder != null) {
            _context.Entry(existingFolder).State = EntityState.Detached;
        }

        _context.Folders.Update(folder);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(string userId, int id) {

        var folders = await _context.Folders.Where(f => f.IdentityUserId == userId).ToListAsync();
        var folder = folders
            .FirstOrDefault(f => f.Id == id) ?? throw new InvalidOperationException($"No folder with id {id} found.");

        if (folder.ParentFolderId == null) {
            throw new InvalidOperationException($"Root folder {folder.Name} can not be deleted.");
        }

        var subFolderCount = folders.Where(s => s.ParentFolderId == id).ToList().Count;
        var documentCount = (await _documentService.GetAllDocumentsAsync(userId)).Where(doc => doc.FolderId == id).ToList().Count;



        if (subFolderCount == 0 || documentCount == 0) {
            throw new InvalidOperationException($"Folder {folder.Name} has contents and can not be deleted");
        }

        _context.Folders.Remove(folder);
        await _context.SaveChangesAsync();
    }


}
