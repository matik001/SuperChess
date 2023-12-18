using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace SuperChessBackend.Configuration
{
    public static class CORSPolicy
    {
        public const string AllowAll = "AllowAll";
        public const string AllowSpecific = "AllowSpecific";
    }
    public static class CORSConfig
    {
        public static bool AllowAll { get; private set; }
        public static string[] AllowedOrigins { get; private set; }
        public static void ConfigureCORS(this IServiceCollection services, IConfigurationManager configuration)
        {
            var section = configuration.GetSection("CORS");
            if(section == null)
            {
                throw new Exception("CORS section not found in appsettings.json");
            }
            AllowAll = section.GetValue<bool>("AllowAll");
            AllowedOrigins = section.GetValue<string[]>("AllowedOrigins");

            services.AddCors(options =>
            {
                options.AddPolicy(CORSPolicy.AllowAll,
                    p =>p.AllowAnyOrigin()
                         .AllowAnyMethod()
                         .AllowAnyHeader()
                         .AllowCredentials()
                    );

                options.AddPolicy(CORSPolicy.AllowSpecific,
                    p => p.WithOrigins(AllowedOrigins)
                          .AllowAnyMethod()
                          .AllowCredentials()
                          .AllowAnyHeader()
                    );
            });
        }
        public static void ConfigureCORS(this IApplicationBuilder app)
        {
            app.UseCors(AllowAll ? CORSPolicy.AllowAll : CORSPolicy.AllowSpecific);
        }
    }
}
