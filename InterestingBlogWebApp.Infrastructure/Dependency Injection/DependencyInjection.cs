using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using InterestingBlogWebApp.Application.Common.Interface.IServices;
using InterestingBlogWebApp.Infrastructure.Persistence;
using InterestingBlogWebApp.Infrastructure.Services;
using InterestingBlogWebApp.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using InterestingBlogWebApp.Application.Common.Interface.IRepositories;
using InterestingBlogWebApp.Infrastructures.Repositories;
using InterestingBlogWebApp.Domain.Auth;
using InterestingBlogWebApp.Application.Common_Interfaces.IServices;
using BisleriumProject.Infrastructures.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("dev"),
            b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)), ServiceLifetime.Transient);


        services.AddIdentity<User, IdentityRole>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
        })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        //services.AddScoped<IAppDbContext>(provider => provider.GetService<AppDbContext>());

        services.Configure<CloudinarySettings>(configuration.GetSection("CloudinarySettings"));


        services.AddDbContext<AppDbContext>();


        services.AddTransient(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IBlogRepository, BlogRepository>();
        services.AddTransient<ICommentRepository, CommentRepository>();
        services.AddTransient<IBlogReactionRepository, BlogReactionRepository>();
        services.AddTransient<IBlogRecordRepository, BlogRecordRepository>();
        services.AddTransient<ICommentReactionRepository, CommentReactionRepository>();
        services.AddTransient<ICommentRecordRepository, CommentRecordRepository>();

        //services.AddTransient<IAuthenticationService, AuthenticationService>();


        services.AddTransient<IEmailService, EmailService>();
        services.AddTransient<IEmailServices, EmailServices>();
        services.AddTransient<IBlogService, BlogService>();
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<ICommentService, CommentService>();
        services.AddTransient<IBlogReactionService, BlogReactionService>();
        services.AddTransient<IBlogRecordService, BlogRecordService>();
        services.AddTransient<ICommentReactionService, CommentReactionService>();
        services.AddTransient<ICommentRecordService, CommentRecordService>();
        services.AddTransient<IAdminDashboardService, AdminService>();
        services.AddTransient<INotificationService, NotificationService>();

        services.AddSingleton<EmailConfiguration>();

        return services;
    }
}
