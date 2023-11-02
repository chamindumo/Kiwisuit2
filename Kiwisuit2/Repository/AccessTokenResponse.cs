using Newtonsoft.Json;

namespace Kiwisuit2.Repository
{
    public class AccessTokenResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
    }
}