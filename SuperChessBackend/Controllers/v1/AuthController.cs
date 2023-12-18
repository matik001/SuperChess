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
    public class AuthController : ControllerBase
    {
        private readonly IAppUnitOfWork uow;
        private readonly IMapper mapper;
        private readonly IAuthService service;

        public AuthController(IAppUnitOfWork uow, IMapper mapper, IAuthService service)
        {
            this.uow = uow;
            this.mapper = mapper;
            this.service = service;
        }

        [AllowAnonymous]
        [HttpPost("signin")]
        [ProducesResponseType<UserTokensDTO>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<UserTokensDTO> SignIn(UserSignInRequestDTO userDto)
        {
            return await service.SignIn(userDto);
        }

        [AllowAnonymous]
        [HttpPost("signup")]
        [ProducesResponseType<UserTokensDTO>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<UserTokensDTO> SignUp(UserSignUpRequestDTO userDto)
        {
            return await service.SignUp(userDto);
        }

        [HttpPost("logout")]
        public async Task Logout()
        {
            await service.Logout();
        }

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        [ProducesResponseType<UserTokensDTO>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<UserTokensDTO> RefreshToken(RefreshTokenRequstDTO refreshToken)
        {
            return await service.RefreshToken(refreshToken);
        }
    }
}
