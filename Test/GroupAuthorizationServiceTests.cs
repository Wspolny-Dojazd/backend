using Application.Exceptions;
using Application.Services;
using Domain.Interfaces;
using Domain.Model;
using Moq;
using Shared.Enums.ErrorCodes;

namespace Test;

[TestFixture]
public class GroupAuthorizationServiceTests
{
    private Mock<IGroupRepository> _groupRepositoryMock;
    private GroupAuthorizationService _authorizationService;

    [SetUp]
    public void SetUp()
    {
        _groupRepositoryMock = new Mock<IGroupRepository>();
        _authorizationService = new GroupAuthorizationService(_groupRepositoryMock.Object);
    }

    private Group CreateDummyGroup(int id, Guid creatorId)
{
    return new Group
    {
        Id = id,
        JoiningCode = "ABC123",
        CreatorId = creatorId,
        GroupMembers = new List<User>()  
    };
}

    [Test]
    public async Task EnsureMembershipAsync_UserIsMember_DoesNotThrow()
    {
        int groupId = 1;
        Guid userId = Guid.NewGuid();

        _groupRepositoryMock.Setup(r => r.GetByIdAsync(groupId))
            .ReturnsAsync(CreateDummyGroup(groupId, userId));
        _groupRepositoryMock.Setup(r => r.HasMemberAsync(groupId, userId))
            .ReturnsAsync(true);

        await _authorizationService.EnsureMembershipAsync(groupId, userId);
    }

    [Test]
    public void EnsureMembershipAsync_UserIsNotMember_ThrowsAppException()
    {
        int groupId = 1;
        Guid userId = Guid.NewGuid();

        _groupRepositoryMock.Setup(r => r.GetByIdAsync(groupId))
            .ReturnsAsync(CreateDummyGroup(groupId, userId));
        _groupRepositoryMock.Setup(r => r.HasMemberAsync(groupId, userId))
            .ReturnsAsync(false);

        var ex = Assert.ThrowsAsync<AppException>(() =>
            _authorizationService.EnsureMembershipAsync(groupId, userId));

        Assert.That(ex.StatusCode, Is.EqualTo(403));
        Assert.That(ex.Code, Is.EqualTo(GroupErrorCode.ACCESS_DENIED));
    }

    [Test]
    public void EnsureMembershipAsync_GroupNotFound_ThrowsGroupNotFoundException()
    {
        int groupId = 123;
        Guid userId = Guid.NewGuid();

        _groupRepositoryMock.Setup(r => r.GetByIdAsync(groupId))
            .ReturnsAsync((Group?)null);

        Assert.ThrowsAsync<GroupNotFoundException>(() =>
            _authorizationService.EnsureMembershipAsync(groupId, userId));
    }

    [Test]
    public async Task EnsureOwnershipAsync_UserIsOwner_DoesNotThrow()
    {
        int groupId = 2;
        Guid userId = Guid.NewGuid();

        _groupRepositoryMock.Setup(r => r.GetByIdAsync(groupId))
            .ReturnsAsync(CreateDummyGroup(groupId, userId));
        _groupRepositoryMock.Setup(r => r.IsOwnerAsync(groupId, userId))
            .ReturnsAsync(true);

        await _authorizationService.EnsureOwnershipAsync(groupId, userId);
    }

    [Test]
    public void EnsureOwnershipAsync_UserIsNotOwner_ThrowsAppException()
    {
        int groupId = 2;
        Guid userId = Guid.NewGuid();

        _groupRepositoryMock.Setup(r => r.GetByIdAsync(groupId))
            .ReturnsAsync(CreateDummyGroup(groupId, userId));
        _groupRepositoryMock.Setup(r => r.IsOwnerAsync(groupId, userId))
            .ReturnsAsync(false);

        var ex = Assert.ThrowsAsync<AppException>(() =>
            _authorizationService.EnsureOwnershipAsync(groupId, userId));

        Assert.That(ex.StatusCode, Is.EqualTo(403));
        Assert.That(ex.Code, Is.EqualTo(GroupErrorCode.ACCESS_DENIED));
    }

    [Test]
    public void EnsureOwnershipAsync_GroupNotFound_ThrowsGroupNotFoundException()
    {
        int groupId = 55;
        Guid userId = Guid.NewGuid();

        _groupRepositoryMock.Setup(r => r.GetByIdAsync(groupId))
            .ReturnsAsync((Group?)null);

        Assert.ThrowsAsync<GroupNotFoundException>(() =>
            _authorizationService.EnsureOwnershipAsync(groupId, userId));
    }
}
