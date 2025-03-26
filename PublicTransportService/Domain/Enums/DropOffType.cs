namespace PublicTransportService.Domain.Enums;

/// <summary>
/// Specifies the type of drop-off available at a stop, based on GTFS specification.
/// </summary>
public enum DropOffType
{
    /// <summary>
    /// Regularly scheduled drop-off (default).
    /// </summary>
    RegularlyScheduled = 0,

    /// <summary>
    /// No drop-off is available at this stop.
    /// </summary>
    NotAvailable = 1,

    /// <summary>
    /// Drop-off must be arranged by calling the agency.
    /// </summary>
    ArrangeByPhone = 2,

    /// <summary>
    /// Drop-off must be arranged by speaking to the driver.
    /// </summary>
    OnDemand = 3,
}
