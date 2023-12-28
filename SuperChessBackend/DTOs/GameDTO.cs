using SuperChessBackend.DB.Repositories;

namespace SuperChessBackend.DTOs
{
    public class GameDTO
    {
        public Guid GameGuid { get; set; }
        public DateTime CreationDate { get; set; }
        public GameState GameStatus { get; set; }
        public GameType GameType { get; set; }
        public ChessData ChessData { get; set; }
        public int RoomId { get; set; }
        public List<UserGameDTO> UserGames { get; set; }

    }
}
