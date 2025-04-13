using System.Text.Json.Serialization;

namespace Application.DTOs.Path;

/// <summary>
/// Represents the base class for different types of path segments.
/// </summary>
/// <remarks>
/// Used for polymorphic serialization/deserialization of segment types.
/// </remarks>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(RouteSegmentDto), nameof(SegmentType.Route))]
[JsonDerivedType(typeof(WalkSegmentDto), nameof(SegmentType.Walk))]
public abstract record SegmentDtoBase;
