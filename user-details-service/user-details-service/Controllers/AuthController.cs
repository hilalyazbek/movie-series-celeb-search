using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using user_details_service.DTOs;
using user_details_service.Entities;
using user_details_service.Infrastructure.DBContexts;
using user_details_service.Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace user_details_service.Controllers;

[ApiController]
[Route("/auth")]
public class AuthController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly UsersContext _context;
    private readonly TokenService _tokenService;

    public AuthController(UserManager<User> userManager, UsersContext context, TokenService tokenService)
    {
        _userManager = userManager;
        _context = context;
        _tokenService = tokenService;
    }

    [HttpGet]
    [Route("/test")]
    public IActionResult Test()
    {
        return Ok("service online");
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register(UserDTO request)
    {
        if (!ModelState.IsValid)
        {
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

        var result = await _userManager.CreateAsync(
            userToBeCreated,
            request.Password
        );

        if (result.Succeeded)
        {
            request.Password = "";
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Register), new { email = request.Email }, request);
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(error.Code, error.Description);
        }
        return BadRequest(ModelState);
    }

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<AuthResponseDTO>> Authenticate([FromBody] AuthRequestDTO request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var managedUser = await _userManager.FindByEmailAsync(request.Email);
        if(managedUser is null)
        {
            return BadRequest("Invalid Credentials");
        }

        var isPasswordValid = await _userManager.CheckPasswordAsync(managedUser, request.Password);
        if (!isPasswordValid)
        {
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

}

