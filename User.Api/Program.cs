using User.Api.Configurations;
using User.Api.Persistence;

var builder = WebApplication.CreateBuilder(args);

MongoDbContext.Configure();

// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.Configure<MongoDbConfigurations>(builder.Configuration.GetSection("MongoDbConfigurations"));
builder.Services.AddSingleton<MongoDbContext>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();
