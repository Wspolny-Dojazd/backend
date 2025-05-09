using Application.DTOs.Message;
using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Interfaces;
using Domain.Model;
using Shared.Enums.ErrorCodes;

namespace Application.Services;

/// <summary>
/// Provides the message-related operations.
/// </summary>
/// <param name="messageRepository">The repository for accessing the message data.</param>
/// <param name="groupRepository">The repository for accessing the group data.</param>
/// <param name="userRepository">The repository for accessing the user data.</param>
/// <param name="mapper">The object mapper.</param>
public class MessageService(
    IMessageRepository messageRepository,
    IGroupRepository groupRepository,
    IUserRepository userRepository,
    IMapper mapper)
    : IMessageService
{
    /// <inheritdoc/>
    public async Task<IEnumerable<MessageDto>> GetAllByGroupIdAsync(Guid userId, int groupId)
    {
        _ = await this.ValidateUserGroupAccess(userId, groupId);

        var messages = await messageRepository.GetAllByGroupIdAsync(groupId);

        return messages.Select(mapper.Map<Message, MessageDto>);
    }

    /// <inheritdoc/>
    public async Task<MessageDto> SendAsync(Guid userId, int groupId, string content)
    {
        _ = await this.ValidateUserGroupAccess(userId, groupId);

        if (string.IsNullOrWhiteSpace(content))
        {
            throw new AppException(400, MessageErrorCode.EMPTY_MESSAGE);
        }

        var message = new Message()
        {
            GroupId = groupId,
            UserId = userId,
            CreatedAt = DateTime.UtcNow,
            Content = content,
        };

        await messageRepository.AddAsync(message);
        return mapper.Map<Message, MessageDto>(message);
    }

    private async Task<(User ValidatedUser, Group ValidatedGroup)> ValidateUserGroupAccess(
        Guid userId, int groupId)
    {
        var user = await userRepository.GetByIdAsync(userId)
            ?? throw new UserNotFoundException(userId);

        var group = await groupRepository.GetByIdAsync(groupId)
            ?? throw new GroupNotFoundException(groupId);

        return group.GroupMembers.Any(m => m.Id == userId)
            ? (user, group)
            : throw new AppException(403, GroupErrorCode.ACCESS_DENIED);
    }
}
