using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace SuperChessBackend.Configuration
{
    public static class AuthPolicy
    {
        public const string AdminOnly = "AdminOnly";
    }
    public static class JWTConfig
    {
        public static string Issuer { get; private set; }
        public static string Audience { get; private set; }
        public static string Key { get; private set; }
        public static int LifetimeInMinutes { get; private set; }

        public static void ConfigureJWT(this IServiceCollection services, IConfigurationManager configuration)
        {
            var section = configuration.GetSection("JWT");
            if(section == null)
            {
                throw new Exception("JWT section not found in appsettings.json");
            }
            Issuer = section.GetValue<string>("Issuer") ?? "";
            Audience = section.GetValue<string>("Audience") ?? "";
            Key = section.GetValue<string>("Key") ?? "";
            LifetimeInMinutes = section.GetValue<int>("LifetimeInMinutes");

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        ValidIssuer = Issuer,
                        ValidAudience = Audience,
                        ClockSkew = TimeSpan.Zero,
                        IssuerSigningKey = new SymmetricSecurityKey(
                             Encoding.UTF8.GetBytes(Key))
                    };

                });

            services.AddAuthorization(options => {
                
                /// Users must be authenticated to access any endpoint
                options.FallbackPolicy = new AuthorizationPolicyBuilder() 
                     .RequireAuthenticatedUser()
                     .Build();

                options.AddPolicy(AuthPolicy.AdminOnly, policy => {
                    policy.RequireClaim("Admin", true.ToString());
                });
            });
        }
    }
}
