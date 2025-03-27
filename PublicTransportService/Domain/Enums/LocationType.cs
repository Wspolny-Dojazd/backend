namespace PublicTransportService.Domain.Enums;

/// <summary>
/// Specifies the type of physical location within a transit station, based on GTFS.
/// </summary>
public enum LocationType
{
    /// <summary>
    /// A stop or platform (default).
    /// </summary>
    Stop = 0,

    /// <summary>
    /// A station containing one or more stops or platforms.
    /// </summary>
    Station = 1,

    /// <summary>
    /// An entrance or exit to the station.
    /// </summary>
    EntranceExit = 2,

    /// <summary>
    /// A generic node used for linking pathways within the station.
    /// </summary>
    GenericNode = 3,

    /// <summary>
    /// A specific boarding area on a platform.
    /// </summary>
    BoardingArea = 4,
}
