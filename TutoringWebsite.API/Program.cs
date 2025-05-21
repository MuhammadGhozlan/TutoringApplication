using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TutoringWebsite.API.Data;
using TutoringWebsite.API.Interfaces.IRepositories;
using TutoringWebsite.API.Interfaces.IServices;
using TutoringWebsite.API.Models;
using TutoringWebsite.API.Repositories;
using TutoringWebsite.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
//Database Setting
builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
    ));

//Identity Settings
builder.Services.AddDbContext<IdentityContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection"))
);
builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<IdentityContext>();

//CORS Setting
builder.Services.AddCors(options => options.AddPolicy("TutoringCORS", builder =>
{
    builder.AllowAnyOrigin();
    builder.AllowAnyMethod();
    builder.AllowAnyHeader();
}));

//Add Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(cfg =>
{
    cfg.RequireHttpsMetadata = false;
    cfg.SaveToken = false;
    cfg.TokenValidationParameters = new TokenValidationParameters
    {        
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["IssuerKey"]!)),
        ValidateAudience = false,
        ValidateIssuer = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
}).AddCookie(cfg =>
{
    cfg.Cookie.Name="MyCookie";
    cfg.LoginPath = "/Account/Login";
    cfg.LogoutPath = "/Account/Logout";
    cfg.AccessDeniedPath = "/Account/AccessDenied";
    cfg.Cookie.SameSite = SameSiteMode.Strict;
    cfg.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    cfg.Cookie.HttpOnly = true;
    cfg.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    cfg.SlidingExpiration = true;
});
builder.Services.AddAuthorization();

//Service Registration
builder.Services.AddScoped<IRegistrationService, RegistrationService>();
builder.Services.AddScoped<IStudentService, StudentService>();
//Repository Registration
builder.Services.AddScoped<IStudentRepository, StudentRepository>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
var app = builder.Build();

// Configure the HTTP request pipeline.
if(app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("TutoringCORS");
app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();
app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("swagger/v1/swagger.json","API V1");
    c.RoutePrefix=string.Empty;
});
var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
