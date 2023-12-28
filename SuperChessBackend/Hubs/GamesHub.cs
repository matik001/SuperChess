using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SuperChessBackend.DB;
using SuperChessBackend.DB.Repositories;
using SuperChessBackend.DTOs;

namespace SuperChessBackend.Hubs
{
    public interface IGamesHubService
    {
        Task JoinRoom(string connId, int roomId);
        Task LeaveRoom(string connId, int roomId);
        Task CreateGame(string connId, int? userId, string nick, GameType gameType, int roomId);
        Task UpdateGamesInRoom(int roomId);
        Task UpdateGamesInRoom(int roomId, string connId);

    }
    public class GamesHubService : IGamesHubService
    {
        private readonly IAppUnitOfWork uow;
        private readonly IMapper mapper;
        private readonly IHubContext<GamesHub> context;

        public GamesHubService(IAppUnitOfWork uow, IHubContext<GamesHub> context, IMapper mapper)
        {
            this.context = context;
            this.uow = uow;
            this.mapper = mapper;

        }
        private string _roomIdToGroupName(int roomId)
        {
            return $"Room:{roomId}";
        }
        private string _gameIdToGroupName(int gameId)
        {
            return $"Game:{gameId}";
        }
        public async Task JoinRoom(string connId, int roomId)
        {
            var group = _roomIdToGroupName(roomId);
            var client = context.Clients.Client(connId);
            await context.Groups.AddToGroupAsync(connId, group);
        }
        public async Task LeaveRoom(string connId, int roomId)
        {
            var group = _roomIdToGroupName(roomId);
            var client = context.Clients.Client(connId);
            await context.Groups.RemoveFromGroupAsync(connId, group);
        }
        public async Task CreateGame(string connId, int? userId, string nick, GameType gameType, int roomId)
        {
            var newGame = new Game
            {
                RoomId = roomId,
                GameGuid = Guid.NewGuid(),
                CreationDate = DateTime.UtcNow,
                GameType = gameType,
                GameStatus = GameState.NotStarted,
                UserGames = new List<UserGame>()
                {
                    new UserGame(){
                        UserId = userId,
                        Color = PlayerColors.White,
                        Nick = nick
                    }
                },
                ChessData = new ChessData()
            };
            await uow.GameRepository.Create(newGame);
            await uow.SaveChangesAsync();
        }
        List<GameDTO> _getGamesInRoom(int roomId)
        {
            return uow.GameRepository.GetAll()
                                       .Include(game => game.UserGames)
                                       .ThenInclude(userGame => userGame.User)
                                       .Where(game => game.RoomId == roomId && game.GameStatus == GameState.NotStarted || game.GameStatus == GameState.InProgress)
                                       .OrderByDescending(game => game.CreationDate)
                                       .Select(game => mapper.Map<GameDTO>(game))
                                       .ToList(); 
        }
        public async Task UpdateGamesInRoom(int roomId)
        {
            var groupName = _roomIdToGroupName(roomId);
            var group = context.Clients.Group(groupName);
            var games = _getGamesInRoom(roomId);
            await group.SendAsync("UpdateGamesInRoom", games);
        }
        public async Task UpdateGamesInRoom(int roomId, string connId)
        {
            var games = _getGamesInRoom(roomId);
            await context.Clients.Client(connId).SendAsync("UpdateGamesInRoom", games);
        }
    }
    public class GamesHub : Hub
    {
        private readonly IGamesHubService hubService;

        public GamesHub(IGamesHubService hubService)
        {
            this.hubService = hubService;
        }

        public override Task OnConnectedAsync()
        {
            var userId = this.Context.UserIdentifier;
            return base.OnConnectedAsync();
        }
        public async Task JoinRoom(int roomId)
        {
            var connId = this.Context.ConnectionId;
            await hubService.JoinRoom(connId, roomId);
            await hubService.UpdateGamesInRoom(roomId, connId);
        }
        public async Task LeaveRoom(int roomId)
        {
            var connId = this.Context.ConnectionId;
            await hubService.LeaveRoom(connId, roomId);
        }
        public async Task CreateGame(int roomId, GameType gameType, string nick)
        {
            var connId = this.Context.ConnectionId;
            var userId = this.Context.UserIdentifier;
            await hubService.CreateGame(connId, userId != null ? int.Parse(userId) : null, nick, gameType, roomId);
            await hubService.UpdateGamesInRoom(roomId);
        }
    }
}
