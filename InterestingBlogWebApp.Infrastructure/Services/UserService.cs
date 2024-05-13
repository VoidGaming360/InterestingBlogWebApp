using InterestingBlogWebApp.Application.Common.Interface.IRepositories;
using InterestingBlogWebApp.Application.Common.Interface.IServices;
using InterestingBlogWebApp.Application.DTOs;
using InterestingBlogWebApp.Domain.Entities;
using InterestingBlogWebApp.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    private readonly IUserRepository _userRepository;
    private readonly AppDbContext _context;

    public UserService(UserManager<User> userManager, IUserRepository userRepository, AppDbContext context)
    {
        _userManager = userManager;
        _userRepository = userRepository;
        _context = context;
    }

    public async Task<List<UserDTO>> GetAll()
    {
        var users = await _userRepository.GetAll(null); 

        var userDTOs = users.Select(user => new UserDTO
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName
        }).ToList(); 

        return userDTOs; 
    }

    public async Task<string> GetUserNameById(string userId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        return user?.UserName; // Returns the UserName if user is found, otherwise null
    }

    public async Task<string> DeleteUser(string userId, List<string> errors)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            errors.Add("User not found.");
            return "Delete failed.";
        }

        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
        {
            errors.Add("Delete operation failed.");
            return "Delete failed.";
        }

        return "User deleted successfully.";
    }

    public async Task<string> UpdateUser(UpdateDTO updateUserDTO, List<string> errors)
    {
        var user = await _userManager.FindByIdAsync(updateUserDTO.Id);

        if (user == null)
        {
            errors.Add("User not found.");
            return "Update failed.";
        }

        var changePasswordResult = await _userManager.ChangePasswordAsync(
            user,
            updateUserDTO.CurrentPassword,
            updateUserDTO.NewPassword
        );

        if (!changePasswordResult.Succeeded)
        {
            errors.AddRange(changePasswordResult.Errors.Select(e => e.Description));
            return "Password update failed.";
        }

        return "User updated successfully.";
    }

    public async Task<string> UpdateUserDetails( UpdateUserDTO updateDTO, List<string> errors)
    {
        var user = await _userManager.FindByIdAsync(updateDTO.Id);

        if (user == null)
        {
            errors.Add("User not found.");
            return "Update failed.";
        }

        bool isUpdated = false;

        // Update UserName if it has changed
        if (updateDTO.UserName != null && updateDTO.UserName != user.UserName)
        {
            user.UserName = updateDTO.UserName;
            isUpdated = true;
        }

        // Update FirstName if it has changed
        if (updateDTO.FirstName != null && updateDTO.FirstName != user.FirstName)
        {
            user.FirstName = updateDTO.FirstName;
            isUpdated = true;
        }

        // Update LastName if it has changed
        if (updateDTO.LastName != null && updateDTO.LastName != user.LastName)
        {
            user.LastName = updateDTO.LastName;
            isUpdated = true;
        }

        // Save changes if any updates were made
        if (isUpdated)
        {
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return "User details updated successfully.";
            }
            else
            {
                errors.AddRange(result.Errors.Select(e => e.Description));
                return "Update failed.";
            }
        }
        else
        {
            return "No updates necessary.";
        }
    }


    public async Task<UserDTO> GetUserById(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            throw new KeyNotFoundException("User not found."); // Handle user not found
        }

        return new UserDTO
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName
        };
    }
}
