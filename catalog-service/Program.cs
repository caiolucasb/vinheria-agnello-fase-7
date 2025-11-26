using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Mesma chave do Auth Service
var secretKey = "cKSik3qWxavw9AwsLCPlSKz5wh3MXyWoEvJDV7mgrvJUhBBdDOnLqLZxLWbE4Zxc";
var key = Encoding.ASCII.GetBytes(secretKey);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false; // apenas para simulação
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

app.MapGet("/catalog", () =>
{
    var produtos = new[]
    {
        new { id = 1, nome = "Vinho Tinto Reserva", safra = 2018, preco = 129.90, harmonizacao = "Carnes vermelhas" },
        new { id = 2, nome = "Vinho Branco Seco", safra = 2020, preco = 89.50, harmonizacao = "Queijos leves" },
        new { id = 3, nome = "Rosé Provence", safra = 2021, preco = 99.00, harmonizacao = "Frutos do mar" }
    };

    return Results.Json(produtos);
})
.RequireAuthorization(); // exige JWT válido

app.Run();
