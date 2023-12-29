using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SuperChessBackend.Configuration;
using SuperChessBackend.DB;
using SuperChessBackend.DB.Repositories;
using SuperChessBackend.DTOs;
using SuperChessBackend.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SuperChessBackend.Controllers.v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class RoomController : ControllerBase
    {
        private readonly IAppUnitOfWork uow;
        private readonly IMapper mapper;
        private readonly IAuthService service;

        public RoomController(IAppUnitOfWork uow, IMapper mapper, IAuthService service)
        {
            this.uow = uow;
            this.mapper = mapper;
            this.service = service;
        }

        // GET: api/<GameController>
        [HttpGet]
        [AllowAnonymous]
        public IEnumerable<RoomExtendedDTO> Get()
        {
            var rooms = uow.RoomRepository.GetAll()
                .Select(room => new RoomExtendedDTO()
                {
                    Id = room.Id,
                    CreationDate = room.CreationDate,
                    RoomName = room.RoomName,
                    AmountOfUsers = room.Games.Where(x => x.GameStatus == GameState.NotStarted 
                                    || x.GameStatus == GameState.InProgress).Count() /// EF is smart and it should be performent
                })
                .OrderByDescending(room => room.CreationDate)
                .ToList();
            return rooms;
        }

        // GET api/<GameController>/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public RoomExtendedDTO Get(int id)
        {
            var room = uow.RoomRepository.GetAll().Where(a => a.Id == id).Select(room => new RoomExtendedDTO()
            {
                Id = room.Id,
                CreationDate = room.CreationDate,
                RoomName = room.RoomName,
                AmountOfUsers = room.Games.Count() /// EF is smart and it should be performent
            }).FirstOrDefault();

            return room;
        }

        // POST api/<GameController>
        [HttpPost]
        [AllowAnonymous]
        public async Task<RoomDTO> Post([FromBody] RoomCreateDTO room)
        {
            var newRoom = new Room()
            {
                RoomName = room.RoomName,
                CreationDate = DateTime.UtcNow
            };
            await uow.RoomRepository.Create(newRoom);
            await uow.SaveChangesAsync();
            var roomDto = mapper.Map<RoomDTO>(newRoom);
            return roomDto;
        }

        // PUT api/<GameController>/5
        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] RoomDTO room)
        {
            await uow.RoomRepository.Update(new Room()
            {
                Id = id,
                RoomName = room.RoomName,
                CreationDate = DateTime.UtcNow
            });
            await uow.SaveChangesAsync();
        }

        // DELETE api/<GameController>/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await uow.RoomRepository.Delete(id);
            await uow.SaveChangesAsync();
        }

    }
}
