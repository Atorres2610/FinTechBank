using Cliente.API.Middlewares;
using Cliente.Infrastructure;
using Cliente.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
string corsPolicy = "AppReactCorsPolicy";

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    //Configuración para que Swagger solicite el JWT
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Introduzca el token de la siguiente manera: 'Bearer {su token}'",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Name = JwtBearerDefaults.AuthenticationScheme,
                In = ParameterLocation.Header,
                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
});

builder.Services.AddInfrastructure(configuration);

//Configuración para poder usar JWT
builder.Services.AddAuthentication(configure =>
{
    configure.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    configure.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false, //Emisor
            ValidateAudience = false, //Receptor
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]!)),
            ClockSkew = TimeSpan.Zero
        };
    });

//Configuración para que la web se pueda comunicar con el API
builder.Services.AddCors(setup =>
{
    setup.AddPolicy(corsPolicy, policy =>
    {
        policy.WithOrigins(configuration["UrlWeb"]!).AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();

//Creación de la base de datos con sus respectivas tablas en caso no exista
using (var servicioScope = app.Services.CreateScope())
{
    var dataSeed = servicioScope.ServiceProvider.GetRequiredService<DataSeed>();
    dataSeed.EjecutarAsync().Wait();
}

if (!app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseMiddleware<ExceptionMiddleware>();

app.UseCors(corsPolicy);

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();