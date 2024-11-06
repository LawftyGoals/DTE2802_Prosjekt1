using CMSAPI.DTOs;

namespace CMSAPI.Services
{
    public interface IDocumentService
    {
        // Method to retrieve all documents for a specific user as a list of DocumentDto
        Task<IEnumerable<DocumentDto>> GetAllDocumentsAsync(string userId);

        // Method to retrieve a single document by its ID and userId as a DocumentDto
        Task<DocumentDto> GetDocumentByIdAsync(int id, string userId);

        // Method to create a new document associated with the specified user
        Task<DocumentDto> CreateDocumentAsync(CreateDocumentDto createDocumentDto);

        // Method to update an existing document identified by its ID and userId
        Task<bool> UpdateDocumentAsync(int id, UpdateDocumentDto updateDocumentDto, string userId);

        // Method to delete a document identified by its ID and userId
        Task<bool> DeleteDocumentAsync(int id, string userId);
    }
}