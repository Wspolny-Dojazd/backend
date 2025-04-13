public class CamelCaseEnumConverter : JsonStringEnumConverter
{
    public CamelCaseEnumConverter()
        : base(JsonNamingPolicy.CamelCase)
    {
    }
}