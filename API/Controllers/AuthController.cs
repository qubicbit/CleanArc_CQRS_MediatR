using Application.Auth.Dtos;
using Application.Common.Interfaces;

using Domain.Models.Common;

using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<OperationResult<AuthResponseDto>>> Register([FromBody] RegisterUserDto dto)
    {
        var result = await _authService.RegisterAsync(dto);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<ActionResult<OperationResult<AuthResponseDto>>> Login([FromBody] LoginUserDto dto)
    {
        var result = await _authService.LoginAsync(dto);

        if (!result.IsSuccess)
            return Unauthorized(result);

        return Ok(result);
    }

}
