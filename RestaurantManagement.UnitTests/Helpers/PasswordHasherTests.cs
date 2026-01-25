using RestaurantManagement.Backend.Helper;

namespace RestaurantManagement.UnitTests;

[TestClass]
public class PasswordHasherTests
{
    [TestMethod]
    public void Hash_ShouldReturnNonEmptyHash()
    {
        // Arrange
        var password = "MyPassword123";

        // Act
        var hash = PasswordHasher.Hash(password);

        // Assert
        Assert.IsFalse(string.IsNullOrWhiteSpace(hash));
        Assert.AreNotEqual(password, hash);
    }

    [TestMethod]
    public void Verify_CorrectPassword_ReturnsTrue()
    {
        // Arrange
        var password = "MyPassword123";
        var hash = PasswordHasher.Hash(password);

        // Act
        var result = PasswordHasher.Verify(password, hash);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void Verify_WrongPassword_ReturnsFalse()
    {
        // Arrange
        var password = "MyPassword123";
        var hash = PasswordHasher.Hash(password);

        // Act
        var result = PasswordHasher.Verify("WrongPassword", hash);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void Verify_PasswordWithSpaces_ReturnsTrue()
    {
        // Arrange
        var password = "MyPassword123";
        var hash = PasswordHasher.Hash(password);

        // Act
        var result = PasswordHasher.Verify("  MyPassword123  ", hash);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void Verify_PasswordIsCaseSensitive_ReturnsFalse()
    {
        // Arrange
        var password = "MyPassword123";
        var hash = PasswordHasher.Hash(password);

        // Act
        var result = PasswordHasher.Verify("mypassword123", hash);

        // Assert
        Assert.IsFalse(result);
    }
}
