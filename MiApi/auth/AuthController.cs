using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthUserService _usuarioService;
    private readonly IConfiguration _configuration;

    public AuthController(AuthUserService userService, IConfiguration configuration)
    {
        _usuarioService = userService;
        _configuration = configuration;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDto loginDto)
    {
        var user = _usuarioService.Authenticate(loginDto.Email, loginDto.Password);
        if (user == null)
        {
            return Unauthorized("Correo o contraseña incorrectos.");
        }

        var token = _usuarioService.GenerateJwtToken(user);
        return Ok(new { Token = token });
    }
}

