using GenExcel.API.Auth;
using Microsoft.AspNetCore.Mvc;

namespace GenExcel.API.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController : ControllerBase
{
    private readonly IJwtTokenService _jwt;

    public AuthController(IJwtTokenService jwt)
    {
        _jwt = jwt;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        if (request.Email != "admin@genexcel.com" || request.Password != "123")
            return Unauthorized(new { message = "Invalid credentials" });

        var token = _jwt.GenerateToken(
            userId: "1",
            email: request.Email,
            roles: ["Admin"]
        );

        return Ok(new { accessToken = token });
    }
}

public sealed record LoginRequest(string Email, string Password);
