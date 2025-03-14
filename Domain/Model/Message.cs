﻿namespace Domain.Model;

/// <summary>
/// Represents a message in a group chat.
/// </summary>
public class Message
{
    /// <summary>
    /// Gets or sets the unique identifier of the message.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the group in which the message was sent.
    /// </summary>
    public int GroupId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user that send the message.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Gets or sets message's content.
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    /// Gets or sets the group in which the message was sent.
    /// </summary>
    public Group Group { get; set; }

    /// <summary>
    /// Gets or sets the user who sent the message.
    /// </summary>
    public User User { get; set; }
}
