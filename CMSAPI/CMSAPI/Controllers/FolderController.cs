using System.Collections.Generic;
using CMSAPI.DTOs;
using CMSAPI.Services.AuthServices;
using CMSAPI.Services.FolderServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CMSAPI.Controllers;


[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FolderController : ControllerBase {


    private readonly IFolderService _folderService;
    private readonly IAuthService _authService;
    private readonly UserManager<IdentityUser> _userManager;

    public FolderController(IFolderService folderService, IAuthService authService, UserManager<IdentityUser> userManager) {
        _folderService = folderService;
        _authService = authService;
        _userManager = userManager;
    }


    private async Task<string> GetCurrentUserId() {

        return await Task.FromResult(User.FindFirstValue(ClaimTypes.NameIdentifier));
    }

    [HttpGet]
    public async Task<ActionResult<List<FolderDto>>> GetAllFolders() {
        var userId = await GetCurrentUserId();

        var folders = await _folderService.GetAllFoldersAsync(userId);

        return folders == null ? NotFound() : Ok(folders);

    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<FolderDto>> GetFolderById(int id) {

        return Ok();
    }




}
