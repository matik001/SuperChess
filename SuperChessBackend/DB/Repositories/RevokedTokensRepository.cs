using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SuperChessBackend.DB.Repositories
{
    [Table("revokedtokens")]
    public class RevokedToken : IEntity
    {
        [NotMapped]
        public int Id { get => RevokedTokenId; }
        [Key]
        public int RevokedTokenId { get; set; }
        public string TokenGuid { get; set; }
        public DateTime ExpirationDate { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User User { get; set; }

    }
    public interface IRevokedTokenRepository : IGenericRepository<RevokedToken>
    {
    }
    public class RevokedTokenRepository : GenericRepository<RevokedToken>, IRevokedTokenRepository
    {
        public RevokedTokenRepository(AppDBContext dbContext) : base(dbContext)
        {
        }

    }
}
