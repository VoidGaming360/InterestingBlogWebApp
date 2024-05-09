using InterestingBlogWebApp.Application.Interfaces;
using InterestingBlogWebApp.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

<<<<<<< Updated upstream
builder.Services.AddCustomServices();
=======
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddCustomServices(builder.Configuration);
>>>>>>> Stashed changes

const string myAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigins,
          policy =>
          {
              policy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();

          });

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
<<<<<<< Updated upstream
app.UseAuthentication();

app.UseStaticFiles();
=======

//app.UseStaticFiles();
>>>>>>> Stashed changes
app.UseRouting();

app.UseCors("_myAllowSpecificOrigins");

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
