using API;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 1 - Carregar configura��es JWT do appsettings.json
//var jwtSection = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.UTF8.GetBytes(JwtSettings.Key);

// 2 - Adicionar autentica��o JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = JwtSettings.Issuer,
        ValidAudience = JwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy
            .WithOrigins("http://localhost:8080") // libere seus frontends
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials(); // se for necess�rio (cookies ou auth)
    });
});

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("CorsPolicy", policy =>
//    {
//        policy
//            .AllowAnyOrigin()
//            .AllowAnyMethod()
//            .AllowAnyHeader();
//    });
//});


builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

// 3 - Registrar os servi�os da API
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "Autentica��o JWT usando o esquema Bearer.\n\nExemplo: 'Bearer {token}'",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });

});

var app = builder.Build();

app.UseCors("CorsPolicy");

// 4 - Configurar pipeline HTTP
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

// 5 - Ativar autentica��o e autoriza��o
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
