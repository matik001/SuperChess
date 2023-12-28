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
        WaitingForMissingPlayer, /// when one player left the game
        Finished, 
        Aborted
    }

    public enum GameType {
        Chess
    }

    public enum ChessGameResult
    {
        WinWhite, WinBlack, Draw
    }

    public enum PlayerColors
    {
        White, Black
            /// If you add more games, you can add more colors here
    }
    public class ChessData
    {
        public string PositionPgn{ get; set; }
        public ChessGameResult? Result { get; set; }

        public ChessData()
        {
            PositionPgn = "";
            Result = null;
        }
    }

    [Table("game")]
    public class Game : IEntity
    {
        [Key]
        [Column("GameId")]
        public int Id { get; set; }
        public Guid GameGuid { get; set; }
        public DateTime CreationDate { get; set; }
        public GameState GameStatus { get; set; }
        public GameType GameType { get; set; }

        /// <summary>
        ///  EF nie radzi sobie z JObject, więc musimy zrobić to ręcznie
        /// </summary>
        public string GameData { get; set; }

        /// <summary>
        /// W zależności od typu gry, dane będą różne. W przypadku szachów jest to ChessData
        /// </summary>
        [NotMapped]
        private ChessData _chessData { get; set; } = null;

        [NotMapped]
        public ChessData ChessData  /// TODO sprawdzic czy prawidlowo dziala zapisywanie i aktualizacja tego pola
        {
            get
            {
                if(_chessData == null)
                {
                    _chessData = JObject.Parse(GameData).ToObject<ChessData>();
                }
                return _chessData;
            }
            set
            {
                GameData = JObject.FromObject(value).ToString();
                _chessData = value; 
            }
        }

        [ForeignKey("Room")]
        public int? RoomId { get; set; }
        public virtual Room? Room { get; set; }

        public virtual ICollection<UserGame> UserGames { get; set; }
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
