using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security;
using System.Security.Claims;
using DataAccess.Base.Interfaces;
using Microsoft.IdentityModel.Tokens;
using TeamsMaker.Api.Configurations;
using TeamsMaker.Api.Contracts.Requests;
using TeamsMaker.Api.Contracts.Responses;
using TeamsMaker.Api.Core.Consts;
using TeamsMaker.Api.Core.Guards;
using TeamsMaker.Api.DataAccess.Context;
using TeamsMaker.Core.Enums;


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
        TokenValidationParameters tokenValidationParams,
        IUserInfo userInfo)
    {
        _db = db;
        _userManager = userManager;
        _roleManager = roleManager;
        _jwtConfig = jwtConfig;
        _tokenValidationParams = tokenValidationParams;
    }


    public async Task<TokenResponse> RegisterAsync(UserRegisterationRequest registerRequest, CancellationToken ct)
    {
        User user = new();

        if (registerRequest.UserType == (int)UserEnum.Student) await RegisterStudentAsync(registerRequest, user, ct);
        else if (registerRequest.UserType == (int)UserEnum.Staff) await RegisterStaffAsync(registerRequest, user, ct);
        else throw new ArgumentException("Invalid user type");

        var result = await _userManager.CreateAsync(user, registerRequest.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new Exception(errors);
        }

        var role = registerRequest.UserType == (int)UserEnum.Staff ? AppRoles.Professor : AppRoles.Student;
        await _userManager.AddToRoleAsync(user, role);

        var tokenResponse = await GenerateJwtTokenAsync(user, ct);
        return tokenResponse;
    }

    #region 
    // public async Task<TokenResponse> RegisterAsyncV1(UserRegisterationRequest registerRequest, CancellationToken ct)
    // {
    //     var existedUser = await _db.ImportedStudents.SingleOrDefaultAsync(u => u.SSN == registerRequest.SSN, ct);

    //     if (existedUser is null)
    //         throw new InvalidOperationException("This user is not allowed to register.");

    //     if (await _db.Users.AnyAsync(x => x.Email == registerRequest.Email))
    //         throw new ArgumentException("This Email already exists");

    //     User user;

    //     if (registerRequest.UserType == (int)UserEnum.Student)
    //     {
    //         Student student = new();
    //         CreateUser(student, registerRequest);

    //         var department = await _db.Departments.SingleOrDefaultAsync(x => x.Code == existedUser.Department, ct);
    //         // Student Data
    //         student.CollegeId = existedUser.CollegeId!;
    //         student.GraduationYear = existedUser.GraduationYear!;
    //         student.GPA = existedUser.GPA!;
    //         student.OrganizationId = existedUser.OrganizationId;
    //         student.Department = department;

    //         user = student;
    //     }
    //     else if (registerRequest.UserType == (int)UserEnum.Staff)
    //     {
    //         Staff staff = new();
    //         CreateUser(staff, registerRequest);

    //         // Professor Data
    //         staff.OrganizationId = existedUser.OrganizationId;
    //         staff.Classification = StaffClassificationsEnum.Professor; //TODO: assign classification from imported user

    //         user = staff;
    //     }
    //     else throw new ArgumentException("Invalid user type");

    //     var result = await _userManager.CreateAsync(user, registerRequest.Password);

    //     if (!result.Succeeded)
    //     {
    //         var errors = string.Join(", ", result.Errors.Select(e => e.Description));
    //         throw new Exception(errors);
    //     }

    //     var role = registerRequest.UserType == (int)UserEnum.Staff ? AppRoles.Professor : AppRoles.Student;
    //     await _userManager.AddToRoleAsync(user, role);

    //     var tokenResponse = await GenerateJwtTokenAsync(user, ct);
    //     return tokenResponse;
    // }
    #endregion

    public async Task<bool> VerifyUserAsync(UserVerificationRequset verificationRequest, CancellationToken ct)
    {
        if (verificationRequest.UserType == (int)UserEnum.Student)
        {
            if (!string.IsNullOrEmpty(verificationRequest.CollegeId)) throw new ArgumentException("College Id must has a value");

            return await _db.ImportedStudents.AnyAsync(s => s.SSN == verificationRequest.SSN && s.CollegeId == verificationRequest.CollegeId, ct);
        }
        else if (verificationRequest.UserType == (int)UserEnum.Staff)
            return await _db.ImportedStaff.AnyAsync(s => s.SSN == verificationRequest.SSN, ct);
        else throw new ArgumentException("Invalid user type");
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

    private async Task RegisterStudentAsync(UserRegisterationRequest registerRequest, User user, CancellationToken ct)
    {
        var existedStudent = await _db.ImportedStudents.SingleOrDefaultAsync(u => u.SSN == registerRequest.SSN, ct)
            ?? throw new InvalidOperationException("This user is not allowed to register.");

        if (await _db.Users.AnyAsync(x => x.Email == registerRequest.Email, ct))
            throw new ArgumentException("This Email already exists");

        Student student = new();
        CreateUser(student, registerRequest);

        var department = await _db.Departments.SingleOrDefaultAsync(x => x.Code == existedStudent.Department, ct);
        // Student Data
        student.CollegeId = existedStudent.CollegeId;
        student.GraduationYear = existedStudent.GraduationYear;
        student.GPA = existedStudent.GPA;
        student.OrganizationId = existedStudent.OrganizationId;
        student.Department = department;

        user = student;
    }

    private async Task RegisterStaffAsync(UserRegisterationRequest registerRequest, User user, CancellationToken ct)
    {
        var existedStaff = await _db.ImportedStaff.SingleOrDefaultAsync(u => u.SSN == registerRequest.SSN, ct)
            ?? throw new InvalidOperationException("This user is not allowed to register.");

        if (await _db.Users.AnyAsync(x => x.Email == registerRequest.Email))
            throw new ArgumentException("This Email already exists");

        Staff staff = new();
        CreateUser(staff, registerRequest);

        // Professor Data
        staff.OrganizationId = existedStaff.OrganizationId;
        staff.Classification = StaffClassificationsEnum.Professor; //TODO: assign classification from imported user

        user = staff;
    }

    private void CreateUser(User user, UserRegisterationRequest registerRequest)
    {
        user.FirstName = registerRequest.FirstName;
        user.LastName = registerRequest.LastName;
        user.Email = registerRequest.Email;
        user.UserName = new MailAddress(registerRequest.Email).User;
        user.SSN = registerRequest.SSN;
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

    private string RandomString(int length)
    {
        Random random = new();

        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

}
