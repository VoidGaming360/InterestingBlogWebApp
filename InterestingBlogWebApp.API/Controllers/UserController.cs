using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using InterestingBlogWebApp.Application.Common.Interface.IServices;
using InterestingBlogWebApp.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    //[Authorize(Roles = "Admin")] 
    [HttpGet("all")]
    public async Task<IActionResult> GetAllUsers()
    {
        try
        {
            var users = await _userService.GetAll(); 
            return Ok(users);
        }
        catch (Exception ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, new { message = ex.Message });
        }
    }

    [Authorize] 
    [HttpGet("by-user-id")]
    public async Task<IActionResult> GetUserById()
    {
        try
        {
            var userId = User.FindFirst("userId")?.Value;

            var userDTO = await _userService.GetUserById(userId);
            return Ok(userDTO); 
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message }); 
        }
        catch (Exception ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, new { message = ex.Message });
        }
    }

    [Authorize]
    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteUser()
    {
        var errors = new List<string>();
        try
        {
            var userId = User.FindFirst("userId")?.Value;

            var response = await _userService.DeleteUser(userId, errors);
            if (errors.Count > 0)
            {
                return BadRequest(new { errors }); 
            }

            return Ok(new { message = response }); 
        }
        catch (Exception ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, new { message = ex.Message });
        }
    }

    [Authorize]
    [HttpPost("update")]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateDTO updateUserDTO)
    {
        var errors = new List<string>();

        try
        {
            // Retrieve the user ID from the JWT claim
            var userId = User.FindFirst("userId")?.Value; // Adjust based on your JWT claims

            if (string.IsNullOrEmpty(userId)) // If UserId is not found in the token
            {
                return Unauthorized(new { message = "User ID not found in token." });
            }

            updateUserDTO.Id = userId;

            var response = await _userService.UpdateUser(updateUserDTO, errors);
            if (errors.Count > 0)
            {
                return BadRequest(new { errors }); 
            }

            return Ok(new { message = response }); 
        }
        catch (Exception ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, new { message = ex.Message });
        }
    }
}
