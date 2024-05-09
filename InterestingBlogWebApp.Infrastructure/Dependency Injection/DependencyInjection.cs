using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using InterestingBlogWebApp.Application.Interfaces;
using InterestingBlogWebApp.Infrastructure.Persistence;
using InterestingBlogWebApp.Infrastructure.Services;
using InterestingBlogWebApp.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using InterestingBlogWebApp.Application.Common_Interfaces;
using InterestingBlogWebApp.Infrastructure.Repositories;
using CloudinaryDotNet;

public static class DependencyInjection
{
    public static void AddCustomServices(this IServiceCollection services, IConfiguration configuration)
    {
        string imageFolderPath = configuration["ImageFolderPath"];

        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
            });
            options.OperationFilter<SecurityRequirementsOperationFilter>();
        });

        services.AddSingleton<Cloudinary>(provider =>
        {
            var account = new Account("cloud_name", "api_key", "api_secret"); // Update with actual Cloudinary credentials
            return new Cloudinary(account);
        });

        services.AddHttpContextAccessor();
        services.AddScoped<IDbInitializer, DbInitializer>();
        services.AddTransient<IUnitOfWork, UnitOfWork>();
        services.AddTransient<IBlogRepository, BlogRepository>();
        services.AddTransient<IBlog, BlogService>(provider =>
        {
            var cloudinary = provider.GetRequiredService<Cloudinary>();
            var userManager = provider.GetRequiredService<UserManager<ApplicationUser>>();
            var blogRepository = provider.GetRequiredService<IBlogRepository>();
            return new BlogService(userManager, blogRepository, cloudinary, imageFolderPath);
        });
        services.AddScoped<IUserAccessor, HttpContextUserAccessor>();

        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddSignInManager()
            .AddRoles<IdentityRole>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                };
            });

        services.AddAuthorization();

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        System.Globalization.CultureInfo.DefaultThreadCurrentCulture = new System.Globalization.CultureInfo("en-US");
    }
}
