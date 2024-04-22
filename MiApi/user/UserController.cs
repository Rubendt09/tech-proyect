using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;
    private readonly IConfiguration _configuration;

    public UserController(UserService userService, IConfiguration configuration)
    {
        _userService = userService;
        _configuration = configuration;
    }

    [HttpPost("register")]  
    [Authorize(Roles = "Admin")]    
    public IActionResult Register([FromBody] CreateUserDto createUserDto)
    {
        var userExists = _userService.GetByEmail(createUserDto.Email);
        if (userExists != null)
        {
            return BadRequest("El usuario ya existe.");
        }

        var user = _userService.Create(createUserDto);
        user.Password = ""; // No retornar la contraseña
        return Ok(user);
    }
}

