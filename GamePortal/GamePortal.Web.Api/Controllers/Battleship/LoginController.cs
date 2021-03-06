﻿using AliaksNad.Battleship.Logic.Services.Contracts;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace GamePortal.Web.Api.Controllers.Battleship
{
    public class LoginController : ApiController
    {
        private readonly IUserService _userService;

        public LoginController(IUserService userService)
        {
            this._userService = userService;
        }

        /// <summary>
        /// Create and register new user in app by OAuth 2.0
        /// </summary>
        /// <param name="model">OAuth 2.0 New user model</param>
        /// <returns></returns>
        [HttpGet, Route("external/google")]
        public async Task<IHttpActionResult> GoogleLogin()
        {
            var provider = Request.GetOwinContext().Authentication;
            var loginInfo = await provider.GetExternalLoginInfoAsync();

            if (loginInfo == null) return BadRequest();

            await _userService.RegisterExternalUserAsync(loginInfo);
            return Ok();
        }
    }
}
