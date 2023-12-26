using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;

namespace SuperChessBackend.DB.Repositories
{
    

    [Table("room")]
    public class Room : IEntity
    {
        [Key]
        [Column("RoomId")]
        public int Id { get; set; }
        public Guid RoomName { get; set; }
        public DateTime CreationDate { get; set; }

        public virtual ICollection<Game> Games { get; set; }
    }
    public interface IRoomRepository : IGenericRepository<Room>
    {
    }
    public class RoomRepository : GenericRepository<Room>, IRoomRepository
    {
        public RoomRepository(AppDBContext dbContext) : base(dbContext)
        {
        }
    }
}
