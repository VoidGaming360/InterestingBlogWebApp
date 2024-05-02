using Microsoft.EntityFrameworkCore;
using InterestingBlogWebApp.Application.Interfaces;
using InterestingBlogWebApp.Infrastructure.Persistence;
using InterestingBlogWebApp.Infrastructure.Services;
using InterestingBlogWebApp.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IDbInitializer, DbInitializer>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<IBlog, Blog>(); //injecting service

builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<UserManager<ApplicationUser>>();
builder.Services.AddScoped<SignInManager<ApplicationUser>>();


builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

System.Globalization.CultureInfo.DefaultThreadCurrentCulture = new System.Globalization.CultureInfo("en-US");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseStaticFiles();
app.UseRouting();

app.MapControllers();


DataSeeding(app);

app.Run();

static void DataSeeding(WebApplication app) // Pass app instance to DataSeeding
{
    using var scope = app.Services.CreateScope();
    var dbInitializer = scope.ServiceProvider
        .GetRequiredService<IDbInitializer>();
    dbInitializer.Initialize();
}
