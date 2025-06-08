using Application.DTOs;
using Application.Exceptions;
using Application.Interfaces;
using Application.Services;
using AutoMapper;
using Domain.Interfaces;
using Domain.Model;
using Moq;
using Shared.Enums.ErrorCodes;

namespace Test
{
    [TestFixture]
    public class FriendServiceTests
    {
        private Mock<IUserService> _userServiceMock;
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IFriendRepository> _friendRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private FriendService _friendService;

        [SetUp]
        public void SetUp()
        {
            // setup mocks for necessary services for every test
            this._userServiceMock = new Mock<IUserService>();
            this._userRepositoryMock = new Mock<IUserRepository>();
            this._friendRepositoryMock = new Mock<IFriendRepository>();
            this._mapperMock = new Mock<IMapper>();
            this._friendService = new FriendService(
                this._userServiceMock.Object,
                this._userRepositoryMock.Object,
                this._friendRepositoryMock.Object,
                this._mapperMock.Object
            );
        }

        [Test]
        public async Task CreateFriendshipAsync_NewFriend_AddsFriends()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var friendId = Guid.NewGuid();

            var user = new User
            {
                Id = userId,
                Username = "User",
                Nickname = "User123",
                Email = "user@example.com",
                PasswordHash = "hash",
                RefreshToken = "refresh",
                RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(1),
                CreatedAt = DateTime.UtcNow,
                Friends = new List<User>(),
                Groups = new List<Group>(),
                UserConfiguration = new UserConfiguration(),
                UserLocation = null
            };

            var friend = new User
            {
                Id = friendId,
                Username = "Friend",
                Nickname = "Friend123",
                Email = "friend@example.com",
                PasswordHash = "hash",
                RefreshToken = "refresh",
                RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(1),
                CreatedAt = DateTime.UtcNow,
                Friends = new List<User>(),
                Groups = new List<Group>(),
                UserConfiguration = new UserConfiguration(),
                UserLocation = null
            };

            this._userServiceMock.Setup(x => x.GetEntityByIdAsync(userId)).ReturnsAsync(user);
            this._userServiceMock.Setup(x => x.GetEntityByIdAsync(friendId)).ReturnsAsync(friend);

            // Act
            await this._friendService.CreateFriendshipAsync(userId, friendId);

            // Assert
            Assert.That(user.Friends, Has.Exactly(1).EqualTo(friend));
            Assert.That(friend.Friends, Has.Exactly(1).EqualTo(user));
            this._userRepositoryMock.Verify(r => r.UpdateAsync(user), Times.Once);
            this._userRepositoryMock.Verify(r => r.UpdateAsync(friend), Times.Once);
        }

        [Test]
        public void CreateFriendshipAsync_AlreadyFriends_ThrowsAppException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var friendId = Guid.NewGuid();

            var friend = new User
            {
                Id = friendId,
                Username = "Friend",
                Nickname = "Friend123",
                Email = "friend@example.com",
                PasswordHash = "hash",
                RefreshToken = "refresh",
                RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(1),
                CreatedAt = DateTime.UtcNow,
                Friends = new List<User>(),
                Groups = new List<Group>(),
                UserConfiguration = new UserConfiguration(),
                UserLocation = null
            };

            var user = new User
            {
                Id = userId,
                Username = "User",
                Nickname = "User123",
                Email = "user@example.com",
                PasswordHash = "hash",
                RefreshToken = "refresh",
                RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(1),
                CreatedAt = DateTime.UtcNow,
                Friends = new List<User> { friend },
                Groups = new List<Group>(),
                UserConfiguration = new UserConfiguration(),
                UserLocation = null
            };

            this._userServiceMock.Setup(x => x.GetEntityByIdAsync(userId)).ReturnsAsync(user);
            this._userServiceMock.Setup(x => x.GetEntityByIdAsync(friendId)).ReturnsAsync(friend);

            // Act & Assert
            var ex = Assert.ThrowsAsync<AppException>(() =>
                this._friendService.CreateFriendshipAsync(userId, friendId));
            Assert.That(ex.StatusCode, Is.EqualTo(400));
            Assert.That(ex.Code, Is.EqualTo(FriendInvitationErrorCode.ALREADY_FRIEND));
        }

        [Test]
        public async Task AreFriendsAsync_WhenFriends_ReturnsTrue()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var friendId = Guid.NewGuid();

            var friend = new User
            {
                Id = friendId,
                Username = "Friend",
                Nickname = "Friend123",
                Email = "",
                PasswordHash = "",
                RefreshToken = "",
                RefreshTokenExpiryTime = DateTime.Now,
                CreatedAt = DateTime.Now,
                Friends = new List<User>(),
                Groups = new List<Group>(),
                UserConfiguration = new UserConfiguration()
            };
            var user = new User
            {
                Id = userId,
                Username = "User",
                Nickname = "User123",
                Email = "",
                PasswordHash = "",
                RefreshToken = "",
                RefreshTokenExpiryTime = DateTime.Now,
                CreatedAt = DateTime.Now,
                Friends = new List<User> { friend },
                Groups = new List<Group>(),
                UserConfiguration = new UserConfiguration()
            };

            this._userServiceMock.Setup(x => x.GetEntityByIdAsync(userId)).ReturnsAsync(user);

            // Act
            var result = await this._friendService.AreFriendsAsync(userId, friendId);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task AreFriendsAsync_WhenNotFriends_ReturnsFalse()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var friendId = Guid.NewGuid();

            var user = new User
            {
                Id = userId,
                Username = "User",
                Nickname = "User123",
                Email = "",
                PasswordHash = "",
                RefreshToken = "",
                RefreshTokenExpiryTime = DateTime.Now,
                CreatedAt = DateTime.Now,
                Friends = new List<User>(),
                Groups = new List<Group>(),
                UserConfiguration = new UserConfiguration()
            };

            this._userServiceMock.Setup(x => x.GetEntityByIdAsync(userId)).ReturnsAsync(user);

            // Act
            var result = await this._friendService.AreFriendsAsync(userId, friendId);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task GetAllAsync_ReturnsUserDtos()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var users = new List<User>
            {
                new User
                {
                    Id = Guid.NewGuid(),
                    Username = "Friend1",
                    Nickname = "Friend123",
                    Email = "f1@example.com",
                    PasswordHash = "hash",
                    RefreshToken = "refresh",
                    RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(1),
                    CreatedAt = DateTime.UtcNow,
                    Friends = new List<User>(),
                    Groups = new List<Group>(),
                    UserConfiguration = new UserConfiguration(),
                    UserLocation = null
                }
            };

            var userDtos = new List<UserDto>
            {
                new UserDto(users[0].Id, users[0].Username, users[0].Nickname, users[0].Email)
            };

            this._friendRepositoryMock.Setup(r => r.GetAllAsync(userId)).ReturnsAsync(users);
            this._mapperMock.Setup(m => m.Map<IEnumerable<User>, IEnumerable<UserDto>>(users)).Returns(userDtos);

            // Act
            var result = await this._friendService.GetAllAsync(userId);

            // Assert
            Assert.That(result, Is.Not.Null);
            CollectionAssert.AreEqual(userDtos, result);
        }
    }
}