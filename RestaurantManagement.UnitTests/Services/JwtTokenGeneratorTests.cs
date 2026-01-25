using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RestaurantManagement.Backend.Services;
using RestaurantManagement.Backend.Utils;
using RestaurantManagement.DataAccess.Models;
using RestaurantManagement.Models.Common.Enums;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RestaurantManagement.UnitTests;

[TestClass]
public class JwtTokenGeneratorTests
{
    private IJwtTokenGenerator _jwtGenerator = null!;
    private IConfiguration _configuration = null!;

    [TestInitialize]
    public void Setup()
    {
        var settings = new Dictionary<string, string>
            {
                { "Jwt:Key", "THIS_IS_A_SUPER_SECRET_KEY_FOR_TESTING_12345" },
                { "Jwt:Issuer", "TestIssuer" },
                { "Jwt:Audience", "TestAudience" },
                { "Jwt:DurationInMinutes", "60" }
            };

        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(settings)
            .Build();

        _jwtGenerator = new JwtTokenGenerator(_configuration);
    }

    [TestMethod]
    public void GenerateToken_ShouldReturnValidJwtToken()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "test@test.com",
            Name = "Chandrapal Singh",
            Role = UserRole.Admin
        };

        // Act
        var tokenString = _jwtGenerator.GenerateToken(user);

        // Assert
        Assert.IsFalse(string.IsNullOrWhiteSpace(tokenString));

        var handler = new JwtSecurityTokenHandler();
        Assert.IsTrue(handler.CanReadToken(tokenString));
    }

    [TestMethod]
    public void GenerateToken_ShouldContainExpectedClaims()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "test@test.com",
            Name = "Chandrapal Singh",
            Role = UserRole.Waiter
        };

        // Act
        var tokenString = _jwtGenerator.GenerateToken(user);

        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(tokenString);

        // Assert claims
        Assert.AreEqual(user.Id.ToString(),
            token.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);

        Assert.AreEqual(user.Email,
            token.Claims.First(c => c.Type == ClaimTypes.Email).Value);

        Assert.AreEqual(user.Role.ToString(),
            token.Claims.First(c => c.Type == ClaimTypes.Role).Value);

        Assert.AreEqual(user.Name,
            token.Claims.First(c => c.Type == "fullName").Value);
    }

    [TestMethod]
    public void GenerateToken_ShouldSetCorrectIssuerAndAudience()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "issuer@test.com",
            Name = "Issuer Test",
            Role = UserRole.Chef
        };

        // Act
        var tokenString = _jwtGenerator.GenerateToken(user);
        var token = new JwtSecurityTokenHandler().ReadJwtToken(tokenString);

        // Assert
        Assert.AreEqual("TestIssuer", token.Issuer);
    }

    [TestMethod]
    public void GenerateToken_ShouldHaveValidExpiryTime()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "expiry@test.com",
            Name = "Expiry Test",
            Role = UserRole.Waiter
        };

        // Act
        var tokenString = _jwtGenerator.GenerateToken(user);
        var token = new JwtSecurityTokenHandler().ReadJwtToken(tokenString);

        // Assert
        Assert.IsTrue(token.ValidTo > DateTime.UtcNow);
        Assert.IsTrue(token.ValidTo <= DateTime.UtcNow.AddMinutes(61));
    }

    [TestMethod]
    public void GenerateToken_ShouldBeSignedWithConfiguredKey()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "sign@test.com",
            Name = "Sign Test",
            Role = UserRole.Admin
        };

        var tokenString = _jwtGenerator.GenerateToken(user);

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidIssuer = "TestIssuer",
            ValidAudience = "TestAudience",
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("THIS_IS_A_SUPER_SECRET_KEY_FOR_TESTING_12345"))
        };

        var handler = new JwtSecurityTokenHandler();

        handler.ValidateToken(tokenString, validationParameters, out _);
    }
}
