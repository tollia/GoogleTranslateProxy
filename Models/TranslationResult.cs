using System.Text.Json.Serialization;

namespace GoogleTranslateProxy.Models
{
    public class TranslationResult
    {
        [JsonPropertyName("data")]
        public TranslationData Data { get; set; }

        [JsonPropertyName("error")]
        public TranslationError Error { get; set; }
    }

    public class TranslationData
    {
        [JsonPropertyName("translations")]
        public List<Translation> Translations { get; set; } = new List<Translation>();
    }

    public class Translation
    {
        [JsonPropertyName("translatedText")]
        public string TranslatedText { get; set; }

        [JsonPropertyName("detectedSourceLanguage")]
        public string? DetectedSourceLanguage { get; set; }
    }

    public class TranslationError
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("errors")]
        public List<Error> Errors { get; set; }

        [JsonPropertyName("details")]
        public List<ErrorDetails> ErrorDetails { get; set; }
    }

    public class Error
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("domain")]
        public string Domain { get; set; }

        [JsonPropertyName("reason")]
        public string Reason { get; set; }
    }

    public class ErrorDetails
    {
        [JsonPropertyName("@type")]
        public string AtType { get; set; }

        [JsonPropertyName("fieldViolations")]
        public List<FieldViolations> Violations { get; set; }
    }

    public class FieldViolations
    {
        [JsonPropertyName("field")]
        public string Field { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
}
