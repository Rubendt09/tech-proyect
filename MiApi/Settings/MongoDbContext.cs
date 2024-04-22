using Microsoft.Extensions.Options;
using MongoDB.Driver;

public class MongoDBContext
{
    private readonly IMongoDatabase _database;

    public MongoDBContext(IOptions<MongoDbSettings> settings, IMongoClient mongoClient)
    {
        _database = mongoClient.GetDatabase(settings.Value.DatabaseName);
    }

    public IMongoCollection<T> GetCollection<T>(string name)
    {
        return _database.GetCollection<T>(name);
    }
}
