namespace BookOrbit.Domain.Common.ValueObjects
{
    public record Url
    {
        public string Value { get; }
        private Url(string value)
        {
            Value = value;
        }
        public static Result<Url> Create(string url)
        {

            if (string.IsNullOrWhiteSpace(url))
                return UrlErrors.RequiredUrl;
            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
                return UrlErrors.InvalidUrl;
            return new Url(url);
        }
    }

    public static class UrlErrors
    {
        private const string className = nameof(Url);


        public static Error InvalidUrl =
            CommonErrors.InvalidProp(className, "Value", "URL", "It must be a valid absolute URL.");

        public static Error RequiredUrl = 
            CommonErrors.RequiredProp(className, "Value", "URL");
    }
}
