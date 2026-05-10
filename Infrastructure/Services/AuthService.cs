using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

using Application.Auth.Dtos;
using Application.Common.Interfaces;

using Domain.Models.Common;
using Domain.Models.Users;

using Infrastructure.Configuration;
using Infrastructure.Database;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly JwtSettings _jwtSettings;

    public AuthService(AppDbContext context, IOptions<JwtSettings> jwtOptions)
    {
        _context = context;
        _jwtSettings = jwtOptions.Value;
    }

    public async Task<OperationResult<AuthResponseDto>> RegisterAsync(RegisterUserDto dto)
    {
        if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
            return OperationResult<AuthResponseDto>.Failure("Username already exists.");

        var hash = HashPassword(dto.Password);

        var user = new User
        {
            Username = dto.Username,
            Email = dto.Email,
            PasswordHash = hash,
            Role = "User" // default
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var token = GenerateJwt(user);

        return OperationResult<AuthResponseDto>.Success(new AuthResponseDto
        {
            Token = token,
            User = MapUser(user)
        });
    }

    public async Task<OperationResult<AuthResponseDto>> LoginAsync(LoginUserDto dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);

        if (user is null || !VerifyPassword(dto.Password, user.PasswordHash))
            return OperationResult<AuthResponseDto>.Failure("Invalid username or password.");

        var token = GenerateJwt(user);

        return OperationResult<AuthResponseDto>.Success(new AuthResponseDto
        {
            Token = token,
            User = MapUser(user)
        });
    }

    private byte[] HashPassword(string password)
    {
        using var sha = SHA256.Create();
        return sha.ComputeHash(Encoding.UTF8.GetBytes(password));
    }

    private bool VerifyPassword(string password, byte[] storedHash)
    {
        using var sha = SHA256.Create();
        var computed = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
        return storedHash.SequenceEqual(computed);
    }


    private string GenerateJwt(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Role, user.Role)
        };

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpireMinutes),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private UserDtoResponse MapUser(User user)
    {
        return new UserDtoResponse
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Role = user.Role
        };
    }
}
