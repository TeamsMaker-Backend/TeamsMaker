using Core.ResultMessages;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TeamsMaker.Api;
using TeamsMaker.Api.Configurations;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.RegisterBusinessServices();
builder.Services.RegisterDataServices(builder.Configuration);
builder.Services.AddHttpContextAccessor();

// model state validation
builder.Services.Configure<ApiBehaviorOptions>(options => {
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

#region JWT
builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));

var key = Encoding.ASCII.GetBytes(builder.Configuration["JwtConfig:Secret"]!);

var tokenValidationParams = new TokenValidationParameters
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
#endregion



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseCors();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
