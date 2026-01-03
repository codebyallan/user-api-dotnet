using User.Api.Application.Interfaces;
using User.Api.Application.Notifications;
using User.Api.Application.Services;
using User.Api.Configurations;
using User.Api.Domain.Interfaces;
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



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();
