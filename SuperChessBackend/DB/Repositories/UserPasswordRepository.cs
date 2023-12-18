using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SuperChessBackend.DB.Repositories
{
    [Table("userpassword")]
    public class UserPassword : IEntity
    {
        [NotMapped]
        public int Id { get => PasswordId; }
        [Key]
        public int PasswordId { get; set; }
        public string PasswordHash { get; set; }
        public DateTime PasswordDate { get; set; }

        [ForeignKey("User")]
        public int PasswordUserId { get; set; }
        public virtual User User { get; set; }



    }
    public interface IUserPasswordRepository : IGenericRepository<UserPassword>
    {
    }
    public class UserPasswordRepository : GenericRepository<UserPassword>, IUserPasswordRepository
    {
        public UserPasswordRepository(AppDBContext dbContext) : base(dbContext)
        {
        }
    }
}
