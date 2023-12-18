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
    public class UserController : ControllerBase
    {
        private readonly IAppUnitOfWork uow;
        private readonly IMapper mapper;
        private readonly IAuthService service;

        public UserController(IAppUnitOfWork uow, IMapper mapper, IAuthService service)
        {
            this.uow = uow;
            this.mapper = mapper;
            this.service = service;
        }

    }
}
