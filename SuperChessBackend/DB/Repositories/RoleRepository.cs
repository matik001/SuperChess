using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;

namespace SuperChessBackend.DB.Repositories
{
    [Table("role")]
    public class Role : IEntity
    {
        [NotMapped]
        public int Id { get => RoleId; }
        [Key]
        public int RoleId { get; set; }
        public string RoleName { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
    public enum RolesEnum
    {
        Admin = 1,
        Blocked = 2
    }

    public interface IRoleRepository : IGenericRepository<Role>
    {
        Task<Role> GetOrCreate(string roleName);
    }

    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        public RoleRepository(AppDBContext dbContext) : base(dbContext)
        {
        }

        public async Task<Role>GetOrCreate(string roleName)
        {
            var role = await _dbContext.Roles.AsNoTracking().FirstOrDefaultAsync(r => r.RoleName == roleName);
            if (role == null)
            {
                var newRole = new Role { RoleName = roleName };
                await this.Create(newRole);
                return newRole; 
            }
            return role;
        }


        public static void Seed(EntityTypeBuilder<Role> builder)
        {
            builder.HasData(
                new Role() { RoleId = (int)RolesEnum.Admin, RoleName = "Admin" },
                new Role() { RoleId = (int)RolesEnum.Blocked, RoleName = "Blocked" }
            );
        }
    }
}
