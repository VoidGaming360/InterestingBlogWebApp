using InterestingBlogWebApp.Application.Interfaces;
using InterestingBlogWebApp.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using InterestingBlogWebApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("ApplicationDbContextConnection") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContextConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddCustomServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


app.UseStaticFiles();


app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.MapGet("/test", (ClaimsPrincipal user) => $"Hello {user.Identity!.Name}").RequireAuthorization();

DataSeeding(app);

app.Run();

static void DataSeeding(WebApplication app) // Pass app instance to DataSeeding
{
    using var scope = app.Services.CreateScope();
    var dbInitializer = scope.ServiceProvider
        .GetRequiredService<IDbInitializer>();
    dbInitializer.Initialize();
}
