using System.Diagnostics.CodeAnalysis;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using Venice.Orders.Domain.Entities;

namespace Venice.Orders.Infrastructure.DataProviders.Contexts;

[ExcludeFromCodeCoverage]
public static class MongoDbMappers
{
    public static void RegisterClassMaps()
    {
        BsonClassMap.RegisterClassMap<OrderItem>(cm =>
        {
            cm.AutoMap();
            cm.MapIdProperty(x => x.Id)
                .SetIdGenerator(StringObjectIdGenerator.Instance)
                .SetSerializer(new StringSerializer(BsonType.ObjectId));
        });
    }
}