using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Kiwisuit2.DTO
{
    public class ProductDTO
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
    }
}
