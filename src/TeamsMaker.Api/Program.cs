using TeamsMaker.Api;
using Microsoft.AspNetCore.Mvc;
using TeamsMaker.Api.Configurations;
using Microsoft.IdentityModel.Tokens;
using TeamsMaker.Api.Core.ResultMessages;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using TeamsMaker.Api.DataAccess.Seeds;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authorization;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

#region JWT & Authorization;
builder.Services.AddSingleton(builder.Configuration.GetSection("JwtConfig").Get<JwtConfig>()!);
var key = Encoding.ASCII.GetBytes(builder.Configuration["JwtConfig:Secret"]!);

TokenValidationParameters tokenValidationParams = new()
{
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(key),
    ValidateIssuer = false,
    ValidateAudience = false,
    ValidateLifetime = true,
    RequireExpirationTime = false
};

builder.Services.AddSingleton(tokenValidationParams);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(jwt =>
{
    jwt.SaveToken = true;
    jwt.TokenValidationParameters = tokenValidationParams;
});

builder.Services.AddAuthorizationBuilder()
    .SetDefaultPolicy(new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
        .Build());
#endregion

builder.Services.RegisterBusinessServices();
builder.Services.RegisterDataServices(builder.Configuration);
builder.Services.AddHttpContextAccessor();

#region  model state validation
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = actionContext =>
    {
        var errors = actionContext.ModelState
            .Where(e => e.Value!.Errors.Any())
            .Select(e => new
            {
                Name = e.Key,
                Message = e.Value!.Errors.First().ErrorMessage.Split(',')[0]
            })
            .ToArray();

        ResultMessage message = new()
        {
            EngMsg = "Validation Error",
            LocMsg = "خطأ فى البيانات المدخلة",
            Success = false,
            exception = errors,
            ReturnObject = null
        };

        return new BadRequestObjectResult(message);
    };
});
#endregion

#region swagger configs
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

await SeedDB.Initialize(app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope().ServiceProvider);
// app.UseCors();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
