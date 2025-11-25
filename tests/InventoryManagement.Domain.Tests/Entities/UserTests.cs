using FluentAssertions;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.ValueObjects;

namespace InventoryManagement.Domain.Tests.Entities;

public class UserTests
{
    [Fact]
    public void Create_WithValidData_ShouldCreateUser()
    {
        // Arrange
        var firstName = "John";
        var lastName = "Doe";
        var email = Email.Create("john.doe@example.com");
        var passwordHash = "hashedPassword123";

        // Act
        var user = User.Create(firstName, lastName, email, passwordHash);

        // Assert
        user.Should().NotBeNull();
        user.Id.Should().NotBeEmpty();
        user.FirstName.Should().Be(firstName);
        user.LastName.Should().Be(lastName);
        user.Email.Should().Be(email);
        user.PasswordHash.Should().Be(passwordHash);
        user.IsActive.Should().BeTrue();
        user.FullName.Should().Be("John Doe");
        user.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        user.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithInvalidFirstName_ShouldThrowException(string invalidFirstName)
    {
        // Arrange
        var lastName = "Doe";
        var email = Email.Create("john.doe@example.com");
        var passwordHash = "hashedPassword123";

        // Act
        Action act = () => User.Create(invalidFirstName, lastName, email, passwordHash);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*First name*");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithInvalidLastName_ShouldThrowException(string invalidLastName)
    {
        // Arrange
        var firstName = "John";
        var email = Email.Create("john.doe@example.com");
        var passwordHash = "hashedPassword123";

        // Act
        Action act = () => User.Create(firstName, invalidLastName, email, passwordHash);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Last name*");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithInvalidPasswordHash_ShouldThrowException(string invalidPasswordHash)
    {
        // Arrange
        var firstName = "John";
        var lastName = "Doe";
        var email = Email.Create("john.doe@example.com");

        // Act
        Action act = () => User.Create(firstName, lastName, email, invalidPasswordHash);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Deactivate_WhenUserIsActive_ShouldDeactivateUser()
    {
        // Arrange
        var user = User.Create("John", "Doe", Email.Create("john.doe@example.com"), "hashedPassword123");

        // Act
        user.Deactivate();

        // Assert
        user.IsActive.Should().BeFalse();
    }

    [Fact]
    public void Activate_WhenUserIsInactive_ShouldActivateUser()
    {
        // Arrange
        var user = User.Create("John", "Doe", Email.Create("john.doe@example.com"), "hashedPassword123");
        user.Deactivate();

        // Act
        user.Activate();

        // Assert
        user.IsActive.Should().BeTrue();
    }

    [Fact]
    public void UpdateInformation_WithValidData_ShouldUpdateUser()
    {
        // Arrange
        var user = User.Create("John", "Doe", Email.Create("john.doe@example.com"), "hashedPassword123");
        var originalUpdatedAt = user.UpdatedAt;
        Thread.Sleep(10); // Small delay to ensure UpdatedAt changes

        var newFirstName = "Jane";
        var newLastName = "Smith";
        var newEmail = Email.Create("jane.smith@example.com");

        // Act
        user.UpdateInformation(newFirstName, newLastName, newEmail);

        // Assert
        user.FirstName.Should().Be(newFirstName);
        user.LastName.Should().Be(newLastName);
        user.Email.Should().Be(newEmail);
        user.FullName.Should().Be("Jane Smith");
        user.UpdatedAt.Should().BeAfter(originalUpdatedAt);
    }

    [Fact]
    public void UpdateInformation_WithInvalidFirstName_ShouldThrowException()
    {
        // Arrange
        var user = User.Create("John", "Doe", Email.Create("john.doe@example.com"), "hashedPassword123");
        var email = Email.Create("john.doe@example.com");

        // Act
        Action act = () => user.UpdateInformation(null, "Doe", email);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void ChangePassword_WithValidHash_ShouldUpdatePassword()
    {
        // Arrange
        var user = User.Create("John", "Doe", Email.Create("john.doe@example.com"), "hashedPassword123");
        var originalPasswordHash = user.PasswordHash;
        var originalUpdatedAt = user.UpdatedAt;
        Thread.Sleep(10); // Small delay to ensure UpdatedAt changes

        var newPasswordHash = "newHashedPassword456";

        // Act
        user.ChangePassword(newPasswordHash);

        // Assert
        user.PasswordHash.Should().Be(newPasswordHash);
        user.PasswordHash.Should().NotBe(originalPasswordHash);
        user.UpdatedAt.Should().BeAfter(originalUpdatedAt);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void ChangePassword_WithInvalidHash_ShouldThrowException(string invalidPasswordHash)
    {
        // Arrange
        var user = User.Create("John", "Doe", Email.Create("john.doe@example.com"), "hashedPassword123");

        // Act
        Action act = () => user.ChangePassword(invalidPasswordHash);

        // Assert
        act.Should().Throw<ArgumentException>();
    }
}
