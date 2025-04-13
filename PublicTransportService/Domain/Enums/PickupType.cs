namespace PublicTransportService.Domain.Enums;

/// <summary>
/// Specifies the type of pickup available at a stop.
/// </summary>
public enum PickupType : byte
{
    /// <summary>
    /// Regularly scheduled pickup (default).
    /// </summary>
    RegularlyScheduled = 0,

    /// <summary>
    /// No pickup available.
    /// </summary>
    NotAvailable = 1,

    /// <summary>
    /// Must phone agency to arrange pickup.
    /// </summary>
    ArrangeByPhone = 2,

    /// <summary>
    /// Must coordinate with driver to arrange pickup.
    /// </summary>
    OnDemand = 3,
}
