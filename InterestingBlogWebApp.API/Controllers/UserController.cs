using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using InterestingBlogWebApp.Application.Common.Interface.IServices;
using InterestingBlogWebApp.Application.Common_Interfaces.IServices;
using InterestingBlogWebApp.Application.DTOs;
using InterestingBlogWebApp.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly UserManager<User> _userManager;
    private readonly IEmailServices _emailService;

    public UserController(IUserService userService, UserManager<User> userManager,
     IEmailServices emailServices)
    {
        _userService = userService;
        _userManager = userManager;
        _emailService = emailServices;
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

    [HttpPost("update-user-details")]
    [Authorize] // Ensures the user is logged in
    public async Task<IActionResult> UpdateUserDetails([FromBody] UpdateUserDTO updateDTO)
    {
        // Fetch the user ID from the User Identity
        var userId = User.FindFirst("userId")?.Value;

        if (updateDTO == null)
        {
            return BadRequest("Invalid user data.");
        }

        if (string.IsNullOrEmpty(userId))
        {
            return BadRequest("User ID could not be found.");
        }

        updateDTO.Id = userId;

        List<string> errors = new List<string>();
        string result = await _userService.UpdateUserDetails(updateDTO, errors);

        if (result == "User details updated successfully.")
        {
            return Ok(result);
        }
        else
        {
            return BadRequest(new { Message = result, Errors = errors });
        }
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPasswordInitiate([FromBody] EmailDTO emailDTO)
    {
        if (string.IsNullOrEmpty(emailDTO.Email))
        {
            return BadRequest("Email is required");
        }

        var user = await _userManager.FindByEmailAsync(emailDTO.Email);
        if (user == null)
        {
            return NotFound($"User with email {emailDTO.Email} not found.");
        }

        var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
        try
        {
            // Construct the reset URL
            var frontEndUrl = "https://localhost:3000/reset-password";
            var encodedToken = WebUtility.UrlEncode(resetToken);
            var resetPasswordUrl = $"{frontEndUrl}?token={encodedToken}&email={Uri.EscapeDataString(user.Email)}";

            string subject = "Reset Your Password";
            string htmlContent = $"<p>Please reset your password by clicking <a href='{resetPasswordUrl}'>here</a>.</p>";

            // Send email with the reset URL
            await _emailService.SendEmailAsync(user.Email, user.UserName, subject, htmlContent);
            return Ok("Reset password link sent to your email.");
        }
        catch (Exception ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, new { message = $"Error sending email: {ex.Message}" });
        }
    }

    [HttpPost("confirm-reset-password")]
    public async Task<IActionResult> ResetPasswordConfirm([FromBody] PasswordResetDTO passwordResetDTO)
    {
        if (string.IsNullOrEmpty(passwordResetDTO.Email) || string.IsNullOrEmpty(passwordResetDTO.Token) || string.IsNullOrEmpty(passwordResetDTO.NewPassword))
        {
            return BadRequest("Email, token, and new password are required.");
        }

        var user = await _userManager.FindByEmailAsync(passwordResetDTO.Email);
        if (user == null)
        {
            return NotFound($"User with email {passwordResetDTO.Email} not found.");
        }

        var resetResult = await _userManager.ResetPasswordAsync(user, passwordResetDTO.Token, passwordResetDTO.NewPassword);
        if (!resetResult.Succeeded)
        {
            return BadRequest(new { errors = resetResult.Errors.Select(e => e.Description) });
        }

        return Ok("Password has been successfully reset.");
    }




}
