﻿using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using AliaksNad.Battleship.Logic.Models.User;
using AliaksNad.Battleship.Logic.Services.Contracts;
using FluentValidation.WebApi;
using GamePortal.Web.Api.Filters.Battleship;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Swashbuckle.Swagger.Annotations;

namespace GamePortal.Web.Api.Controllers.Battleship
{
    [RoutePrefix("api/battleship/Users"), ModelStateValidation]
    public class UserController : ApiController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            this._userService = userService;
        }

        /// <summary>
        /// Create and register new user in app
        /// </summary>
        /// <param name="model">New user model</param>
        /// <returns></returns>
        [HttpPost, Route("register")]
        [SwaggerResponse(HttpStatusCode.NoContent)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> Register([CustomizeValidator(RuleSet = "PreValidation"), FromBody]NewUserDto model)
        {
            var result = await _userService.RegisterAsync(model);
            return result.IsSuccess ? StatusCode(HttpStatusCode.NoContent) : StatusCode(HttpStatusCode.InternalServerError);
        }

        /// <summary>
        /// Login and authentication user
        /// </summary>
        /// <param name="model">User name and password</param>
        /// <returns></returns>
        [HttpPost, Route("login")]
        [SwaggerResponse(HttpStatusCode.Unauthorized)]
        [SwaggerResponse(HttpStatusCode.OK)]
        public async Task<IHttpActionResult> Login([CustomizeValidator(RuleSet = "PreValidation"), FromBody]LoginDto model)
        {
            var result = await _userService.GetUserAsync(model.UserName, model.Password);
            if (result.HasNoValue) return Unauthorized();

            var identity = new ClaimsIdentity(DefaultAuthenticationTypes.ApplicationCookie);
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, result.Value.Id));
            identity.AddClaim(new Claim(ClaimTypes.Name, result.Value.UserName));

            var provider = Request.GetOwinContext().Authentication;

            provider.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            provider.SignIn(new AuthenticationProperties { IsPersistent = true }, identity);

            return Ok();
        }

        /// <summary>
        /// Reset user password in app
        /// </summary>
        /// <param name="email">User email</param>
        /// <returns></returns>
        [HttpPut, Route("resetpass")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> ResetPassword([FromBody]string email)
        {
            var result = await _userService.ResetPasswordAsync(email);
            return result.IsSuccess ? Ok() : (IHttpActionResult)StatusCode(HttpStatusCode.InternalServerError);
        }

        /// <summary>
        /// Change user password in app
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="token">Validation token</param>
        /// <param name="newPassword">New password</param>
        /// <returns></returns>
        [HttpPut, Route("changepass")]
        [SwaggerResponse(HttpStatusCode.NoContent)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> ChangePassword(string userId, string token, string newPassword)
        {
            var result = await _userService.ChangePasswordAsync(userId, token, newPassword);
            return result.IsSuccess ? StatusCode(HttpStatusCode.NoContent) : StatusCode(HttpStatusCode.InternalServerError);
        }

        /// <summary>
        /// Confirm user email in app
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="token">Validation token</param>
        /// <returns></returns>
        [HttpPut, Route("confirmemail")]
        [SwaggerResponse(HttpStatusCode.NoContent)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> ConfirmEmail(string userId, string token)
        {
            var result = await _userService.ConfirmEmailAsync(userId, token);
            return result.IsSuccess ? StatusCode(HttpStatusCode.NoContent) : StatusCode(HttpStatusCode.InternalServerError);
        }

        /// <summary>
        /// Delete user in app
        /// </summary>
        /// <param name="userId">User ID</param>
        [HttpDelete, Route(""), Authorize]
        [SwaggerResponse(HttpStatusCode.NoContent)]
        [SwaggerResponse(HttpStatusCode.InternalServerError)]
        public async Task<IHttpActionResult> Delete()
        {
            var userId = User.Identity.GetUserId();

            var result = await _userService.DeleteAsync(userId);
            return result.IsSuccess ? StatusCode(HttpStatusCode.NoContent) : StatusCode(HttpStatusCode.InternalServerError);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
