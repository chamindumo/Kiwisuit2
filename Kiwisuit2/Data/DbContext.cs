using Kiwisuit2.Models;
using MongoDB.Driver;

namespace Kiwisuit2.Data
{
    public class DbContext
    {
        private readonly IMongoDatabase _database;

        public DbContext(IConfiguration configuration)
        {
            // Set your connection string and database name here
            string connectionString = configuration.GetConnectionString("MongoDB");
            string databaseName = configuration.GetConnectionString("DatabaseName");

            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<Product> Products => _database.GetCollection<Product>("Products");

    }
}
