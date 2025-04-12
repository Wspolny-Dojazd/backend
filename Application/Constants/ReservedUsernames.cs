namespace PublicTransportService.Application.Constants;

/// <summary>
/// Contains reserved usernames that cannot be used for registration.
/// </summary>
internal class ReservedUsernames
{
    /// <summary>
    /// A set of reserved usernames that are not allowed for registration.
    /// </summary>
    public static readonly HashSet<string> List = new(StringComparer.OrdinalIgnoreCase)
    {
        "admin", "auth", "api", "system", "login", "logout", "register", "support", "me", "root", "help",
    };
}
