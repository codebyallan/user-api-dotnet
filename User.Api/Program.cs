using Microsoft.AspNetCore.Identity;
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
builder.Services.AddOpenApi();
builder.Services.Configure<MongoDbConfigurations>(builder.Configuration.GetSection("MongoDbConfigurations"));
builder.Services.AddSingleton<MongoDbContext>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<NotificationContext>();
builder.Services.AddScoped<IPasswordHasher<User.Api.Domain.Entities.User>, PasswordHasher<User.Api.Domain.Entities.User>>();
builder.Services.AddScoped<IHashPasswordService, HashPasswordService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Endpoints
app.MapUserEndpoints();

app.Run();
