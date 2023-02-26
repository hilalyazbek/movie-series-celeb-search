using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using user_details_service.DTOs;
using user_details_service.Entities;
using user_details_service.Infrastructure.DBContexts;
using user_details_service.Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace user_details_service.Controllers;

[ApiController]
[Route("/users")]
public class UsersController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly UsersContext _context;
    private readonly TokenService _tokenService;
    private readonly IMapper _mapper;

    public UsersController(
        UserManager<User> userManager,
        UsersContext context,
        TokenService tokenService,
        IMapper mapper)
    {
        _userManager = userManager;
        _context = context;
        _tokenService = tokenService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IEnumerable<UserDTO>> Users()
    {
        var users = await _context.Users.ToListAsync();

        var result = _mapper.Map<IEnumerable<UserDTO>>(users);

        return result;
    }

    [HttpGet]
    [Route("/users/{id}")]
    public async Task<UserDTO> UserDetails(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        var result = _mapper.Map<UserDTO>(user);

        return result;
    }

    [HttpPost("{id:guid}")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var userToBeDeleted = await _userManager.FindByIdAsync(id);
        if(userToBeDeleted is null)
        {
            return NotFound("Invalid User");
        }

        var result = await _userManager.DeleteAsync(userToBeDeleted);
        if (result.Succeeded)
        {
            await _context.SaveChangesAsync();
            return Ok($"User with id {id} was deleted successfully");
        }
        return BadRequest("something went wrong");
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateUser(string id, UpdateUserDTO user)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userToBeUpdated = await _userManager.FindByIdAsync(id);
        if(userToBeUpdated is null)
        {
            return BadRequest("Invalid User");
        }

        if(!string.IsNullOrEmpty(user.FirstName))
            userToBeUpdated.FirstName = user.FirstName;

        if (!string.IsNullOrEmpty(user.FirstName))
            userToBeUpdated.LastName = user.LastName;

        if (!string.IsNullOrEmpty(user.QidNumber))
            userToBeUpdated.QidNumber = user.QidNumber;

        var result = await _userManager.UpdateAsync(userToBeUpdated);

        if (!result.Succeeded)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

}

