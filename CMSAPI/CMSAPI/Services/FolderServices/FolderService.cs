using CMSAPI.Data;
using CMSAPI.DTOs;
using CMSAPI.Models;
using CMSAPI.Services.DocumentServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

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


                String url = getFullUrl(folders, f, parent);

                return new FolderDto()
                {
                    Id = f.Id,
                    Name = f.Name,
                    ParentFolder = parent,
                    Documents = foldersDocuments,
                    Url = url
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

            var splitName = name.Split("/").ToList();

            var folder = traverseToTarget(folders, splitName);

            if (folder == null) { return null; }

            var documents = await _documentService.GetAllDocumentsAsync(userId);

            var parent = folder.ParentFolderId != null ? folders.FirstOrDefault(p => p.Id == p.ParentFolderId) : null;
            var foldersDocuments = documents.Where(d => d.FolderId == d.Id).ToList();

            var url = getFullUrl(folders, folder, parent);

            var folderDto = new FolderDto() {
                Id = folder.Id,
                Name = folder.Name,
                ParentFolder = parent,
                Documents = foldersDocuments,
                Url = url
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

    public async Task Save(string userId, CreateFolderDto folderDto) {
        var existingFolder = await _context.Folders.FirstOrDefaultAsync(f => f.Id == folderDto.Id && f.IdentityUserId == userId);
        if (existingFolder != null) {


            _context.Entry(existingFolder).State = EntityState.Detached;
        }

        var folder = new Folder()
        {
            Name = folderDto.Name,
            ParentFolderId = folderDto.ParentFolderId,
            IdentityUserId = userId
        };

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

    private String getFullUrl(List<Folder> folders, Folder folder, Folder? parent) {

        String url = parent?.Name ?? "" + "/" + folder.Name;

        var targetParent = parent;

        while (targetParent != null)
        {
            parent = folders.FirstOrDefault(tp => tp.Id == targetParent.ParentFolderId);

            url = targetParent.Name + "/" + url;
        }

        return url;
    }

    private List<Folder> getAncestors(List<Folder> folders, Folder folder)
    {
        var ancestors = new List<Folder>() {};

        Folder? parent = folders.FirstOrDefault(p => p.Id == folder.ParentFolderId);

        while (parent != null)
        {
            ancestors.Add(parent);

            parent = folders.FirstOrDefault(p => p.Id == parent.ParentFolderId);

        }
        
        ancestors.Reverse();

        return ancestors;

    }

    private Folder? traverseToTarget(List<Folder> folders, List<String> splitFolders)
    {
        splitFolders.Reverse();
        
        var finalTarget = folders.FirstOrDefault(t => t.Name == splitFolders[0]);

        if (finalTarget == null)
        {
            return null;
        }

        for (var i = 1; i < splitFolders.Count; i++)
        {
            var nextFolder = folders.FirstOrDefault(nf => nf.Name == splitFolders[i]);

            if (nextFolder == null)
            {
                return null;
            }

            if (nextFolder.ParentFolderId != null)
            {
                continue;
            }
        }

        return finalTarget;

    }

    public async Task<Folder?> GetUserRootFolder(string userId)
    {
        return await _context.Folders.FirstOrDefaultAsync(rf => rf.IdentityUserId == userId && rf.ParentFolderId == null);
    }

}
