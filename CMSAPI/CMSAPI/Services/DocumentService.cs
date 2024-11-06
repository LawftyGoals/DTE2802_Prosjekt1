using CMSAPI.Data;
using CMSAPI.DTOs;
using CMSAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CMSAPI.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly CMSAPIDbContext _context;

        public DocumentService(CMSAPIDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DocumentDto>> GetAllDocumentsAsync(string userId)
        {
            return await _context.Documents
                .Where(d => d.IdentityUserId == userId)
                .Select(d => new DocumentDto
                {
                    Id = d.Id,
                    Title = d.Title,
                    Content = d.Content,
                    ContentType = d.ContentType,
                    CreatedDate = d.CreatedDate,
                    IdentityUserId = d.IdentityUserId,
                    FolderId = d.FolderId
                })
                .ToListAsync();
        }

        public async Task<DocumentDto> GetDocumentByIdAsync(int id, string userId)
        {
            var document = await _context.Documents
                .FirstOrDefaultAsync(d => d.Id == id && d.IdentityUserId == userId);

            if (document == null)
                return null;

            return new DocumentDto
            {
                Id = document.Id,
                Title = document.Title,
                Content = document.Content,
                ContentType = document.ContentType,
                CreatedDate = document.CreatedDate,
                IdentityUserId = document.IdentityUserId,
                FolderId = document.FolderId
            };
        }

        public async Task<DocumentDto> CreateDocumentAsync(CreateDocumentDto createDocumentDto)
        {
            var document = new Document
            {
                Title = createDocumentDto.Title,
                Content = createDocumentDto.Content,
                ContentType = createDocumentDto.ContentType,
                CreatedDate = DateTime.UtcNow,
                IdentityUserId = createDocumentDto.IdentityUserId,
                FolderId = createDocumentDto.FolderId
            };

            _context.Documents.Add(document);
            await _context.SaveChangesAsync();

            return new DocumentDto
            {
                Id = document.Id,
                Title = document.Title,
                Content = document.Content,
                ContentType = document.ContentType,
                CreatedDate = document.CreatedDate,
                IdentityUserId = document.IdentityUserId,
                FolderId = document.FolderId
            };
        }

        public async Task<bool> UpdateDocumentAsync(int id, UpdateDocumentDto updateDocumentDto, string userId)
        {
            var document = await _context.Documents
                .FirstOrDefaultAsync(d => d.Id == id && d.IdentityUserId == userId);

            if (document == null)
                return false;

            document.Title = updateDocumentDto.Title;
            document.Content = updateDocumentDto.Content;
            document.ContentType = updateDocumentDto.ContentType;
            document.FolderId = updateDocumentDto.FolderId;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteDocumentAsync(int id, string userId)
        {
            var document = await _context.Documents
                .FirstOrDefaultAsync(d => d.Id == id && d.IdentityUserId == userId);

            if (document == null)
                return false;

            _context.Documents.Remove(document);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
