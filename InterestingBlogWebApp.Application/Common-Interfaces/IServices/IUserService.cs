using InterestingBlogWebApp.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterestingBlogWebApp.Application.Common.Interface.IServices
{
    public interface IUserService
    {
        Task<List<UserDTO>> GetAll();
        Task<string> DeleteUser(string id, List<string> errors);
        Task<string> UpdateUser(UpdateDTO updateUserDTO, List<string> errors);
        Task<UserDTO> GetUserById(string userId);
    }
}
