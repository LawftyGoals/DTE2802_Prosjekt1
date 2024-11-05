using CMSAPI.DTOs;
using CMSAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CMSAPI.Controllers
{
    // Marks the class as an API controller and sets the route for the controller
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentController : ControllerBase
    {
        // Dependency injection for the document service
        private readonly IDocumentService _documentService;

        // Constructor to inject the document service
        public DocumentController(IDocumentService documentService)
        {
            _documentService = documentService;
        }

        // HTTP GET: /api/document
        // Retrieves all documents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DocumentDto>>> GetAllDocuments()
        {
            var documents = await _documentService.GetAllDocumentsAsync();
            return Ok(documents); // Returns HTTP 200 OK with the list of documents
        }

        // HTTP GET: /api/document/{id}
        // Retrieves a document by its ID
        [HttpGet("{id}")]
        public async Task<ActionResult<DocumentDto>> GetDocumentById(int id)
        {
            var document = await _documentService.GetDocumentByIdAsync(id);
            if (document == null)
                return NotFound(); // Returns HTTP 404 Not Found if the document doesn't exist

            return Ok(document); // Returns HTTP 200 OK with the document data
        }

        // HTTP POST: /api/document
        // Creates a new document
        [HttpPost]
        public async Task<ActionResult<DocumentDto>> CreateDocument(CreateDocumentDto createDocumentDto)
        {
            var document = await _documentService.CreateDocumentAsync(createDocumentDto);
            // Returns HTTP 201 Created with the newly created document and its location
            return CreatedAtAction(nameof(GetDocumentById), new { id = document.Id }, document);
        }

        // HTTP PUT: /api/document/{id}
        // Updates an existing document
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDocument(int id, UpdateDocumentDto updateDocumentDto)
        {
            var result = await _documentService.UpdateDocumentAsync(id, updateDocumentDto);
            if (!result)
                return NotFound(); // Returns HTTP 404 Not Found if the document doesn't exist

            return NoContent(); // Returns HTTP 204 No Content if the update is successful
        }

        // HTTP DELETE: /api/document/{id}
        // Deletes a document by its ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocument(int id)
        {
            var result = await _documentService.DeleteDocumentAsync(id);
            if (!result)
                return NotFound(); // Returns HTTP 404 Not Found if the document doesn't exist

            return NoContent(); // Returns HTTP 204 No Content if the deletion is successful
        }
    }
}
