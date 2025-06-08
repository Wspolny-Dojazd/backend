using Application.DTOs;
using Application.Exceptions;
using Application.Services;
using AutoMapper;
using Domain.Interfaces;
using Domain.Model;
using Moq;

namespace Test
{
    [TestFixture]
    public class UserServiceTests
    {
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private UserService _userService;

        [SetUp]
        public void SetUp()
        {
            // setup mocks for necessary services for every test
            this._userRepositoryMock = new Mock<IUserRepository>();
            this._mapperMock = new Mock<IMapper>();
            this._userService = new UserService(this._userRepositoryMock.Object, this._mapperMock.Object);
        }

        [Test]
        public async Task GetByIdAsync_UserExists_ReturnsUserDto()
        {
            var userId = Guid.NewGuid();
            var user = new User
            {
                Id = userId,
                Username = "TestUser",
                Nickname = "TestNick",
                Email = "test@test.com",
                PasswordHash = "hashTest",
                RefreshToken = "refreshTest",
                RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(1),
                CreatedAt = DateTime.UtcNow,
                Friends = new List<User>(),
                Groups = new List<Group>(),
                UserConfiguration = new UserConfiguration(),
                UserLocation = null
            };

            var userDto = new UserDto(
                user.Id,
                user.Username,
                user.Nickname,
                user.Email
            );

            this._userRepositoryMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);
            this._mapperMock.Setup(m => m.Map<User, UserDto>(user)).Returns(userDto);

            // Act
            var result = await this._userService.GetByIdAsync(userId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(userDto));
        }

        [Test]
        public void GetEntityByIdAsync_UserNotFound_ThrowsUserNotFoundException()
        {
            var userId = Guid.NewGuid();
            
            this._userRepositoryMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync((User)null);

            Assert.ThrowsAsync<UserNotFoundException>(() => this._userService.GetEntityByIdAsync(userId));
        }

        [Test]
        public async Task SearchByUsernameOrNicknameAsync_FindsMatchingUsers_ReturnsUserDtos()
        {
            // Arrange
            var users = new List<User>
            {
                new User
                {
                    Id = Guid.NewGuid(),
                    Username = "JohnDoe",
                    Nickname = "JD",
                    Email = "john@example.com",
                    PasswordHash = "hash",
                    RefreshToken = "refresh",
                    RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(1),
                    CreatedAt = DateTime.UtcNow,
                    Friends = new List<User>(),
                    Groups = new List<Group>(),
                    UserConfiguration = new UserConfiguration(),
                    UserLocation = null
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    Username = "Jane",
                    Nickname = "JJ",
                    Email = "jane@example.com",
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
                new UserDto(users[0].Id, users[0].Username, users[0].Nickname, users[0].Email),
                new UserDto(users[1].Id, users[1].Username, users[1].Nickname, users[1].Email)
            };

            this._userRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(users);
            this._mapperMock.Setup(m => m.Map<IEnumerable<User>, IEnumerable<UserDto>>(It.IsAny<IEnumerable<User>>()))
                .Returns(userDtos);

            // Act
            var result = await this._userService.SearchByUsernameOrNicknameAsync("john");

            // Assert
            Assert.IsNotNull(result);
            CollectionAssert.AreEqual(userDtos, result);
        }

        [Test]
        public async Task GetEntityByIdAsync_UserExists_ReturnsUser()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User
            {
                Id = userId,
                Username = "TestUser",
                Nickname = "TestNick",
                Email = "test@example.com",
                PasswordHash = "hash",
                RefreshToken = "refresh",
                RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(1),
                CreatedAt = DateTime.UtcNow,
                Friends = new List<User>(),
                Groups = new List<Group>(),
                UserConfiguration = new UserConfiguration(),
                UserLocation = null
            };

            this._userRepositoryMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);

            // Act
            var result = await this._userService.GetEntityByIdAsync(userId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(user));
        }
    }
}
