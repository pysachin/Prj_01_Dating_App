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
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using API.Extensions;

namespace API.Controllers
{

    [Authorize]
    public class UsersController : BaseAPIController
    {
        private readonly IUserRepository UserRepo;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;
        public UsersController(
            IUserRepository UserRepo,
            IMapper mapper,
            IPhotoService photoService
            )

        {
            this.UserRepo = UserRepo;
            _mapper = mapper;
            _photoService = photoService;
        }

        //api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDTO>>> GetUsers()
        {
            return Ok(await UserRepo.GetMembersAsync());
        }

        //api/users/4
        [HttpGet("{username}", Name = "GetUser")]
        public async Task<ActionResult<MemberDTO>> GetUser(string username)
        {
            return await UserRepo.GetMemberByUserNameAsync(username);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDTO memberInfo)
        {
            // var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await UserRepo.GetUserByUserNameAsync(User.GetUserName());
            _mapper.Map(memberInfo, user);
            UserRepo.UpdateUser(user);
            if (await UserRepo.SaveAllAsync()) return NoContent();
            return BadRequest("Failed To Update User");
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDTO>> AddPhoto(IFormFile file)
        {
            var user = await UserRepo.GetUserByUserNameAsync(User.GetUserName());

            var result = await _photoService.AddPhotoAsync(file);

            if (result.Error != null) return BadRequest(result.Error.Message);

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            if (user.Photos.Count == 0)
            {
                photo.IsMain = true;
            }

            user.Photos.Add(photo);

            if (await UserRepo.SaveAllAsync())
            {
                //return _mapper.Map<PhotoDTO>(photo); 200 response but we need 201 created
                return CreatedAtRoute("GetUser", new { username = user.UserName },
                _mapper.Map<PhotoDTO>(photo));
            }

            return BadRequest("Failed To Add Photo ");

        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var user = await UserRepo.GetUserByUserNameAsync(User.GetUserName());
            var photo = user.Photos.FirstOrDefault(p => p.Id == photoId);

            if (photo.IsMain) return BadRequest("This is already your main photo");

            var currentMain = user.Photos.First(x => x.IsMain);

            if (currentMain != null) currentMain.IsMain = false;

            photo.IsMain = true;

            if (await UserRepo.SaveAllAsync()) return NoContent();

            return BadRequest("failed to set main photo");
        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var user = await UserRepo.GetUserByUserNameAsync(User.GetUserName());
            var photo = user.Photos.FirstOrDefault(p => p.Id == photoId);

            if (photo == null) return NotFound();

            if (photo.IsMain) return BadRequest("You cannot delete your main photo");

            if (photo.PublicId != null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if (result.Error != null) return BadRequest(result.Error.Message);
            }

            user.Photos.Remove(photo);

            if (await UserRepo.SaveAllAsync()) return Ok();

            return BadRequest("Failed to delete the photo");

        }

    }
}
