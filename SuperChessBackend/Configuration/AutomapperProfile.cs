using AutoMapper;
using SuperChessBackend.DB.Repositories;
using SuperChessBackend.DTOs;

namespace SuperChessBackend.Configuration
{
    public class AutomapperProfile : Profile
    {

        public AutomapperProfile()
        {
            CreateMap<Role, RoleDTO>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<Room, RoomDTO>().ReverseMap();
            CreateMap<Game, GameDTO>().ReverseMap();
            CreateMap<UserGame, UserGameDTO>().ReverseMap();
        }
    }
}
