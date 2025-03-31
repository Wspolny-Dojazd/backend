using System.Text.RegularExpressions;

namespace API.Conventions;

/// <summary>
/// Transforms parameter names to kebab-case for use in route templates.
/// </summary>
public class KebabCaseParameterTransformer : IOutboundParameterTransformer
{
    /// <inheritdoc/>
    public string? TransformOutbound(object? value)
    {
        return value != null
            ? Regex.Replace(value.ToString()!, "([a-z])([A-Z])", "$1-$2").ToLower()
            : null;
    }
}
