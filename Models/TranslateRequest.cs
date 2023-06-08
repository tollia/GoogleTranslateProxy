namespace GoogleTranslateProxy.Models
{
    public class TranslateRequest
    {
        public IEnumerable<string> Text { get; set; }
        public string SourceLanguage { get; set; }
        public string TargetLanguage { get; set; }
    }
}
