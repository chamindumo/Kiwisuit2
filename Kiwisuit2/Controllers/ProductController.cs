using Geocoding;
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
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductController(IHttpClientFactory httpClientFactory, IProductService productRepository)
        {
            _productRepository = productRepository;
            _httpClientFactory = httpClientFactory;

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
        public async Task<IActionResult> CreateProduct(ProductDTO product)
        {

            await _productRepository.CreateAsync(product);
            return Ok(product);

        }
        [HttpPost("post")]
        public async Task<IActionResult> PostToFacebook([FromBody] string message, string type)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return BadRequest("Message cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(type))
            {
                return BadRequest("type cannot be empty.");
            }

            var success = await _productRepository.PostToFacebookAsync(message, type);

            if (success)
            {
                return Ok("Posted to Facebook successfully.");
            }
            else
            {
                return BadRequest("Failed to post to Facebook.");
            }
        }



        [HttpPost("facebook")]
        public async Task<IActionResult> PostToBothFacebook([FromBody] string message, string link)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return BadRequest("Message cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(link))
            {
                return BadRequest("type cannot be empty.");
            }

            var success = await _productRepository.PostLinkAndMessageToFacebookAsync(message, link);

            if (success)
            {
                return Ok("Posted to Facebook successfully.");
            }
            else
            {
                return BadRequest("Failed to post to Facebook.");
            }
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

        [HttpGet("Google")]
        public async Task<IActionResult> GetCoffeeShopsBetweenLocations(string location1, string location2,string Choice)
        {
            try
            {
                var apiKey = "AIzaSyAoS8LDAxSJ78ycaq2diQewFx7L3d0qUWE";
                var client = _httpClientFactory.CreateClient();
                var encodedLocation1 = Uri.EscapeDataString(location1);
                var encodedLocation2 = Uri.EscapeDataString(location2);
                var encodedChoice = Uri.EscapeDataString(Choice);
                // Make a request to the Google Places API to get coffee shops between the locations
                var url = $"https://maps.googleapis.com/maps/api/place/textsearch/json?query={encodedChoice}+ between+{encodedLocation1}+and+{encodedLocation2}&key={apiKey}";

                var response = await client.GetStringAsync(url);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }


        }


        [HttpGet("Googlelocation2")]
        public async Task<IActionResult> GetCoffeeShopsBetweenLocation(string location2, string location3, string Choice)
        {
            try
            {
                var apiKey = "AIzaSyAoS8LDAxSJ78ycaq2diQewFx7L3d0qUWE";
                var client = _httpClientFactory.CreateClient();

                // Properly format the query parameters and URL encode them
                var encodedLocation2 = Uri.EscapeDataString(location2);
                var encodedLocation3 = Uri.EscapeDataString(location3);
                var encodedChoice = Uri.EscapeDataString(Choice);

                // Make a request to the Google Places API to get coffee shops between the locations
                var url = $"https://maps.googleapis.com/maps/api/place/textsearch/json?query={encodedChoice}+between+{encodedLocation2}+and+{encodedLocation3}&key={apiKey}";

                var response = await client.GetStringAsync(url);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet("goo")]
        public async Task<IActionResult> GetCoffeeShopsBetweenLocations1(string location1, string location2, string choice)
        {
            try
            {
                var apiKey = "YOUR_GOOGLE_MAPS_API_KEY";
                var client = _httpClientFactory.CreateClient();
                var encodedLocation1 = Uri.EscapeDataString(location1);
                var encodedLocation2 = Uri.EscapeDataString(location2);
                var encodedChoice = Uri.EscapeDataString(choice);

                // Use Google Directions API to get the route between the locations
                var directionsUrl = $"https://maps.googleapis.com/maps/api/directions/json?origin={encodedLocation1}&destination={encodedLocation2}&key={apiKey}";
                var directionsResponse = await client.GetStringAsync(directionsUrl);

                // Parse the directions response to get the polyline
                // You may need to install a library like Polyline.Net to decode the polyline
                // For simplicity, assuming you have a method to extract polyline points
                var polylinePoints = ExtractPolylinePoints(directionsResponse);

                // Use Google Places API to find coffee shops along the polyline
                var placesUrl = $"https://maps.googleapis.com/maps/api/place/textsearch/json?query={encodedChoice}+along+polyline:{polylinePoints}&key={apiKey}";
                var placesResponse = await client.GetStringAsync(placesUrl);

                return Ok(placesResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Extract polyline points from the Directions API response
        private List<Location> ExtractPolylinePoints(string directionsResponse)
        {
            // Deserialize the Directions API response to get the polyline points
            var directions = Newtonsoft.Json.JsonConvert.DeserializeObject<DirectionsResponse>(directionsResponse);

            if (directions?.Routes != null && directions.Routes.Count > 0)
            {
                // Assuming you are using the overview polyline of the first route
                var polyline = directions.Routes[0].OverviewPolyline;

                // Decode the polyline using Polyline.Net
                var polylinePoints = Polyline.Decode(polyline.Points);

                // Return the list of Location objects representing the polyline points
                return polylinePoints;
            }

            return null;
        }





    }
}
