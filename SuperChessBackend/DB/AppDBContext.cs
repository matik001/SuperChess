using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using SuperChessBackend.DB.Repositories;
using System.Reflection.Emit;

namespace SuperChessBackend.DB
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserPassword> UserPasswords { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<UserGame> UserGames { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<RevokedToken> RevokedTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.Entity<User>()
                    .Property(a => a.CreationDate)
                    .HasConversion(v => v,
                        v => new DateTime(v.Ticks, DateTimeKind.Utc));
            builder.Entity<Game>()
                    .Property(a => a.CreationDate)
                    .HasConversion(v => v,
                        v => new DateTime(v.Ticks, DateTimeKind.Utc));

            builder.Entity<RefreshToken>()
                    .Property(a => a.CreationDate)
                    .HasConversion(v => v,
                        v => new DateTime(v.Ticks, DateTimeKind.Utc));
            
            builder.Entity<RefreshToken>()
                    .Property(a => a.ExpirationDate)
                    .HasConversion(v => v,
                        v => new DateTime(v.Ticks, DateTimeKind.Utc));

            builder.Entity<RevokedToken>()
                    .Property(a => a.ExpirationDate)
                    .HasConversion(v => v,
                        v => new DateTime(v.Ticks, DateTimeKind.Utc));


            /////
            ///// relations
            /////

            builder.Entity<User>()
                .HasMany(e => e.Roles)
                .WithMany(e => e.Users)
                //.UsingEntity<UserRole>(
                //    l => l.HasOne<Role>().WithMany(e => e.UserRoles).HasForeignKey(e => e.RoleId),
                //    r => r.HasOne<User>().WithMany(e => e.UserRoles).HasForeignKey(e => e.UserId)
                //)
                ;
            builder.Entity<User>()
              .HasMany(e => e.Games)
              .WithMany(e => e.Users)
              //.UsingEntity<UserGame>(
              //    l => l.HasOne<Game>().WithMany(e => e.UserGame).HasForeignKey(e => e.GameId),
              //    r => r.HasOne<User>().WithMany(e => e.UserGame).HasForeignKey(e => e.UserId)
              //)
              ;
            builder.Entity<User>()
                .HasMany(e => e.UserPasswords)
                .WithOne(e => e.User);

            builder.Entity<User>()
                .HasMany(e => e.RefreshTokens)
                .WithOne(e => e.User);

            builder.Entity<User>()
                .HasMany(e => e.RevokedTokens)
                .WithOne(e => e.User);


            


            /////
            ///// Seeds
            /////
            ///
            RoleRepository.Seed(builder.Entity<Role>());
        }
    }
}
