using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using UserManagement.Application.Common.Interfaces.Identity;
using UserManagement.Application.Features.Auth.Dtos;
using UserManagement.Domain.Entities.Identity;

namespace UserManagement.WebAPI.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IAuthService _authService;

    public AuthController(UserManager<ApplicationUser> userManager, IAuthService authService)
    {
        _userManager = userManager;
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _userManager.FindByNameAsync(request.UserName);
        if (user == null || !user.IsActive || user.IsDeleted)
            return Unauthorized("Invalid credentials.");

        var isValid = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!isValid)
            return Unauthorized("Invalid credentials.");

        if (_authService.IsPasswordExpired(user))
            return BadRequest("Password expired. Please change your password.");

        var result = await _authService.GenerateJwtTokenAsync(user);
        return Ok(result);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
    {
        try
        {
            var result = await _authService.RefreshTokenAsync(request.RefreshToken);
            return Ok(result);
        }
        catch (SecurityTokenException e)
        {
            return Unauthorized(e.Message);
        }
    }
}
