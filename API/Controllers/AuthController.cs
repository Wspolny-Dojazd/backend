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
        this.authService= authService;
    }

    [HttpPost]
    public async Task<ActionResult<LoginUserReturnDto>> LoginAsync(LoginUserDto loginUserDto)
    {
        var user = await authService.LoginUserAsync(loginUserDto);
        if (user == null)
        {
            return Unauthorized();
        }
        return Ok(user);
    }

}
