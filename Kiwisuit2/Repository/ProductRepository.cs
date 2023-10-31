using Kiwisuit2.Data;
using Kiwisuit2.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Kiwisuit2.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly DbContext _dbContext;

        public ProductRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _dbContext.Products.Find(_ => true).ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(string id)
        {
            if (!ObjectId.TryParse(id, out ObjectId objectId))
            {
                return null; // Return null for an invalid ID
            }

            return await _dbContext.Products.Find(p => p.Id == objectId).FirstOrDefaultAsync();
        }

        public async Task CreateProductAsync(Product product)
        {
            await _dbContext.Products.InsertOneAsync(product);
        }



        public async Task UpdateProductAsync(string id, Product updatedProduct)
        {
            if (!ObjectId.TryParse(id, out ObjectId objectId))
            {
                return; // Do nothing for an invalid ID
            }

            // Remove the _id field from the updatedProduct
            updatedProduct.Id = objectId;

            var filter = Builders<Product>.Filter.Eq(p => p.Id, objectId);
            await _dbContext.Products.ReplaceOneAsync(filter, updatedProduct);
        }

        public async Task DeleteProductAsync(string id)
        {
            if (!ObjectId.TryParse(id, out ObjectId objectId))
            {
                return; // Do nothing for an invalid ID
            }

            await _dbContext.Products.DeleteOneAsync(p => p.Id == objectId);
        }
    }
}
