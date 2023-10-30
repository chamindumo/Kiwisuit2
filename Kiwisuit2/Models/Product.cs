using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Kiwisuit2.Models
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public string ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public bool IsAvalable { get; set; }
        public DateTime ExpirDate { get; set; }
        public string ImageData { get; set; }
    }
}