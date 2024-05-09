using InterestingBlogWebApp.Application.Interfaces;
using InterestingBlogWebApp.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using InterestingBlogWebApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("ApplicationDbContextConnection") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContextConnection' not found.");

<<<<<<< HEAD
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
=======
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddCustomServices(builder.Configuration);
>>>>>>> 6d36ad5f803454f02477edef962546858f674053

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

<<<<<<< HEAD
app.UseAuthorization();
<<<<<<< Updated upstream
=======
>>>>>>> 6d36ad5f803454f02477edef962546858f674053
app.UseAuthentication();
app.UseAuthorization();


app.UseStaticFiles();
=======

//app.UseStaticFiles();
>>>>>>> Stashed changes
app.UseRouting();

app.UseCors("_myAllowSpecificOrigins");

app.MapControllers();

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
