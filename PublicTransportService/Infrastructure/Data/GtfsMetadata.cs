using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PublicTransportService.Infrastructure.Data;

/// <summary>
/// Represents metadata information about the GTFS dataset.
/// </summary>
[Table("pts_gtfs_metadata")]
public class GtfsMetadata
{
    /// <summary>
    /// Gets or sets the primary key of the metadata record.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the timestamp indicating the last update time of the GTFS data.
    /// </summary>
    [Required]
    public DateTime LastUpdated { get; set; }
}
