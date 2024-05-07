using InterestingBlogWebApp.Application.Interfaces;
using InterestingBlogWebApp.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCustomServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

app.UseStaticFiles();
app.UseRouting();

app.MapControllers();

app.MapIdentityApi<ApplicationUser>();
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
