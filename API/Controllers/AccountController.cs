using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseAPIController
    {

        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        public AccountController(UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        ITokenService tokenService,
        IMapper mapper
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO regDTO)
        {

            if (await IsUserExists(regDTO.Username.ToLower())) return BadRequest("User Name Already Exists");

            var user = _mapper.Map<AppUser>(regDTO);

            user.UserName = regDTO.Username.ToLower();

            var result = await _userManager.CreateAsync(user, regDTO.Password);

            if (!result.Succeeded) return BadRequest(result.Errors);

            var roleResult = await _userManager.AddToRoleAsync(user, "Member");

            if (!roleResult.Succeeded) return BadRequest(roleResult.Errors);

            return new UserDTO()
            {
                UserName = user.UserName,
                Token = await _tokenService.CreateToken(user),
                KnownAs = user.KnownAs,
                Gender = user.Gender
            };
        }

        private async Task<bool> IsUserExists(string UserName)
        {
            return await _userManager.Users.AnyAsync(u => u.UserName.ToLower() == UserName.ToLower());
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO logDTO)
        {
            var user = await _userManager.Users
                        .Include(p => p.Photos)
                        .SingleOrDefaultAsync(u => u.UserName.ToLower() == logDTO.Username.ToLower());

            if (user == null) return Unauthorized("Invalid User Name");

            var result = await _signInManager
                        .CheckPasswordSignInAsync(user, logDTO.Password, false);

            if (!result.Succeeded) return Unauthorized();

            return new UserDTO()
            {
                UserName = user.UserName,
                Token = await _tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain == true)?.Url,
                KnownAs = user.KnownAs,
                Gender = user.Gender
            };
        }

    }
}