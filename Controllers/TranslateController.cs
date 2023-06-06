using GoogleTranslateProxy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using static System.Net.Mime.MediaTypeNames;
using System.Runtime.InteropServices;
using System.Text.Unicode;
using System.Text.Json;

namespace GoogleTranslateProxy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TranslateController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _googleTranslateApiKey;
        private readonly IConfiguration _configuration;

        public TranslateController(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _googleTranslateApiKey = _configuration["Google:ApiKey"];
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<TranslationResult?> Translate([FromBody] TranslateRequest request)
        {
            var targetUrl = "https://translation.googleapis.com/language/translate/v2";
            var httpClient = _httpClientFactory.CreateClient();

            // Set up the request to the Google Translate API
            var translationRequest = new HttpRequestMessage(HttpMethod.Post, targetUrl);
            translationRequest.Headers.Add("X-Goog-Api-Key", _googleTranslateApiKey);
            translationRequest.Content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("q", request.Text),
                new KeyValuePair<string, string>("source", request.SourceLanguage),
                new KeyValuePair<string, string>("target", request.TargetLanguage),
                new KeyValuePair<string, string>("format", "text")
            });

            // Send the request and get the response
            HttpResponseMessage translationResponse = await httpClient.SendAsync(translationRequest);
            string responseContent = await translationResponse.Content.ReadAsStringAsync();
            TranslationResult translationResult = JsonSerializer.Deserialize<TranslationResult>(responseContent);
            TranslationData translationData = translationResult.Data;

            // The below block is only here to provide a sample of how to interpret the result.
            if (translationResult?.Data == null)
            {
                // Response could not be Deserialized into the TranslationResult object.
            }
            else if (translationResult.Error != null)
            {
                // Handle the error case.
                var errorCode = translationResult.Error.Code;
                var errorMessage = translationResult.Error.Message;
            }
            else if (translationData.Translations.Count == 0)
            { 
                // There was no translation returned but return code was success.
            }
            else 
            {
                // Access the translations as before.
                var translatedText = translationData.Translations[0].TranslatedText;
                var detectedLanguage = translationData.Translations[0].DetectedSourceLanguage;
            }

            return translationResult;
        }

        [HttpGet("Token")]
        [AllowAnonymous]
        public IActionResult Token(string secret)
        {
            IConfigurationSection accessTokenSection = _configuration.GetSection("AccessToken");
            AccessToken accessToken = new(
                secret,
                accessTokenSection["ValidIssuer"],
                accessTokenSection["ValidAudience"],
                accessTokenSection.GetValue<int>("ExpirationMinutes"),
                Array.Empty<string>()
            );
            return new JsonResult(accessToken.Get());
        }
    }
}