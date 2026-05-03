using Microsoft.Extensions.Configuration;
using MyPortfolio.BusinessLayer.Abstract;
using System.Text.Json;
using System.Net.Http;

namespace MyPortfolio.BusinessLayer.Concrete
{
    public class TurnstileService : ITurnstileService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public TurnstileService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<bool> VerifyTokenAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token)) return false;

            var secretKey = _configuration["TurnstileSettings:SecretKey"];
            var client = _httpClientFactory.CreateClient();

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("secret", secretKey),
                new KeyValuePair<string, string>("response", token)
            });

            try
            {
                var response = await client.PostAsync("https://challenges.cloudflare.com/turnstile/v0/siteverify", content);

                if (!response.IsSuccessStatusCode) return false;

                var jsonResult = await response.Content.ReadAsStringAsync();
                using var document = JsonDocument.Parse(jsonResult);

                return document.RootElement.GetProperty("success").GetBoolean();
            }
            catch (Exception)
            {
                // Kurumsal bir yapıda buraya Logger eklenmelidir.
                return false;
            }
        }
    }
}