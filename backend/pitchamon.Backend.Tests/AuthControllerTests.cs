using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pitchamon.Backend.Controllers;
using pitchamon.Backend.Data;
using pitchamon.Backend.Models;

namespace pitchamon.Backend.Tests;

public class AuthControllerTests
{
    private static BackendDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<BackendDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new BackendDbContext(options);
    }

    [Fact]
    public async Task Register_ReturnsBadRequest_WhenEmailIsMissing()
    {
        var context = CreateDbContext();
        var controller = new AuthController(context);

        var request = new RegisterRequest
        {
            Email = "",
            Password = "password123"
        };

        var result = await controller.Register(request);

        var badRequest = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        badRequest.StatusCode.Should().Be(400);
    }

    [Fact]
    public async Task Register_ReturnsConflict_WhenEmailAlreadyExists()
    {
        var context = CreateDbContext();
        context.Users.Add(new User
        {
            Email = "test@example.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123")
        });
        context.SaveChanges();

        var controller = new AuthController(context);

        var request = new RegisterRequest
        {
            Email = "test@example.com",
            Password = "newpassword"
        };

        var result = await controller.Register(request);

        var conflict = result.Should().BeOfType<ConflictObjectResult>().Subject;
        conflict.StatusCode.Should().Be(409);
    }

    [Fact]
    public async Task Register_ReturnsExpectedResult_WhenRequestIsValid()
    {
        var context = CreateDbContext();
        var controller = new AuthController(context);

        var request = new RegisterRequest
        {
            Email = "newuser@example.com",
            Password = "password123"
        };

        var result = await controller.Register(request);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.StatusCode.Should().Be(200);
        context.Users.Any(u => u.Email == "newuser@example.com").Should().BeTrue();
    }

    [Fact]
    public async Task Login_ReturnsBadRequest_WhenEmailIsMissing()
    {
        var context = CreateDbContext();
        var controller = new AuthController(context);

        var request = new LoginRequest
        {
            Email = "",
            Password = "password123"
        };

        var result = await controller.Login(request);

        var badRequest = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        badRequest.StatusCode.Should().Be(400);
    }

    [Fact]
    public async Task Login_ReturnsUnauthorized_WhenUserDoesNotExist()
    {
        var context = CreateDbContext();
        var controller = new AuthController(context);

        var request = new LoginRequest
        {
            Email = "missing@example.com",
            Password = "password123"
        };

        var result = await controller.Login(request);

        var unauthorized = result.Should().BeOfType<UnauthorizedObjectResult>().Subject;
        unauthorized.StatusCode.Should().Be(401);
    }

    [Fact]
    public async Task Login_ReturnsUnauthorized_WhenPasswordIsWrong()
    {
        var context = CreateDbContext();
        context.Users.Add(new User
        {
            Email = "test@example.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("correctpassword")
        });
        context.SaveChanges();

        var controller = new AuthController(context);

        var request = new LoginRequest
        {
            Email = "test@example.com",
            Password = "wrongpassword"
        };

        var result = await controller.Login(request);

        var unauthorized = result.Should().BeOfType<UnauthorizedObjectResult>().Subject;
        unauthorized.StatusCode.Should().Be(401);
    }

    [Fact]
    public async Task Login_ReturnsExpectedResult_WhenCredentialsAreValid()
    {
        var context = CreateDbContext();
        context.Users.Add(new User
        {
            Email = "test@example.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123")
        });
        context.SaveChanges();

        var controller = new AuthController(context);

        var request = new LoginRequest
        {
            Email = "test@example.com",
            Password = "password123"
        };

        var result = await controller.Login(request);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.StatusCode.Should().Be(200);
    }
}