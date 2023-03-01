using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using application_infrastructure.Logging;
using application_infrastructure.PagingAndSorting;
using application_infrastructure.Repositories;
using application_infrastructure.Repositories.Interfaces;
using application_infrastructure.TokenService;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using user_details_service.DTOs;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace user_details_service.Controllers;

[ApiController]
[Route("/users")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class UsersController : Controller
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;
    private readonly ILoggerManager _logger;

    public UsersController(
        IUserRepository userRepository,
        ITokenService tokenService,
        IMapper mapper,
        ILoggerManager logger)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers([FromQuery] PagingParameters pagingParameters,
        [FromQuery] SortingParameters sortingParameters)
    {
        try
        {
            var users = await _userRepository.GetUsersAsync(pagingParameters, sortingParameters);
            var result = _mapper.Map<IEnumerable<ViewUserDTO>>(users);

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occured {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet]
    [Route("/users/{id}")]
    public async Task<IActionResult> UserDetails(string id)
    {
        try
        {
            var user = await _userRepository.GetUserByIdAsync(id);

            if (user is null)
            {
                return NotFound($"user with id {id} does not exist");
            }
            var result = _mapper.Map<ViewUserDTO>(user);

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occured {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        try
        {
            var userToBeDeleted = await _userRepository.GetUserByIdAsync(id);
            if (userToBeDeleted is null)
            {
                _logger.LogInfo($"user with id {id} does not exist");
                return NotFound($"user with id {id} does not exist");
            }

            _userRepository.Delete(userToBeDeleted);

            return Ok($"User with id {id} was deleted successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occured {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateUser(string id, UpdateUserDTO user)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"invalid model state");
                return BadRequest(ModelState);
            }

            var userToBeUpdated = await _userRepository.GetUserByIdAsync(id);

            if (userToBeUpdated is null)
            {
                _logger.LogInfo($"user with id {id} does not exist");
                return NotFound($"Invalid User with id {id}");
            }

            if (!string.IsNullOrEmpty(user.FirstName))
                userToBeUpdated.FirstName = user.FirstName;

            if (!string.IsNullOrEmpty(user.FirstName))
                userToBeUpdated.LastName = user.LastName;

            if (!string.IsNullOrEmpty(user.QidNumber))
                userToBeUpdated.QidNumber = user.QidNumber;

            var result = _userRepository.Update(userToBeUpdated);

            if (result is null)
            {
                _logger.LogError($"user with id {id} cannot be updated.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occured {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

}

