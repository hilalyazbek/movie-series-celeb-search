﻿using System;
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
using user_details_service.Infrastructure.Repositories;
using user_details_service.Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace user_details_service.Controllers;

[ApiController]
[Route("/users")]
public class UsersController : Controller
{
    //private readonly UserManager<User> _userManager;
    //private readonly ApplicationDbContext _context;
    private readonly IUserRepository _userRepository;
    private readonly TokenService _tokenService;
    private readonly IMapper _mapper;

    public UsersController(
        UserManager<User> userManager,
        ApplicationDbContext context,
        IUserRepository userRepository,
        TokenService tokenService,
        IMapper mapper)
    {
        //_userManager = userManager;
        //_context = context;
        _userRepository = userRepository;
        _tokenService = tokenService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IEnumerable<ViewUserDTO>> GetUsers([FromQuery] UserParameters userParameters)
    {
        var users = _userRepository.GetUsersAsync(userParameters);
        var result = _mapper.Map<IEnumerable<ViewUserDTO>>(users);
        return result;
    }

    [HttpGet]
    [Route("/users/{id}")]
    public async Task<ViewUserDTO> UserDetails(string id)
    {
        var user =  await _userRepository.GetUserByIdAsync(id);

        var result = _mapper.Map<ViewUserDTO>(user);

        return result;
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var userToBeDeleted = await _userRepository.GetUserByIdAsync(id);
        if(userToBeDeleted is null)
        {
            return NotFound("Invalid User");
        }

        _userRepository.Delete(userToBeDeleted);

        return Ok($"User with id {id} was deleted successfully");
        
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateUser(string id, UpdateUserDTO user)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userToBeUpdated = await _userRepository.GetUserByIdAsync(id);

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

        var result = _userRepository.Update(userToBeUpdated);

        return Ok(result);
    }

}

