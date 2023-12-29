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

        public PlayerColors Color { get; set; }
        public string Nick { get; set; }
        public string? GuestGuid { get; set; } /// In case player is guest, we store his guid

        [ForeignKey("User")]
        public int? UserId { get; set; }
        public virtual User? User { get; set; }

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
