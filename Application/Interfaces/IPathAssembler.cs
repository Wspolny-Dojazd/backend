using Application.DTOs.Path;
using PublicTransportService.Application.PathFinding;
using PublicTransportService.Domain.Entities;

namespace Application.Interfaces;

/// <summary>
/// Defines a contract for assembling user paths from raw path planning results.
/// </summary>
public interface IPathAssembler
{
    /// <summary>
    /// Assembles user paths based on path planning results and stop metadata.
    /// </summary>
    /// <param name="paths">The computed path results, mapped by user ID.</param>
    /// <param name="stopLookup">A lookup containing stop metadata.</param>
    /// <returns>The assembled user paths.</returns>
    Task<IEnumerable<UserPathDto>> AssemblePaths(
        Dictionary<Guid, PathResult> paths,
        IReadOnlyDictionary<string, Stop> stopLookup);
}
