﻿using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CMSAPI.Services.AuthServices;

public class AuthService : IAuthService {
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IConfiguration _config;

    public AuthService(UserManager<IdentityUser> userManager, IConfiguration config) {
        _userManager = userManager;
        _config = config;
    }

    // Register a new user with IdentityUser
    public async Task<bool> RegisterUser(string username, string email, string password) {
        var identityUser = new IdentityUser {
            UserName = username,
            Email = email
        };

        var result = await _userManager.CreateAsync(identityUser, password);
        return result.Succeeded;
    }

    // Login by verifying the user's email and password
    public async Task<bool> Login(string email, string password) {
        var identityUser = await _userManager.FindByEmailAsync(email);
        if (identityUser == null) {
            return false;
        }

        return await _userManager.CheckPasswordAsync(identityUser, password);
    }

    // Generate a JWT token for the authenticated IdentityUser
    public string GenerateTokenString(IdentityUser user) {
        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id) // IdentityUser ID
            };

        var jwtKey = _config.GetSection("Jwt:Key").Value;
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

        var securityToken = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddHours(48),
            issuer: _config.GetSection("Jwt:Issuer").Value,
            audience: _config.GetSection("Jwt:Audience").Value,
            signingCredentials: signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }
}
