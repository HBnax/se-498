using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pitchamon.Backend.Data;
using pitchamon.Backend.Models;

namespace pitchamon.Backend.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly BackendDbContext dbContext;
    
    public AuthController(BackendDbContext context)
    {
        dbContext = context;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest(new { error = "Email and password are required." });
        }
        
        var email = request.Email.Trim().ToLowerInvariant();

        var exists = await dbContext.Users.AnyAsync(u => u.Email == email);

        if (exists)
        {
            return Conflict(new { error = "Email is already registered." });
        }

        var user = new User
        {
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
        };

        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();

        return Ok(new
        {
            message = "Successfully registered.",
            user = new
            {
                user.Id,
                user.Email,
                user.CreatedAt
            }
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest(new { error = "Email and password are required." });
        }
        
        var email = request.Email.Trim().ToLowerInvariant();
        
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);

        if (user == null)
        {
            return Unauthorized(new { error = "User not found" });
        }
        
        var validPassword = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
        
        if (!validPassword)
        {
            return Unauthorized(new { error = "Invalid password" });
        }

        return Ok(new
        {
            message = "Login successful.",
            user = new
            {
                user.Id,
                user.Email,
                user.CreatedAt
            }
        });
    }
}