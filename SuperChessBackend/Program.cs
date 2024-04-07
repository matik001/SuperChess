using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using SuperChessBackend.Configuration;
using SuperChessBackend.DB;
using SuperChessBackend.Hubs;
using SuperChessBackend.Services;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((context, loggerConfiguration) => loggerConfiguration
    .ReadFrom.Configuration(context.Configuration));

var services = builder.Services;
// Add services to the container.
services.AddAutoMapper(typeof(AutomapperProfile));
services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

//AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
services.AddDbContext<AppDBContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseMySQL(connectionString);
});

services.AddScoped<IAppUnitOfWork, AppUnitOfWork>();
services.AddScoped<IAuthService, AuthService>();
services.AddScoped<IChessService, ChessService>();
services.AddControllers().AddNewtonsoftJson(options =>
        options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter()));

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.ConfigureJWT(builder.Configuration);
services.ConfigureCORS(builder.Configuration);
services.AddSignalR()
        .AddJsonProtocol(options =>
        {
            options.PayloadSerializerOptions.Converters
               .Add(new JsonStringEnumConverter());
        });
;
services.AddScoped<IGamesHubService, GamesHubService>();



var app = builder.Build();

/// for reverse proxy (nginx)
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.ConfigureCORS();
// Configure the HTTP request pipeline.
if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers().RequireAuthorization();
app.MapHub<GamesHub>("/hub/games");

app.ConfigureExceptionHandler(builder.Configuration, app.Logger);

app.Run();
