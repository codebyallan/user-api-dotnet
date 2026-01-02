using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using User.Api.Configurations;
using User.Api.Domain.ValueObjects;
using User.Api.Persistence.Serializers;

namespace User.Api.Persistence;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;
    public MongoDbContext(IOptions<MongoDbConfigurations> _configuration)
    {
        MongoClient client = new(_configuration.Value.ConnectionString);
        _database = client.GetDatabase(_configuration.Value.DatabaseName);
    }
    public static void Configure()
    {
        if (!BsonClassMap.IsClassMapRegistered(typeof(Domain.Entities.User)))
        {
            BsonClassMap.RegisterClassMap<Domain.Entities.User>(classMap =>
        {
            classMap.AutoMap();
            classMap.MapIdProperty(c => c.Id)
                    .SetIdGenerator(GuidGenerator.Instance)
                    .SetSerializer(new GuidSerializer(GuidRepresentation.Standard));
            classMap.MapMember(u => u.CreatedAt).SetElementName("created_at");
            classMap.MapMember(u => u.UpdatedAt).SetElementName("updated_at").SetIgnoreIfNull(true);
            classMap.MapMember(u => u.DeletedAt).SetElementName("deleted_at").SetIgnoreIfNull(true);

        });
        }
        BsonSerializer.TryRegisterSerializer(new EmailSerializer());
        BsonSerializer.TryRegisterSerializer(new PasswordSerializer());

    }
    public IMongoCollection<Domain.Entities.User> Users => _database.GetCollection<Domain.Entities.User>("users");
}
