using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{

    [Authorize]
    public class UsersController : BaseAPIController
    {
        private readonly IUserRepository UserRepo;

        public UsersController(
            IUserRepository UserRepo
            )

        {
            this.UserRepo = UserRepo;
        }

        //api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDTO>>> GetUsers()
        {
            return Ok(await UserRepo.GetMembersAsync());
        }

        //api/users/4
        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDTO>> GetUser(string username)
        {
            return await UserRepo.GetMemberByUserNameAsync(username);
        }

    }
}
