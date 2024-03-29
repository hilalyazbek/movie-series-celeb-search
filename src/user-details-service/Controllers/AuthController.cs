﻿using application_infrastructure.DBContexts;
using application_infrastructure.Entities;
using application_infrastructure.Logging;
using application_infrastructure.TokenService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using user_details_service.DTOs;

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

    /// <summary>
    /// It takes a CreateUserDTO object, creates a new user object, and then creates a new user in the
    /// database
    /// </summary>
    /// <param name="CreateUserDTO">This is a class that contains the properties of the user that we
    /// want to create.</param>
    /// <returns>
    /// The user object is being returned.
    /// </returns>
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
                return Ok(request);
            }

            foreach (var error in result.Errors)
            {
                _logger.LogError($"Error while creating the user {request.Username}, error details: {error.Code}|{error.Description}");
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

    /// <summary>
    /// It takes in a request object, checks if the user exists, checks if the password is valid,
    /// creates a token, and returns the token
    /// </summary>
    /// <param name="AuthRequestDTO">This is the request object that contains the email and password of
    /// the user.</param>
    /// <returns>
    /// A token
    /// </returns>
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