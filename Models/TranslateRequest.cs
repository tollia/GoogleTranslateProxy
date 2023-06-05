namespace GoogleTranslateProxy.Models
{
    public class TranslateRequest
    {
        public string Text { get; set; }
        public string SourceLanguage { get; set; }
        public string TargetLanguage { get; set; }
    }
}
