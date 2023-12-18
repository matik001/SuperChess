using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;

namespace SuperChessBackend.DB.Repositories
{
    public enum GameState {
        NotStarted, 
        InProgress, 
        WaitingForMissingPlayer, 
        Finished, 
        Aborted
    }

    public enum GameType {
        ClassicChess
    }

    public enum ChessGameResult
    {
        WinWhite, WinBlack, Draw
    }
    public class ChessData
    {
        public string PositionPgn{ get; set; }
        public ChessGameResult? Result { get; set; }
    }

    [Table("game")]
    public class Game : IEntity
    {
        [NotMapped]
        public int Id { get => GameId; }
        [Key]
        public int GameId { get; set; }
        public Guid GameGuid { get; set; }
        public DateTime CreationDate { get; set; }
        public GameState GameStatus { get; set; }
        public GameType GameType { get; set; }

        /// <summary>
        /// W zależności od typu gry, dane będą różne. W przypadku szachów jest to ChessData
        /// </summary>
        public JObject GameData { get; set; }

        [NotMapped]
        private ChessData _chessData { get; set; } = null;
        [NotMapped]
        public ChessData ChessData 
        {
            get
            {
                if(_chessData == null)
                {
                    _chessData = GameData.ToObject<ChessData>();
                }
                return _chessData;
            }
            set
            {
                GameData = JObject.FromObject(value);
                _chessData = value; 
            }
        }

        public virtual ICollection<UserGame> UserGame { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
    public interface IGameRepository : IGenericRepository<Game>
    {
    }
    public class GameRepository : GenericRepository<Game>, IGameRepository
    {
        public GameRepository(AppDBContext dbContext) : base(dbContext)
        {
        }
    }
}
