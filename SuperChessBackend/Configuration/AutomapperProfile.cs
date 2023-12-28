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

            //CreateMap<User, UserLoginResponseDTO>()
            //    .ForMember(a => a.userId,
            //        s => s.MapFrom(b => b.id));




            //CreateMap<Item, ItemDTO>()
            //    .ForMember(a => a.steelType,
            //s => s.MapFrom(x => x.steelType.typeName));
            //CreateMap<CreateItemDTO, Item>()
            //    .ForMember(a => a.steelType, b => b.Ignore());
            //CreateMap<UpdateItemDTO, Item>()
            //    .ForMember(a => a.steelType, b => b.Ignore());



            //CreateMap<OrderItem, OrderItemDTO>();

            //CreateMap<Status, StatusDTO>().ReverseMap();

            //CreateMap<Order, OrderDTO>()
            //    .ForMember(a => a.orderingPerson,
            //        s => s.MapFrom(x => x.orderingUser.ToString()))
            //    .ForMember(a => a.status,
            //        s => s.MapFrom(x => x.status));
        }
    }
}
