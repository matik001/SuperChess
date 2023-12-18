using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using SuperChessBackend.Configuration;
using SuperChessBackend.DB;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

var services = builder.Services;
// Add services to the container.
services.AddAutoMapper(typeof(AutomapperProfile));
services.AddScoped<IAppUnitOfWork, AppUnitOfWork>();
services.AddControllers().AddNewtonsoftJson();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.ConfigureJWT(builder.Configuration);
services.ConfigureCORS(builder.Configuration);

//AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
services.AddDbContext<AppDBContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();
app.ConfigureCORS();
// Configure the HTTP request pipeline.
if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.ConfigureExceptionHandler(builder.Configuration, app.Logger);
app.Run();
