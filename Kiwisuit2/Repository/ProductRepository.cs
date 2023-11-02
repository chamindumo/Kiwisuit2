using Kiwisuit2.Data;
using Kiwisuit2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System.Net.Http;
using DbContext = Kiwisuit2.Data.DbContext;

namespace Kiwisuit2.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly DbContext _dbContext;
        private readonly HttpClient _httpClient;

        public ProductRepository(DbContext dbContext, HttpClient httpClient)
        {
            _dbContext = dbContext;
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _dbContext.Products.Find(_ => true).ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(string id)
        {
            return await _dbContext.Products.Find(p => p.ProductId == id).FirstOrDefaultAsync();
        }

        public async Task CreateProductAsync(Product product)
        {
            await _dbContext.Products.InsertOneAsync(product);
        }

        public async Task<bool> UpdateProductAsync(string id, Product updatedUser)
        {
            var filter = Builders<Product>.Filter.Eq(u => u.ProductId, id);
            var update = Builders<Product>.Update
                .Set(u => u.ProductId, updatedUser.ProductId)
                .Set(u => u.ExpirDate, updatedUser.ExpirDate)
                .Set(u => u.Name, updatedUser.Name)
                .Set(u => u.Price, updatedUser.Price)
                .Set(u => u.IsAvalable, updatedUser.IsAvalable)

                .Set(u => u.ExpirDate, updatedUser.ExpirDate)
                .Set(u => u.ImageData, updatedUser.ImageData)

                .Set(u => u.Description, updatedUser.Description);

            var updateResult = await _dbContext.Products.UpdateOneAsync(filter, update);

            if (updateResult.ModifiedCount > 0)
            {
                return true; // User was successfully updated
            }

            return false; // User with the given username was not fou
        }

        public async Task<bool> DeleteProductAsync(string productId)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.ProductId, productId);
            var deleteResult = await _dbContext.Products.DeleteOneAsync(filter);

            if (deleteResult.DeletedCount > 0)
            {
                return true; // Product was successfully deleted
            }

            return false; // Product with the given productId was not found
        }

        public async Task<bool> PostToFacebookAsync(string content, string type)
        {
            var accessToken = "EAAODFjzI1MQBOwT6KlW87yHxuyES3VpoetQw4VTxObAORH9bZC6RxOnSJzXcKrPDqweOkwRahxJne6Nv4lSkvWcEU5Isqs8lW67ZC6ZC5TT3gu06mVcJcO2IZC0bABQOLvsaQpbhCOhZBCniEDSAK4XVV5ZCjUES6AdRluUCXf8ylrmLxXFaYpmFZAf997l1s7lUZATgAJEZD";
            var postParameters = new Dictionary<string, string>
    {
        { "access_token", accessToken }
    };

            if (type == "message")
            {
                postParameters["message"] = content;
            }
            else if (type == "link")
            {
                postParameters["link"] = content;
            }
            else
            {
                // Handle unsupported post type or provide a default action
            }

            using (var httpClient = new HttpClient())
            {
                var requestUri = "https://graph.facebook.com/v18.0/me/feed";
                var postContent = new FormUrlEncodedContent(postParameters);

                var response = await httpClient.PostAsync(requestUri, postContent);

                if (response.IsSuccessStatusCode)
                {
                    return true; // Post was successfully created on Facebook
                }
            }

            return false; // Posting to Facebook failed
        }


        public async Task<bool> PostLinkAndMessageToFacebookAsync(string message,string link)
        {
            var accessToken = "EAAODFjzI1MQBOwT6KlW87yHxuyES3VpoetQw4VTxObAORH9bZC6RxOnSJzXcKrPDqweOkwRahxJne6Nv4lSkvWcEU5Isqs8lW67ZC6ZC5TT3gu06mVcJcO2IZC0bABQOLvsaQpbhCOhZBCniEDSAK4XVV5ZCjUES6AdRluUCXf8ylrmLxXFaYpmFZAf997l1s7lUZATgAJEZD";
            var postParameters = new Dictionary<string, string>
                    {
                        { "message", message },
                         { "link", link }

                    };
 
            using (var httpClient = new HttpClient())
            {
                var requestUri = $"https://graph.facebook.com/v18.0/me/feed?access_token={accessToken}";
                var content = new FormUrlEncodedContent(postParameters);

                var response = await httpClient.PostAsync(requestUri, content);

                if (response.IsSuccessStatusCode)
                {
                    return true; // Post was successfully created on Facebook
                }
            }

            return false; // Posting to Facebook failed
        }




    }


}
