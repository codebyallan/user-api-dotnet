using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using User.Api.Domain.ValueObjects;

namespace User.Api.Persistence.Serializers;

public class PasswordSerializer : SerializerBase<Password>
{
    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, Password password)
    {
        if (password is null)
        {
            context.Writer.WriteNull();
            return;
        }
        context.Writer.WriteString(password.Value);
    }

    public override Password Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        if (context.Reader.CurrentBsonType == BsonType.Null)
        {
            context.Reader.ReadNull();
            return null!;
        }
        return new Password(context.Reader.ReadString(), isAlreadyHashed: true);
    }

}
