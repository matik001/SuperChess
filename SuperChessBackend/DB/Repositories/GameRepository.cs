using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using SuperChessBackend.DTOs;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;

namespace SuperChessBackend.DB.Repositories
{
    public enum GameState
    {
        NotStarted,
        InProgress,
        WaitingForMissingPlayer, /// when one player left the game
        Finished,
        Aborted
    }

    public enum GameType
    {
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
        public string PositionFEN { get; set; }
        public ChessGameResult? Result { get; set; }
        public static readonly string START_FEN = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
        public ChessData()
        {
            PositionFEN = START_FEN;
            Result = null;
        }
    }

    /// it may be more efficient to store games history in another table
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
        IEnumerable<Game> GetGamesInRoom(int roomId);
        Game? GetGameByGuid(string gameGuid);
        IEnumerable<Game> GetUserGames(int userId);

    }
    public class GameRepository : GenericRepository<Game>, IGameRepository
    {
        public GameRepository(AppDBContext dbContext) : base(dbContext)
        {
        }

        public IEnumerable<Game> GetGamesInRoom(int roomId)
        {
            return GetAll()
                    .Include(game => game.UserGames)
                    .ThenInclude(userGame => userGame.User)
                    .Where(game => game.RoomId == roomId && (game.GameStatus == GameState.NotStarted || game.GameStatus == GameState.InProgress))
                    .OrderByDescending(game => game.CreationDate)
                    .ToList();
        }


        Game? IGameRepository.GetGameByGuid(string gameGuid)
        {
            return GetAll()
                    .Include(game => game.UserGames)
                    .ThenInclude(userGame => userGame.User)
                    .Where(game => game.GameGuid == new Guid(gameGuid))
                    .FirstOrDefault();
        }

      

        IEnumerable<Game> IGameRepository.GetUserGames(int userId)
        {
            return GetAll()
                    .Include(game => game.UserGames)
                    .ThenInclude(userGame => userGame.User)
                    .Where(game => game.UserGames.Any(a => a.UserId == userId))
                    .OrderByDescending(game => game.CreationDate)
                    .ToList();
        }
    }
}
