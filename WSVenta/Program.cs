using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WSVenta.Models;
using WSVenta.Models.Common;
using WSVenta.Services;
using WSVenta.Tools;

const string myAllowSpecificOrigins = "_myAllowSpecificOrigins ";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigins,
                        policy =>
                        {
                            //policy.WithHeaders("*");
                            policy.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
                        });
});


builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new IntToStringConverter());
        options.JsonSerializerOptions.Converters.Add(new DecimalToStringConverter());   
    });
var appSettingsSection = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingsSection);
var appSettings = appSettingsSection.Get<AppSettings>();
if (appSettings is { Secreto: not null })
{
    var llave = Encoding.ASCII.GetBytes(appSettings.Secreto);
    builder.Services.AddAuthentication(d =>
    {
        d.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        d.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(d =>
        {
            d.RequireHttpsMetadata = false;
            d.SaveToken = true;
            d.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(llave),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });
}

var connectionString = builder.Configuration.GetConnectionString("VentaRealConnection");
builder.Services.AddDbContext<VentaRealContext>(options =>
{
    options.UseSqlServer(connectionString);
});
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IVentaService, VentaService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

app.UseHttpsRedirection();

app.UseCors(myAllowSpecificOrigins);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
