using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi;
using User.Api.Application.Interfaces;
using User.Api.Application.Notifications;
using User.Api.Application.Services;
using User.Api.Configurations;
using User.Api.Domain.Interfaces;
using User.Api.Endpoints;
using User.Api.Persistence;

var builder = WebApplication.CreateBuilder(args);

MongoDbContext.Configure();

// Add services to the container.
builder.Services.Configure<MongoDbConfigurations>(builder.Configuration.GetSection("MongoDbConfigurations"));
builder.Services.AddSingleton<MongoDbContext>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<NotificationContext>();
builder.Services.AddScoped<IPasswordHasher<User.Api.Domain.Entities.User>, PasswordHasher<User.Api.Domain.Entities.User>>();
builder.Services.AddScoped<IHashPasswordService, HashPasswordService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "User Api",
        Description = "An ASP.NET Core Web API for managing Users"
    });
});
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        options.SlidingExpiration = true;
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Events.OnRedirectToLogin = context =>
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.CompletedTask;
        };
        options.Events.OnRedirectToAccessDenied = context =>
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            return Task.CompletedTask;
        };
    });
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

// Auth
app.UseAuthentication();
app.UseAuthorization();

// Endpoints
app.MapUserEndpoints();
app.MapAuthEndpoints();

app.Run();
