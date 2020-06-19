﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using CSharpFunctionalExtensions;
using FluentValidation;
using FluentValidation.Results;
using JetBrains.Annotations;
using Kbalan.TouchType.Data.Contexts;
using Kbalan.TouchType.Data.Models;
using Kbalan.TouchType.Logic.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kbalan.TouchType.Logic.Services
{
    public class UserService : IUserService
    {
        private readonly TouchTypeGameContext _gameContext;
        private readonly IMapper _mapper;

        public UserService([NotNull]TouchTypeGameContext gameContext, [NotNull]IMapper mapper)
        {
            this._gameContext = gameContext;
            this._mapper = mapper;
    }

        /// <summary>
        /// Implementation of IUserService GetAll() method
        /// </summary>
        /// <returns></returns>
        public Result<IEnumerable<UserSettingStatisticDto>> GetAll()
        {
            try
            {
                var getAllResult = _gameContext.Users.ProjectToArray<UserSettingStatisticDto>(_mapper.ConfigurationProvider);
                return Result.Success<IEnumerable<UserSettingStatisticDto>>(getAllResult);
            }
            catch (DbUpdateException ex)
            {
                return Result.Failure<IEnumerable<UserSettingStatisticDto>>(ex.Message);
            }
        }
        public async Task<Result<IEnumerable<UserSettingStatisticDto>>> GetAllAsync ()
        {
            try
            {
                var getAllResult = await _gameContext.Users
                    .ProjectToArrayAsync<UserSettingStatisticDto>(_mapper.ConfigurationProvider)
                    .ConfigureAwait(false);
                return Result.Success<IEnumerable<UserSettingStatisticDto>>(getAllResult);
            }
            catch (DbUpdateException ex)
            {
                return Result.Failure<IEnumerable<UserSettingStatisticDto>>(ex.Message);
            }
        }

        /// <summary>
        /// Implementation of GetById()
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Result<Maybe<UserSettingStatisticDto>> GetById(int id)
        {
            try
            {
                Maybe<UserSettingStatisticDto> getResultById = _gameContext.Users.Where(x => x.Id == id)
                    .ProjectToSingleOrDefault<UserSettingStatisticDto>(_mapper.ConfigurationProvider);

                    return Result.Success(getResultById);
            }
            catch (SqlException ex)
            {
                return Result.Failure<Maybe<UserSettingStatisticDto>>(ex.Message);
            }
        }
        public async Task<Result<Maybe<UserSettingStatisticDto>>> GetByIdAsync(int id)
        {
            try
            {
                Maybe<UserSettingStatisticDto> getResultById = await _gameContext.Users.Where(x => x.Id == id)
                    .ProjectToSingleOrDefaultAsync<UserSettingStatisticDto>(_mapper.ConfigurationProvider)
                    .ConfigureAwait(false);

                return Result.Success(getResultById);
            }
            catch (SqlException ex)
            {
                return Result.Failure<Maybe<UserSettingStatisticDto>>(ex.Message);
            }
        }

        /// <summary>
        /// Add new user to RegisterUserDto collection. If user with such id or name exist's null will be
        /// returned
        /// </summary>
        /// <param name="model">RegisterUserDto model</param>
        /// <returns>New User or null</returns>
        public Result<UserSettingDto> Add(UserSettingDto model)
        {
            try
            {
                var DbModel = _mapper.Map<UserDb>(model);

                _gameContext.Users.Add(DbModel);
                _gameContext.SaveChanges();

                model.Id = DbModel.Id;
                return Result.Success(model);
            }
            catch (DbUpdateException ex)
            {
                return Result.Failure<UserSettingDto>(ex.Message);
            }
        }
        public async Task<Result<UserSettingDto>> AddAsync(UserSettingDto model)
        {
            try
            {
                var DbModel = _mapper.Map<UserDb>(model);

                _gameContext.Users.Add(DbModel);
                await _gameContext.SaveChangesAsync().ConfigureAwait(false);

                model.Id = DbModel.Id;
                return Result.Success(model);
            }
            catch (DbUpdateException ex)
            {
                return Result.Failure<UserSettingDto>(ex.Message);
            }
        }

        /// <summary>
        /// Implementation of Update()
        /// </summary>
        /// <param name="model"></param>
        public Result Update(UserDto model)
        {

            try
            {
                var dbModel = _mapper.Map<UserDb>(model);

                _gameContext.Users.Attach(dbModel);

                var entry = _gameContext.Entry(dbModel);
                entry.Property(x => x.NickName).IsModified = true;
                entry.Property(x => x.Password).IsModified = true;
                _gameContext.SaveChanges();

                return Result.Success();
            }
            catch (DbUpdateException ex)
            {
                return Result.Failure(ex.Message);
            }
        }
        public async Task<Result> UpdateAsync(UserDto model)
        {

            try
            {
                var dbModel = _mapper.Map<UserDb>(model);

                _gameContext.Users.Attach(dbModel);

                var entry = _gameContext.Entry(dbModel);
                entry.Property(x => x.NickName).IsModified = true;
                entry.Property(x => x.Password).IsModified = true;
                await _gameContext.SaveChangesAsync().ConfigureAwait(false);

                return Result.Success();
            }
            catch (DbUpdateException ex)
            {
                return Result.Failure(ex.Message);
            }
        }

        /// <summary>
        /// Implementation of Delete()
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>true or false</returns>
        public Result Delete(int id)
        {
            try
            {
                var dbModel = _gameContext.Users.Find(id);

                if (dbModel == null)
                    return Result.Failure($"No user with id {id} exist");

                _gameContext.Users.Remove(dbModel);
                _gameContext.SaveChanges();
                return Result.Success();
            }
            catch (DbUpdateException ex)
            {
                return Result.Failure(ex.Message);
            }
        }
        public async Task<Result> DeleteAsync(int id)
        {
            try
            {
                var dbModel = await _gameContext.Users.FindAsync(id).ConfigureAwait(false);

                if (dbModel == null)
                    return Result.Failure($"No user with id {id} exist");

                _gameContext.Users.Remove(dbModel);
                await _gameContext.SaveChangesAsync().ConfigureAwait(false);
                return Result.Success();
            }
            catch (DbUpdateException ex)
            {
                return Result.Failure(ex.Message);
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

     

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }
                _gameContext.Dispose();
                GC.SuppressFinalize(this);
                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~UserService()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        #endregion
    }
}
