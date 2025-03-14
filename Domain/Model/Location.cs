﻿namespace Domain.Model;

/// <summary>
/// Represents user's current location.
/// </summary>
public class Location
{
    /// <summary>
    /// Gets or sets the unique identifier of location.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the identifier of user.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Gets or sets location's lattitude.
    /// </summary>
    public int Lat { get; set; }

    /// <summary>
    /// Gets or sets location's longitude.
    /// </summary>
    public int Lon { get; set; }

    /// <summary>
    /// Gets or sets user.
    /// </summary>
    public User User { get; set; }
}
