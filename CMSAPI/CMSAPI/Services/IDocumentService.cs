using CMSAPI.DTOs;

namespace CMSAPI.Services
{
    // Interface defining the contract for the Document service
    public interface IDocumentService
    {
        // Method to retrieve all documents as a list of DocumentDto
        Task<IEnumerable<DocumentDto>> GetAllDocumentsAsync();

        // Method to retrieve a single document by its ID as a DocumentDto
        Task<DocumentDto> GetDocumentByIdAsync(int id);

        // Method to create a new document from CreateDocumentDto
        // Returns the created document as a DocumentDto
        Task<DocumentDto> CreateDocumentAsync(CreateDocumentDto createDocumentDto);

        // Method to update an existing document identified by its ID
        Task<bool> UpdateDocumentAsync(int id, UpdateDocumentDto updateDocumentDto);

        // Method to delete a document identified by its ID
        Task<bool> DeleteDocumentAsync(int id);
    }
}