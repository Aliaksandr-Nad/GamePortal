﻿using CSharpFunctionalExtensions;
using Kbalan.TouchType.Logic.Dto;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kbalan.TouchType.Logic.Services
{
    /// <summary>
    /// Service for User
    /// </summary>
    public interface IUserService : IDisposable
    {
        Task<Result> Register(NewUserDto model);

        Task<Result> ConfirmEmail(string userId, string token);

        Task<Result> ResetPassword(string email);

        Task<Result> ChangePassword(string userId, string token, string newPassword);

        Task<Result<IReadOnlyCollection<UserDto>>> GetAllAsync();

        Task<Maybe<UserDto>> GetUser(string username, string password);

        Task<Result> RegisterExternalUser(ExternalLoginInfo info);

        Task<Result> DeleteAsync(string username);
    }
}