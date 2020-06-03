﻿using System.Net;
using System.Web.Http;
using AliaksNad.Battleship.Logic.Models;
using AliaksNad.Battleship.Logic.Services;
using FluentValidation;
using FluentValidation.WebApi;

namespace GamePortal.Web.Api.Controllers.Battleship
{
    [RoutePrefix("api/battleship/users")]
    public class UserController : ApiController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            this._userService = userService;
        }

        /// <summary>
        /// Get all users from logic layer.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAll()
        {
            return Ok(_userService.GetAll());
        }

        /// <summary>
        /// Get user from logic layer by id.
        /// </summary>
        /// <param name="id">user id.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult GetById(int id)
        {
            var user = _userService.GetById(id);
            return user == null ? (IHttpActionResult)NotFound() : Ok(user);
        }

        /// <summary>
        /// Add user to data via logic layer.
        /// </summary>
        /// <param name="model">User model.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        public IHttpActionResult Add([CustomizeValidator(RuleSet = "PreValidation")][FromBody]UserDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _userService.Add(model);
            return result.IsSuccess ? Created($"/api/battleship/users/{result.Value.Id}", result.Value) : (IHttpActionResult)BadRequest(result.Error) ;
            
        }

        /// <summary>
        /// Update user model in data via logic layer.
        /// </summary>
        /// <param name="model">User model.</param>
        [HttpPut]
        [Route("")]
        public IHttpActionResult Update([FromBody]UserDto model)
        {
            _userService.Update(model);
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Delete user in data via logic layer by id.
        /// </summary>
        /// <param name="id">User id.</param>
        [HttpDelete]
        [Route("{id}:int:min(1)")]      //check risone 
        public IHttpActionResult Delete(int id)
        {
            _userService.Delete(id);
            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
