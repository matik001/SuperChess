﻿using SuperChessBackend.DB.Repositories;

namespace SuperChessBackend.DB
{
    public interface IAppUnitOfWork : IDisposable
    {
        IUserRepository UserRepository { get; }
        IUserPasswordRepository UserPasswordRepository { get; }
        IRoleRepository RoleRepository { get; }
        IUserRoleRepository UserRoleRepository { get; }
        IGameRepository GameRepository { get; }
        IGameRepository UserGameRepository { get; }
        IRefreshTokenRepository RefreshTokenRepository { get; }
        IRevokedTokenRepository RevokedTokenRepository { get; }
        Task<int> SaveChangesAsync();
    }
    public class AppUnitOfWork : IAppUnitOfWork
    {
        private readonly AppDBContext _dbContext;
        public AppUnitOfWork(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        private IUserRepository _userRepository;
        public IUserRepository UserRepository
        {
            get
            {
                if (_userRepository == null)
                {
                    _userRepository = new UserRepository(_dbContext);
                }
                return _userRepository;
            }
        }
        private IUserPasswordRepository _userPasswordRepository;
        public IUserPasswordRepository UserPasswordRepository
        {
            get
            {
                if (_userPasswordRepository == null)
                {
                    _userPasswordRepository = new UserPasswordRepository(_dbContext);
                }
                return _userPasswordRepository;
            }
        }

        private IRoleRepository _roleRepository;
        public IRoleRepository RoleRepository
        {
            get
            {
                if (_roleRepository == null)
                {
                    _roleRepository = new RoleRepository(_dbContext);
                }
                return _roleRepository;
            }
        }
        private IUserRoleRepository _userRoleRepository;
        public IUserRoleRepository UserRoleRepository
        {
            get
            {
                if (_userRoleRepository == null)
                {
                    _userRoleRepository = new UserRoleRepository(_dbContext);
                }
                return _userRoleRepository;
            }
        } 
        private IGameRepository _gameRepository;
        public IGameRepository GameRepository
        {
            get
            {
                if (_gameRepository == null)
                {
                    _gameRepository = new GameRepository(_dbContext);
                }
                return _gameRepository;
            }
        }

        private IGameRepository _userGameRepository;
        public IGameRepository UserGameRepository
        {
            get
            {
                if (_userGameRepository == null)
                {
                    _userGameRepository = new GameRepository(_dbContext);
                }
                return _userGameRepository;
            }
        }
        private IRefreshTokenRepository _refreshTokenRepository;

        public IRefreshTokenRepository RefreshTokenRepository
        {
            get
            {
                if (_refreshTokenRepository == null)
                {
                    _refreshTokenRepository = new RefreshTokenRepository(_dbContext);
                }
                return _refreshTokenRepository;
            }
        }
        private IRevokedTokenRepository _revokedTokenRepository;

        public IRevokedTokenRepository RevokedTokenRepository
        {
            get
            {
                if (_revokedTokenRepository == null)
                {
                    _revokedTokenRepository = new RevokedTokenRepository(_dbContext);
                }
                return _revokedTokenRepository;
            }
        }
        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }  
}