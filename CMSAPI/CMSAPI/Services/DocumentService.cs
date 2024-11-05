using CMSAPI.Data;
using CMSAPI.DTOs;
using CMSAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CMSAPI.Services
{
    // Implementation of the IDocumentService interface
    public class DocumentService : IDocumentService
    {
        private readonly CMSAPIDbContext _context;

        // Constructor for dependency injection of the database context
        public DocumentService(CMSAPIDbContext context)
        {
            _context = context;
        }

        // Retrieves all documents and maps them to DocumentDto
        public async Task<IEnumerable<DocumentDto>> GetAllDocumentsAsync()
        {
            return await _context.Documents
                .Select(d => new DocumentDto
                {
                    Id = d.Id,
                    Title = d.Title,
                    Content = d.Content,
                    ContentType = d.ContentType,
                    CreatedDate = d.CreatedDate,
                    UserId = d.UserId,
                    FolderId = d.FolderId
                })
                .ToListAsync(); // Executes the query and returns the results as a list
        }

        // Retrieves a document by its ID and maps it to DocumentDto
        public async Task<DocumentDto> GetDocumentByIdAsync(int id)
        {
            var document = await _context.Documents.FindAsync(id);
            if (document == null)
                return null; // Returns null if no document is found

            // Maps the found document to a DocumentDto
            return new DocumentDto
            {
                Id = document.Id,
                Title = document.Title,
                Content = document.Content,
                ContentType = document.ContentType,
                CreatedDate = document.CreatedDate,
                UserId = document.UserId,
                FolderId = document.FolderId
            };
        }

        // Creates a new document and returns the created document as DocumentDto
        public async Task<DocumentDto> CreateDocumentAsync(CreateDocumentDto createDocumentDto)
        {
            var document = new Document
            {
                Title = createDocumentDto.Title,
                Content = createDocumentDto.Content,
                ContentType = createDocumentDto.ContentType,
                CreatedDate = DateTime.UtcNow,
                UserId = createDocumentDto.UserId,
                FolderId = createDocumentDto.FolderId
            };

            _context.Documents.Add(document); // Adds the document to the database context
            await _context.SaveChangesAsync(); // Saves changes to the database

            // Returns the newly created document mapped to DocumentDto
            return new DocumentDto
            {
                Id = document.Id,
                Title = document.Title,
                Content = document.Content,
                ContentType = document.ContentType,
                CreatedDate = document.CreatedDate,
                UserId = document.UserId,
                FolderId = document.FolderId
            };
        }

        // Updates an existing document and returns a boolean indicating success
        public async Task<bool> UpdateDocumentAsync(int id, UpdateDocumentDto updateDocumentDto)
        {
            var document = await _context.Documents.FindAsync(id);
            if (document == null)
                return false; // Returns false if the document does not exist

            // Updates the document properties with new values
            document.Title = updateDocumentDto.Title;
            document.Content = updateDocumentDto.Content;
            document.ContentType = updateDocumentDto.ContentType;
            document.FolderId = updateDocumentDto.FolderId;

            await _context.SaveChangesAsync(); // Saves changes to the database
            return true; // Returns true if update is successful
        }

        // Deletes a document by its ID and returns a boolean indicating success
        public async Task<bool> DeleteDocumentAsync(int id)
        {
            var document = await _context.Documents.FindAsync(id);
            if (document == null)
                return false; // Returns false if the document does not exist

            _context.Documents.Remove(document); // Removes the document from the database context
            await _context.SaveChangesAsync(); // Saves changes to the database
            return true; // Returns true if deletion is successful
        }
    }
}
