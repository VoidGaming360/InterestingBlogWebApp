using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using InterestingBlogWebApp.Application.DTOs;
using InterestingBlogWebApp.Application.Helpers;
using InterestingBlogWebApp.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace InterestingBlogWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthenticationController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return StatusCode((int)HttpStatusCode.Unauthorized, new Response(null, new List<string> { "Invalid login credentials." }, HttpStatusCode.Unauthorized));
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>
        {
            new Claim("name", user.UserName), // User name claim
            new Claim("userId", user.Id), // Custom claim key for UserId
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Unique token ID
        };

            // Add role claims
            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim("role", role)); // Role claims
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.WriteToken(token);

            return Ok(new Response(
                new { token = jwtToken, expiration = token.ValidTo },
                null,
                HttpStatusCode.OK
            ));
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new Response(null, new List<string> { "User already exists." }, HttpStatusCode.InternalServerError));
            }


            var user = new User
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new Response(null, new List<string> { "User creation failed. Please check the details and try again." }, HttpStatusCode.InternalServerError));
            }

            // Ensure the admin role exists, create if necessary
            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
            {
                var role = new IdentityRole(UserRoles.User)
                {
                    ConcurrencyStamp = Guid.NewGuid().ToString() // Ensure concurrency stamp
                };
                await _roleManager.CreateAsync(role);
            }

            await _userManager.AddToRoleAsync(user, UserRoles.User);

            return Ok(new Response(
                "User created successfully.",
                null,
                HttpStatusCode.OK
            ));
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new Response(null, new List<string> { "User already exists." }, HttpStatusCode.InternalServerError));
            }

            var user = new User
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new Response(null, new List<string> { "User creation failed. Please check the details and try again." }, HttpStatusCode.InternalServerError));
            }

            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                var role = new IdentityRole(UserRoles.Admin)
                {
                    ConcurrencyStamp = Guid.NewGuid().ToString() // Ensure concurrency stamp
                };
                await _roleManager.CreateAsync(role);
            }


            await _userManager.AddToRoleAsync(user, UserRoles.Admin);

            return Ok(new Response(
                "Admin user created successfully.",
                null,
                HttpStatusCode.OK
            ));
        }
    }
}
