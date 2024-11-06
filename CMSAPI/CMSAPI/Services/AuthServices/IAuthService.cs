using CMSAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace CMSAPI.Services.AuthServices;

public interface IAuthService
{
    Task<bool> RegisterUser(User user);

    Task<bool> Login(User user);

    string GenerateTokenString(IdentityUser user);
}