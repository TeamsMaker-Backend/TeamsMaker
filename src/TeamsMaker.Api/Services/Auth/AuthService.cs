using System.Security;
using System.Security.Claims;
using TeamsMaker.Api.Core.Guards;
using TeamsMaker.Api.Configurations;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using TeamsMaker.Api.Contracts.Requests;
using TeamsMaker.Api.Contracts.Responses;
using TeamsMaker.Api.DataAccess.Context;
using TeamsMaker.Api.Core.Consts;


namespace TeamsMaker.Api.Services.Auth;

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


    public async Task<TokenResponse> RegisterAsync(UserRegisterationRequest registerationRequest, CancellationToken ct)
    {
        var existedUser = await _userManager.FindByEmailAsync(registerationRequest.Email);

        if (existedUser != null) throw new InvalidOperationException("A user with the specified email already exists.");

        Staff user = new()
        {
            FirstName = registerationRequest.FirstName,
            LastName = registerationRequest.LastName,
            Email = registerationRequest.Email,
            UserName = registerationRequest.UserName,
            Classification = StaffClassificationsEnum.Professor
        };

        var isCreated = await _userManager.CreateAsync(user, registerationRequest.Password);

        if (!isCreated.Succeeded)
        {
            var errors = string.Join(", ", isCreated.Errors.Select(e => e.Description));
            throw new InvalidOperationException(errors);
        }

        await _userManager.AddToRoleAsync(user, AppRoles.Professor);
        var jwtToken = await GenerateJwtTokenAsync(user, ct);

        return jwtToken;
    }

    public async Task<bool> VerifyUserAsync(UserVerificationRequset verificationRequest, CancellationToken ct)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.SSN == verificationRequest.SSN, ct);

        Guard.Against.Null(user, nameof(user));



        throw new NotImplementedException();
    }

    public async Task<TokenResponse> LoginAsync(UserLoginRequest loginRequest, CancellationToken ct)
    {
        var existedUser = await _userManager.FindByEmailAsync(loginRequest.Email);

        Guard.Against.Null(existedUser, nameof(existedUser));

        var isCorrectPassword = await _userManager.CheckPasswordAsync(existedUser!, loginRequest.Password);

        if (!isCorrectPassword) throw new SecurityException("Invalid password.");

        var jwtToken = await GenerateJwtTokenAsync(existedUser!, ct);

        return jwtToken;
    }

    public async Task<TokenResponse> RefreshTokenAsync(TokenRequest tokenRequest, CancellationToken ct)
    {
        bool isValidToken = await ValidateTokenAsync(tokenRequest, ct);

        if (!isValidToken) throw new SecurityTokenValidationException("Invalid token");

        var storedToken = await _db.RefreshTokens.FirstOrDefaultAsync(t => t.Token == tokenRequest.RefreshToken);

        var user = await _userManager.FindByIdAsync(storedToken!.UserId);

        Guard.Against.Null(user, nameof(user));

        var token = await GenerateJwtTokenAsync(user!, ct);

        return token;
    }

    private async Task<TokenResponse> GenerateJwtTokenAsync(User user, CancellationToken ct)
    {
        JwtSecurityTokenHandler jwtTokenHandler = new();

        var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

        var claims = await GetAllValidClaimsAsync(user, ct);

        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.Add(_jwtConfig.ExpireyTimeFrame),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };

        var token = jwtTokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = jwtTokenHandler.WriteToken(token);

        RefreshToken refreshToken = new()
        {
            JwtId = token.Id,
            UserId = user.Id,
            Token = RandomString(35) + Guid.NewGuid(),
            AddedOn = DateTime.UtcNow,
            ExpiresOn = DateTime.UtcNow.AddMonths(6),
            IsUsed = false,
            IsInvoked = false,
        };

        await _db.RefreshTokens.AddAsync(refreshToken);
        await _db.SaveChangesAsync();

        return new TokenResponse
        {
            Token = jwtToken,
            RefreshToken = refreshToken.Token
        };
    }

    private async Task<List<Claim>> GetAllValidClaimsAsync(User user, CancellationToken ct)
    {
        List<Claim> claims =
        [
            new Claim("Id", user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim("OrganizationId", user.OrganizationId.ToString()),
            new Claim(JwtRegisteredClaimNames.Sub, user.Email!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString())
        ];

        var userClaims = await _userManager.GetClaimsAsync(user);
        claims.AddRange(userClaims);

        var userRoles = await _userManager.GetRolesAsync(user);

        foreach (var userRole in userRoles)
        {
            var role = await _roleManager.FindByNameAsync(userRole);

            if (role != null)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));

                var roleClaims = await _roleManager.GetClaimsAsync(role);

                foreach (var roleClaim in roleClaims) claims.Add(roleClaim);
            }
        }

        return claims;
    }

    private async Task<bool> ValidateTokenAsync(TokenRequest tokenRequest, CancellationToken ct)
    {
        JwtSecurityTokenHandler tokenHandler = new();

        // step 1: validate jwt token format
        var tokenInVerification = tokenHandler
            .ValidateToken(tokenRequest.Token, _tokenValidationParams, out var validatedToken);

        // step 2: validate encryption alg
        if (validatedToken is JwtSecurityToken jwtSecurityToken)
        {
            var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCulture);

            if (result == false) return false;
        }

        // step 3: expiry date
        long utcExpiryDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp)!.Value);

        DateTime expiryDate = DateTimeOffset.FromUnixTimeSeconds(utcExpiryDate).DateTime;

        if (expiryDate > DateTime.UtcNow) return false;

        // step 4: validate token exists or not
        var storedToken = await _db.RefreshTokens.FirstOrDefaultAsync(t => t.Token == tokenRequest.RefreshToken);

        if (storedToken == null) return false;

        // step 5: validate token in use or not
        if (storedToken.IsUsed) return false;

        // step 6: validate if revoked
        if (storedToken.IsInvoked) return false;

        // step 7: validate the id
        var jti = tokenInVerification.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)!.Value;

        if (storedToken.JwtId != jti) return false;

        if (storedToken.ExpiresOn < DateTime.UtcNow) return false;

        // update current token
        storedToken.IsUsed = true;
        _db.RefreshTokens.Update(storedToken);
        await _db.SaveChangesAsync();

        return true;
    }


    private static string RandomString(int length)
    {
        Random random = new();

        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

}
