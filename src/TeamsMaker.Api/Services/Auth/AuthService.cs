using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using TeamsMaker.Api.Configurations;
using TeamsMaker.Api.Contracts.Requests;
using TeamsMaker.Api.DataAccess.Context;
using TeamsMaker.Api.DataAccess.Models;

namespace TeamsMaker.Api;

public class AuthService : IAuthService
{
    private readonly AppDBContext _db;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly JwtConfig _jwtConfig;
    private readonly TokenValidationParameters _tokenValidationParams;

    public AuthService(AppDBContext db,
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        JwtConfig jwtConfig, 
        TokenValidationParameters tokenValidationParams)
    {
        _db = db;
        _userManager = userManager;
        _roleManager = roleManager;
        _jwtConfig = jwtConfig;
        _tokenValidationParams = tokenValidationParams;
    }


    public Task RegisterAsync(UserRegisterationRequest registerationRequest, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task<bool> VerifyUserAsync(UserVerificationRequset verificationRequest, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task LoginAsync(UserLoginRequest loginRequest, CancellationToken ct)
    {
        throw new NotImplementedException();
    }



    // private async Task<string> GenerateTokenAsync(User user, CancellationToken ct)
    // {
    //     JwtSecurityTokenHandler jwtTokenHandler = new();

    //     var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

    //     //var claims = await GetAllValidClaims(user);

    //     SecurityTokenDescriptor tokenDescriptor = new()
    //     {
    //         //Subject = new ClaimsIdentity(claims),
    //         Expires = DateTime.UtcNow.Add(_jwtConfig.ExpireyTimeFrame),
    //         SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
    //     };

    //     var token = jwtTokenHandler.CreateToken(tokenDescriptor);
    //     var jwtToken = jwtTokenHandler.WriteToken(token);

    //     RefreshToken refreshToken = new()
    //     {
    //         JwtId = token.Id,
    //         UserId = user.Id,
    //         Token = RandomString(35) + Guid.NewGuid(),
    //         AddedOn = DateTime.UtcNow,
    //         ExpiresOn = DateTime.UtcNow.AddMonths(6),
    //         IsUsed = false,
    //         IsInvoked = false,
    //     };

    //     await _db.RefreshTokens.AddAsync(refreshToken);
    //     await _db.SaveChangesAsync();

    //     throw new NotImplementedException();
    // }

    private static string RandomString(int length)
    {
        Random random = new();

        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
