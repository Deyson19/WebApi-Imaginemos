namespace WebApi_Imaginemos
{
    public class Link
    {
        public string? Href { get; set; }
        public string? Rel { get; set; }
        public string? Title { get; set; }
    }

    public static class Helper
    {
        public static string baseUrl = "https://localhost:7167";
    }
}