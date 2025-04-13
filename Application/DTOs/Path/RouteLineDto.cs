using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using PublicTransportService.Domain.Enums;

namespace Application.DTOs.Path;

/// <summary>
/// Represents a data transfer object used for route lines.
/// </summary>
/// <param name="Type">The type of the route line.</param>
/// <param name="ShortName">The short name of the route line (e.g., line number).</param>
/// <param name="LongName">The full name or description of the route line.</param>
/// <param name="HeadSign">The head sign of the route line, indicating the direction.</param>
/// <param name="Color">The background color of the route in hexadecimal RRGGBB format.</param>
/// <param name="TextColor">The text color to be used on the route's background in hexadecimal RRGGBB format.</param>
public record RouteLineDto(
    [property: Required] RouteType Type,
    [property: Required] string ShortName,
    [property: Required] string LongName,
    [property: Required] string HeadSign,
    [property: Required] string Color,
    [property: Required] string TextColor);
