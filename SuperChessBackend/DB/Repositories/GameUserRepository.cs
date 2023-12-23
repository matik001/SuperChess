using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;

namespace SuperChessBackend.DB.Repositories
{
    [Table("gameuser")]
    public class UserGame: IEntity
    {
        [Key]
        [Column("UserGameId")]
        public int Id { get; set; }

        public string Color { get; set; } /// may be something more than just "white" or "black"

        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User User { get; set; }

        [ForeignKey("Game")]
        public int GameId { get; set; }
        public virtual Game Game { get; set; }

    }
    public interface IUserGameRepository : IGenericRepository<UserGame>
    {
    }
    public class UserGameRepository : GenericRepository<UserGame>, IUserGameRepository
    {
        public UserGameRepository(AppDBContext dbContext) : base(dbContext)
        {
        }
    }
}
