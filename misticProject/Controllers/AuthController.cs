using Microsoft.AspNetCore.Mvc;
using misticProject.Data.DTOs;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserService _userService;

    public AuthController(UserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto registerUserDto)
    {
        var result = await _userService.RegisterUser(registerUserDto);
        if (result.Succeeded)
        {
            return Ok();
        }

        return BadRequest(result.Errors);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var result = await _userService.LoginUser(loginDto.Email, loginDto.Password, loginDto.RememberMe);
        if (result.Succeeded)
        {
            return Ok();
        }
        else if (result.IsLockedOut)
        {
            return Forbid("Konto jest zablokowane.");
        }

        return Unauthorized("Nieprawidłowy email lub hasło.");
    }
}
