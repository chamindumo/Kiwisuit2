using Kiwisuit2.Data;
using Kiwisuit2.DTO;
using Kiwisuit2.Models;
using Kiwisuit2.Repository;
using Kiwisuit2.Service;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Kiwisuit2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productRepository;

        public ProductController(IProductService productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var data = await _productRepository.GetAllAsync();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string productId)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
            {
                return NotFound(); // Return 404 if the product is not found
            }
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct( ProductDTO product)
        {
            
           await _productRepository.CreateAsync(product);
            return Ok(product);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(string id, [FromBody] ProductDTO updatedProduct)
        {
            await _productRepository.UpdateAsync(id, updatedProduct);
            return Ok(updatedProduct);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<string>> DeleteProduct(string id)
        {
            await _productRepository.DeleteAsync(id);
            return Ok(id);

        }
    }
}
