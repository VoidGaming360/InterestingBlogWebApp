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

public static class DependencyInjection
{
    public static void AddCustomServices(this IServiceCollection services, IConfiguration configuration)
    {
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

        // Register application services
        services.AddScoped<IDbInitializer, DbInitializer>();
        services.AddTransient<IUnitOfWork, UnitOfWork>();
        services.AddTransient<IBlog, BlogService>();

        // Register Identity services
        services.AddScoped<UserManager<ApplicationUser>>();
        services.AddScoped<SignInManager<ApplicationUser>>();

        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddSignInManager()
            .AddRoles<IdentityRole>();

        services.AddAuthentication(options=> {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,

                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                IssuerSigningKey = new
            SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!))
            };
        });



        services.AddAuthorizationBuilder();

        // Configure the database context
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(services.BuildServiceProvider().GetRequiredService<IConfiguration>().GetConnectionString("DefaultConnection")));

        // Set default thread culture for localization
        System.Globalization.CultureInfo.DefaultThreadCurrentCulture = new System.Globalization.CultureInfo("en-US");
    }
}