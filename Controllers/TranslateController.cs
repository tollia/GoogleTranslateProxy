using GoogleTranslateProxy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

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
        public async Task<IActionResult> Translate([FromBody] TranslateRequest request)
        {
            var accessToken = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var targetUrl = "https://translation.googleapis.com/language/translate/v2";
            var httpClient = _httpClientFactory.CreateClient();

            // Set up the request to the Google Translate API
            var translationRequest = new HttpRequestMessage(HttpMethod.Post, targetUrl);
            translationRequest.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            translationRequest.Headers.Add("X-Goog-Api-Key", _googleTranslateApiKey);
            translationRequest.Content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("q", request.Text),
                new KeyValuePair<string, string>("source", request.SourceLanguage),
                new KeyValuePair<string, string>("target", request.TargetLanguage),
                new KeyValuePair<string, string>("format", "text")
            });

            // Send the request and get the response
            var translationResponse = await httpClient.SendAsync(translationRequest);
            var translationResult = await translationResponse.Content.ReadAsStringAsync();

            return Ok(translationResult);
        }

        [HttpGet("Token")]
        [AllowAnonymous]
        public IActionResult Token(string secret)
        {
            IConfigurationSection accessTokenSection = _configuration.GetSection("AcccessToken");
            AccessToken accessToken = new AccessToken(
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