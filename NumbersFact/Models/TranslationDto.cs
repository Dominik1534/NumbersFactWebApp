namespace NumbersFact.Models
{

    public class TranslationDto
    {
        public Data data { get; set; }
    }

    public class Data
    {
        public Translation[] translations { get; set; }
    }

    public class Translation
    {
        public string translatedText { get; set; }
    }

}

