using InterestingBlogWebApp.Application.Common.Interface.IRepositories;
using InterestingBlogWebApp.Application.Common.Interface.IServices;
using InterestingBlogWebApp.Application.DTOs;
using InterestingBlogWebApp.Domain.Entities;
using Microsoft.AspNetCore.Identity;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    private readonly IUserRepository _userRepository;

    public UserService(UserManager<User> userManager, IUserRepository userRepository)
    {
        _userManager = userManager;
        _userRepository = userRepository;
    }

    public async Task<List<UserDTO>> GetAll()
    {
        var users = await _userRepository.GetAll(null); 

        var userDTOs = users.Select(user => new UserDTO
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email
        }).ToList(); 

        return userDTOs; 
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
            Email = user.Email
        };
    }
}
