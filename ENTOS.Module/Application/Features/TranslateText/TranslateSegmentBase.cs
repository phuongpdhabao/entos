namespace ENTOS.Application.Features.TranslateText
{
    public class TranslateSegmentBase
    {
        public int Id { get; set; }
        public string TranslatedText { get; set; }
        public string OriginalText { get; set; }

    }

    public class TranslateSegmentBlock
    {
        public int StartIndex { get; set; }

        public List<TranslateSegmentBase> TranslateSegments { get; set; }

        public string TranslatedTextBlock { get; set; }

        public int Index { get; set; }

        public bool IsTranslated { get; set; }
    }
}
