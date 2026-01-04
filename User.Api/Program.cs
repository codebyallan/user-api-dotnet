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

// Endpoints
app.MapUserEndpoints();

app.Run();
