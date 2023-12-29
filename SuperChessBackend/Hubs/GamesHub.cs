using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SuperChessBackend.DB;
using SuperChessBackend.DB.Repositories;
using SuperChessBackend.DTOs;
using SuperChessBackend.Services;

namespace SuperChessBackend.Hubs
{
    public interface IGamesHubService
    {
        Task JoinRoom(string connId, int roomId);
        Task JoinGame(string connId, int? userId, string? guestGuid, string nick, string gameGuid);
        Task LeaveRoom(string connId, int roomId);
        Task<string> CreateGame(string connId, int? userId, string nick, GameType gameType, int roomId, string? guestGuid);
        Task UpdateGamesInRoom(int roomId);
        Task UpdateGamesInRoom(int roomId, string connId);
        Task UpdateGame(string gameGuid);
        Task MakeMove(string connId, int? userId, string? guestGuid, string gameGuid, Move move);

    }
    public class GamesHubService : IGamesHubService
    {
        private readonly IAppUnitOfWork uow;
        private readonly IMapper mapper;
        private readonly IChessService chessService;
        private readonly IHubContext<GamesHub> context;

        public GamesHubService(IAppUnitOfWork uow, IHubContext<GamesHub> context, IMapper mapper, IChessService chessService)
        {
            this.context = context;
            this.uow = uow;
            this.mapper = mapper;
            this.chessService = chessService;
        }
        private string _roomIdToGroupName(int roomId)
        {
            return $"Room:{roomId}";
        }
        private string _gameGuidToGroupName(string gameGuid)
        {
            return $"Game:{gameGuid}";
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
        public async Task<string> CreateGame(string connId, int? userId, string nick, GameType gameType, int roomId, string? guestGuid)
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
                        Nick = nick,
                        GuestGuid = guestGuid
                    }
                },
                ChessData = new ChessData()
            };
            await uow.GameRepository.Create(newGame);
            await uow.SaveChangesAsync();

            var groupName = _gameGuidToGroupName(newGame.GameGuid.ToString());
            await context.Groups.AddToGroupAsync(connId, groupName);
            return newGame.GameGuid.ToString();
        }

        public async Task JoinGame(string connId, int? userId, string? guestGuid, string nick, string gameGuid)
        {
            var game = uow.GameRepository.GetGameByGuid(gameGuid);
            if (game == null)
            {
                throw new Exception("Game not found");
            }
            var groupName = _gameGuidToGroupName(gameGuid);
            bool isAlreadyInGame = game.UserGames.Any(ug => userId != null && ug.UserId == userId || 
                                                         guestGuid != null && ug.GuestGuid == guestGuid);

            if(isAlreadyInGame || game.GameStatus != GameState.NotStarted) /// player can also observe game without playing, or reconnect to game
            {
                await context.Groups.AddToGroupAsync(connId, groupName);
                return;
            }
            game.UserGames.Add(new UserGame()
            {
                GuestGuid = guestGuid,
                UserId = userId,
                Color = PlayerColors.Black,
                Nick = nick,
            });
            game.GameStatus = GameState.InProgress;
            await uow.GameRepository.Update(game);
            await uow.SaveChangesAsync();

            await context.Groups.AddToGroupAsync(connId, groupName);
        }
        public async Task MakeMove(string connId, int? userId, string? guestGuid, string gameGuid, Move move)
        {
            var game = uow.GameRepository.GetGameByGuid(gameGuid);
            if(game == null)
            {
                throw new Exception("Game not found");
            }
            var newState = chessService.MakeMove(game.ChessData, move);
            game.ChessData = newState;
            if(newState.Result != null)
                game.GameStatus = GameState.Finished;
            
            await uow.GameRepository.Update(game);
            await uow.SaveChangesAsync();

            var groupName = _gameGuidToGroupName(gameGuid);
            await context.Groups.AddToGroupAsync(connId, groupName);
        }
        /// SENDING -------------------------------------
        public async Task UpdateGamesInRoom(int roomId)
        {
            var groupName = _roomIdToGroupName(roomId);
            var group = context.Clients.Group(groupName);
            var games = uow.GameRepository.GetGamesInRoom(roomId)
                                          .Select(g=>mapper.Map<GameDTO>(g));

            await group.SendAsync("UpdateGamesInRoom", games);
        }
        public async Task UpdateGamesInRoom(int roomId, string connId)
        {
            var games = uow.GameRepository.GetGamesInRoom(roomId)
                                          .Select(g => mapper.Map<GameDTO>(g));
            await context.Clients.Client(connId).SendAsync("UpdateGamesInRoom", games);
        }
        public async Task UpdateGame(string gameGuid)
        {
            var game = uow.GameRepository.GetGameByGuid(gameGuid);
            var gameDto = mapper.Map<GameDTO>(game);

            int roomId = game.RoomId ?? throw new Exception("Game is not in any room");
            var gameGroupName = _gameGuidToGroupName(gameGuid);
            var roomGroupName = _roomIdToGroupName(roomId);
            await context.Clients.Group(gameGroupName).SendAsync("UpdateGame", gameDto);
            await context.Clients.Group(roomGroupName).SendAsync("UpdateGame", gameDto);
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
        public async Task<string> CreateGame(string? guestGuid, string nick, int roomId, GameType gameType)
        {
            var connId = this.Context.ConnectionId;
            var userId = this.Context.UserIdentifier;
            var gameGuid = await hubService.CreateGame(connId, userId != null ? int.Parse(userId) : null, nick, gameType, roomId, guestGuid);
            await hubService.UpdateGamesInRoom(roomId);
            return gameGuid;
        }

        public async Task JoinGame(string? guestGuid, string nick, string gameGuid)
        {
            var connId = this.Context.ConnectionId;
            var userId = this.Context.UserIdentifier;

            await hubService.JoinGame(connId, userId != null ? int.Parse(userId) : null, guestGuid, nick, gameGuid);
            await hubService.UpdateGame(gameGuid);
        }

        public async Task MakeMove(string? guestGuid, string gameGuid, Move move)
        {
            var connId = this.Context.ConnectionId;
            var userId = this.Context.UserIdentifier;

            await hubService.MakeMove(connId, userId != null ? int.Parse(userId) : null, guestGuid, gameGuid, move);
            await hubService.UpdateGame(gameGuid);
        }
    }
}
