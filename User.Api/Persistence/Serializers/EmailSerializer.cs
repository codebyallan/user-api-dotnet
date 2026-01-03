using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using User.Api.Domain.ValueObjects;

namespace User.Api.Persistence.Serializers;

public class EmailSerializer : SerializerBase<Email>
{
    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, Email value)
    {
        if (value is null)
        {
            context.Writer.WriteNull();
            return;
        }
        context.Writer.WriteString(value.Address);
    }
    public override Email Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        if (context.Reader.CurrentBsonType == BsonType.Null)
        {
            context.Reader.ReadNull();
            return null!;
        }
        return new Email(context.Reader.ReadString());
    }

}
