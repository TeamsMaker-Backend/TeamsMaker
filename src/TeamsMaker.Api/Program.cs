using System.Text;
using DataAccess.Context;
using DataAccess.Interceptors;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using TeamsMaker.Api;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

#region  AddCors
// string[] allowedOrigin = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>()!;
// builder.Services.AddCors(options =>
// {
//     options.AddPolicy("CorsPolicy",
//         policy =>
//             policy
//                 .WithOrigins(allowedOrigin!)
//                 .AllowAnyMethod()
//                 .AllowAnyHeader());
// });
#endregion

builder.Services.RegisterDataServices(builder.Configuration);
builder.Services.RegisterBusinessServices();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<EntitySaveChangesInterceptor>();
#region JWT
// builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AppDBContext>().AddDefaultTokenProviders();
// builder.Services.AddAuthentication(options =>
// {
//     options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//     options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//     options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
// }).AddJwtBearer(options =>
// {
//     options.SaveToken = true;
//     options.RequireHttpsMetadata = false;
//     options.TokenValidationParameters = new TokenValidationParameters()
//     {
//         ValidateIssuer = false,
//         ValidateAudience = false,
//         ValidAudience = builder.Configuration["SiteConfig:Audience"],
//         ValidIssuer = builder.Configuration["SiteConfig:Issuer"],
//         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["SiteConfig:Secret"]!))
//     };
// });
#endregion

// Learn more about configuring Swagger/OpenAPI
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

app.UseAuthorization();

app.MapControllers();

app.Run();
