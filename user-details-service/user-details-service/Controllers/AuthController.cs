using application_infrastructure.DBContexts;
using application_infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using user_details_service.DTOs;
using user_details_service.Helpers.Logging;
using user_details_service.Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace user_details_service.Controllers;

[ApiController]
[Route("/auth")]
public class AuthController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly ApplicationDbContext _context;
    private readonly ITokenService _tokenService;
    private readonly ILoggerManager _logger;

    public AuthController(UserManager<User> userManager, ApplicationDbContext context, ITokenService tokenService, ILoggerManager logger)
    {
        _userManager = userManager;
        _context = context;
        _tokenService = tokenService;
        _logger = logger;
    }

    [HttpGet]
    [Route("/test")]
    public IActionResult Test()
    {
        _logger.LogInfo("test function called");
        return Ok("service online");
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register(CreateUserDTO request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state");
                return BadRequest(ModelState);
            }

            var userToBeCreated = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                QidNumber = request.QidNumber,
                Email = request.Email,
                UserName = request.Username
            };

            _logger.LogInfo($"Creating a new user with username {request.Username}");
            var result = await _userManager.CreateAsync(
                userToBeCreated,
                request.Password
            );

            if (result.Succeeded)
            {
                request.Password = "";
                await _context.SaveChangesAsync();
                _logger.LogInfo($"New user Created {request.Username}");
                return CreatedAtAction(nameof(Register), new { email = request.Email }, request);
            }

            foreach (var error in result.Errors)
            {
                _logger.LogError($"Error while creating the user {request.Username}, error details: {error}");
                ModelState.AddModelError(error.Code, error.Description);
            }
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occured {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<AuthResponseDTO>> Authenticate([FromBody] AuthRequestDTO request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state");
                return BadRequest(ModelState);
            }

            var managedUser = await _userManager.FindByEmailAsync(request.Email);
            if (managedUser is null)
            {
                _logger.LogInfo($"User {request.Email} not found");
                return NotFound("Not Found");
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(managedUser, request.Password);
            if (!isPasswordValid)
            {
                _logger.LogInfo($"Invalid credentials for {request.Email}");
                return BadRequest("Invalid Credentials");
            }

            var validUser = _context.Users.FirstOrDefault(usr => usr.Email == request.Email);
            if (validUser is null)
            {
                return Unauthorized();
            }

            var token = _tokenService.CreateToken(validUser);
            await _context.SaveChangesAsync();

            return Ok(new AuthResponseDTO
            {
                UserName = validUser.UserName,
                Email = validUser.Email,
                Token = token
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occured {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}