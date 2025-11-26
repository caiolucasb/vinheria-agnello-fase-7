using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// O ideal é colocar isso numa secret, mas para o intuito do desafio que é simular uma infra não achei necessário
var secretKey = "cKSik3qWxavw9AwsLCPlSKz5wh3MXyWoEvJDV7mgrvJUhBBdDOnLqLZxLWbE4Zxc";
var key = Encoding.ASCII.GetBytes(secretKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // apenas para testes
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapPost("/login", () =>
{
    // Simulação de usuário (não usa banco aqui)
    var claims = new[]
    {
        new Claim(ClaimTypes.Name, "usuarioVinheria"),
        new Claim(ClaimTypes.Role, "cliente")
    };

    var credenciais = new SigningCredentials(
        new SymmetricSecurityKey(key),
        SecurityAlgorithms.HmacSha256
    );

    var token = new JwtSecurityToken(
        claims: claims,
        expires: DateTime.UtcNow.AddHours(1),
        signingCredentials: credenciais
    );

    var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

    return Results.Json(new { token = tokenString });
});

app.Run();