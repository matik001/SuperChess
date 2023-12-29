using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SuperChessBackend.DB;
using SuperChessBackend.DTOs;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SuperChessBackend.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    /// niepotrzebne, doda sie automatycznie
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] 
    public class GameController : ControllerBase
    {
        private readonly IAppUnitOfWork uow;
        private readonly IMapper mapper;

        public GameController(IAppUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }
        // GET: api/<GameController>
        [HttpGet]
        public IEnumerable<GameDTO> Get()
        {
            var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userId = int.Parse(userIdStr);
            var games = uow.GameRepository.GetUserGames(userId).ToList();
            var gamesDto = mapper.Map<List<GameDTO>>(games);
            return gamesDto;
        }

        //// GET api/<GameController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<GameController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<GameController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<GameController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
