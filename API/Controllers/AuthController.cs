using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService authService;

    public AuthController(IAuthService authService)
    {
        this.authService = authService;
    }

    [HttpPost]
    public async Task<ActionResult<LoginUserReturnDto>> LoginAsync(LoginUserDto loginUserDto)
    {
        var user = await this.authService.LoginUserAsync(loginUserDto);
        if (user == null)
        {
            return Unauthorized();
        }

        return this.Ok(user);
    }

    [HttpPost("register")]

    public async Task<ActionResult<LoginUserReturnDto>> Register([FromBody] RegisterUserDto userRegisterData)
    {
        if (!await this.authService.ValidateEmailAsync(userRegisterData.Email))
        {
            return BadRequest("Email already exists");
        }

        var user = await this.authService.RegisterUserAsync(userRegisterData);

        return this.Ok(user);
    }
}
