using Application.DTOs;
using Application.Interfaces;
using Application.Services;
using AutoMapper;
using Domain.Interfaces;
using Domain.Model;
using Moq;

namespace Test;

[TestFixture]
public class GroupServiceTests
{
    private Mock<IGroupRepository> _groupRepositoryMock = null!;
    private Mock<IUserService> _userServiceMock = null!;
    private Mock<IMapper> _mapperMock = null!;
    private GroupService _groupService = null!;

    [SetUp]
    public void Setup()
    {
        _groupRepositoryMock = new Mock<IGroupRepository>();
        _userServiceMock = new Mock<IUserService>();
        _mapperMock = new Mock<IMapper>();

        _groupService = new GroupService(
            _groupRepositoryMock.Object,
            _userServiceMock.Object,
            _mapperMock.Object);
    }

    private User CreateDummyUser(Guid id)
    {
        return new User
        {
            Id = id,
            Username = "testuser",
            Email = "test@example.com",
            Nickname = "Tester",
            PasswordHash = "hash",
            RefreshToken = "token",
            RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(1),
            CreatedAt = DateTime.UtcNow,
            Friends = new List<User>(),
            Groups = new List<Group>(),
            UserConfiguration = new UserConfiguration()
        };
    }

    [Test]
    public async Task RemoveUserAsync_UserIsCreator_RemovesGroupAndReturnsNull()
    {
        var groupId = 1;
        var userId = Guid.NewGuid();
        var user = CreateDummyUser(userId);

        var group = new Group
        {
            Id = groupId,
            CreatorId = userId,
            JoiningCode = "JOIN123",
            GroupMembers = new List<User> { user }
        };

        _groupRepositoryMock.Setup(r => r.GetByIdAsync(groupId)).ReturnsAsync(group);
        _userServiceMock.Setup(u => u.GetEntityByIdAsync(userId)).ReturnsAsync(user);

        var result = await _groupService.RemoveUserAsync(groupId, userId);

        _groupRepositoryMock.Verify(r => r.RemoveAsync(group), Times.Once);
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task RemoveUserAsync_UserIsNotCreator_RemovesUserAndReturnsDto()
    {
        var groupId = 1;
        var creatorId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var user = CreateDummyUser(userId);

        var group = new Group
        {
            Id = groupId,
            CreatorId = creatorId,
            JoiningCode = "JOIN123",
            GroupMembers = new List<User> { user }
        };

        var groupDto = new GroupDto(group.Id, group.JoiningCode, new List<GroupMemberDto>());

        _groupRepositoryMock.Setup(r => r.GetByIdAsync(groupId)).ReturnsAsync(group);
        _userServiceMock.Setup(u => u.GetEntityByIdAsync(userId)).ReturnsAsync(user);
        _mapperMock.Setup(m => m.Map<Group, GroupDto>(group)).Returns(groupDto);

        var result = await _groupService.RemoveUserAsync(groupId, userId);

        _groupRepositoryMock.Verify(r => r.RemoveUserAsync(group, user), Times.Once);
        Assert.That(result, Is.EqualTo(groupDto));
    }

    [Test]
    public async Task IsInGroup_UserIsMember_ReturnsTrue()
    {
        var groupId = 2;
        var userId = Guid.NewGuid();
        var user = CreateDummyUser(userId);

        var group = new Group
        {
            Id = groupId,
            CreatorId = Guid.NewGuid(),
            JoiningCode = "XYZ",
            GroupMembers = new List<User> { user }
        };

        _groupRepositoryMock.Setup(r => r.GetByIdAsync(groupId)).ReturnsAsync(group);

        var result = await _groupService.IsInGroup(groupId, userId);

        Assert.IsTrue(result);
    }

    [Test]
    public async Task IsInGroup_UserNotInGroup_ReturnsFalse()
    {
        var groupId = 3;
        var userId = Guid.NewGuid();

        var group = new Group
        {
            Id = groupId,
            CreatorId = Guid.NewGuid(),
            JoiningCode = "XYZ",
            GroupMembers = new List<User>()
        };

        _groupRepositoryMock.Setup(r => r.GetByIdAsync(groupId)).ReturnsAsync(group);

        var result = await _groupService.IsInGroup(groupId, userId);

        Assert.IsFalse(result);
    }
}