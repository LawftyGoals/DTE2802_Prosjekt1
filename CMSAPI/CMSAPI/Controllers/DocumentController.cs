using System.Collections.Generic;
using CMSAPI.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using CMSAPI.Services.AuthServices;
using CMSAPI.Services.DocumentServices;

namespace CMSAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentService _documentService;
        private readonly IAuthService _authService;
        private readonly UserManager<IdentityUser> _userManager;

        public DocumentController(IDocumentService documentService, IAuthService authService, UserManager<IdentityUser> userManager)
        {
            _documentService = documentService;
            _authService = authService;
            _userManager = userManager;
        }

        // Midlertidig endpoint for å generere og returnere en token for testbrukeren
        [AllowAnonymous]
        [HttpGet("generate-test-token")]
        public async Task<IActionResult> GetTestToken()
        {
            // Finn testbrukeren basert på e-posten som er brukt i SeedData
            var testUser = await _userManager.FindByEmailAsync("test@example.com");
            if (testUser == null)
            {
                return NotFound("Test user not found.");
            }

            // Generer token for testbrukeren
            var token = _authService.GenerateTokenString(testUser);

            return Ok(new { token });
        }

        private string GetCurrentUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DocumentDto>>> GetAllDocuments()
        {
            var userId = GetCurrentUserId();
            var documents = await _documentService.GetAllDocumentsAsync(userId);
            return Ok(documents);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DocumentDto>> GetDocumentById(int id)
        {
            var userId = GetCurrentUserId();
            var document = await _documentService.GetDocumentByIdAsync(id, userId);
            if (document == null)
                return NotFound();

            return Ok(document);
        }

        [HttpPost]
        public async Task<ActionResult<DocumentDto>> CreateDocument(CreateDocumentDto createDocumentDto)
        {
            var userId = GetCurrentUserId();
            createDocumentDto.IdentityUserId = userId;
            var document = await _documentService.CreateDocumentAsync(createDocumentDto);
            return CreatedAtAction(nameof(GetDocumentById), new { id = document.Id }, document);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDocument(int id, UpdateDocumentDto updateDocumentDto)
        {
            var userId = GetCurrentUserId();
            var result = await _documentService.UpdateDocumentAsync(id, updateDocumentDto, userId);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocument(int id)
        {
            var userId = GetCurrentUserId();
            var result = await _documentService.DeleteDocumentAsync(id, userId);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
