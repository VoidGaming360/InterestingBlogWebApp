using InterestingBlogWebApp.Application.DTOs;
using InterestingBlogWebApp.Domain.Entities;
using InterestingBlogWebApp.Domain.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace InterestingBlogWebApp.API.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public record LoginResponse(bool Flag, string Token, string Message);
        public record UserSession(string? Id, string? Name, string? Email, string? Role);
        public AccountController(UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager, IConfiguration configuration,
        SignInManager<ApplicationUser> signInManager)
        {

            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _signInManager = signInManager;
        }
        [HttpPost("Register-Blogger")]
        public async Task<IActionResult> RegisterBlogger(RegisterDTO model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return BadRequest(errors);
            }

            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            // Check if the specified role "Blogger" exists
            var roleExists = await _roleManager.RoleExistsAsync("Blogger");

            if (!roleExists)
            {
                // If the role doesn't exist, create it
                var createRoleResult = await _roleManager.CreateAsync(new IdentityRole("Blogger"));

                if (!createRoleResult.Succeeded)
                {
                    // If role creation fails, return error
                    return BadRequest("Failed to create role 'Blogger'.");
                }
            }

            // Assign the role "Blogger" to the user
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Blogger");
                return Ok("User registered successfully.");
            }

            return BadRequest(result.Errors);
        }


        [HttpPost("Register-Admin")]
        public async Task<IActionResult> RegisterAdmin(AdminRegisterDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };

            // Check if the specified role "Blogger" exists
            var roleExists = await _roleManager.RoleExistsAsync("Admin");

            if (!roleExists)
            {
                // If the role doesn't exist, create it
                var createRoleResult = await _roleManager.CreateAsync(new IdentityRole("Admin"));

                if (!createRoleResult.Succeeded)
                {
                    // If role creation fails, return error
                    return BadRequest("Failed to create role 'Admin'.");
                }
            }

            // Assign the role "Blogger" to the user
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Admin");
                return Ok("Admin registered successfully.");
            }

            return BadRequest(result.Errors);
        }
        

        [HttpPost("Login")]
        public async Task<LoginResponse> Login([FromBody] LoginModel loginUser)
        {

            var result = await _signInManager.PasswordSignInAsync(loginUser.Email,
            loginUser.Password, false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                var getUser = await _userManager.FindByEmailAsync(loginUser.Email);
                var getUserRole = await _userManager.GetRolesAsync(getUser);
                var userSession = new UserSession(getUser.Id, getUser.UserName,
                getUser.Email, getUserRole.First());
                string token = GenerateToken(userSession);
                return new LoginResponse(true, token!, "Login completed");
            }
            else
            {
                return new LoginResponse(false, null!, "Login not completed");
            }
        }

        private string GenerateToken(UserSession user)
        {
            var securityKey = new
            SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey,
            SecurityAlgorithms.HmacSha256);
            var userClaims = new[]
            {
new Claim(ClaimTypes.NameIdentifier, user.Id),
new Claim(ClaimTypes.Name, user.Name),
new Claim(ClaimTypes.Email, user.Email),
new Claim(ClaimTypes.Role, user.Role)
};
            var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: userClaims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: credentials

            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
