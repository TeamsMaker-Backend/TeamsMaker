using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using TeamsMaker.Api;
using TeamsMaker.Api.Configurations;
using TeamsMaker.Api.Core.ResultMessages;
using TeamsMaker.Api.DataAccess.Seeds;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
                .AddNewtonsoftJson();

builder.Services.AddCors(options => options.AddPolicy("CorsPolicy",
        builder => builder.WithOrigins("http://localhost:5173")
                        .AllowCredentials()
                        .AllowAnyMethod()
                        .WithHeaders("Content-Type", "Authorization")));

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

// builder.Services.AddExceptionHandler<AppExceptionHandler>();
builder.Services.AddHttpContextAccessor();
builder.Services.RegisterBusinessServices();
builder.Services.RegisterDataServices(builder.Configuration);

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
            Success = false,
            Exception = errors,
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


app.UseSwagger();
app.UseSwaggerUI();

await SeedDB.Initialize(app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope().ServiceProvider);

// app.UseExceptionHandler(opt => { });

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
