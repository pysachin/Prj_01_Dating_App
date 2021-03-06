using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseAPIController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        public AccountController(DataContext context,
        ITokenService tokenService,
        IMapper mapper
        )
        {
            _context = context;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO regDTO)
        {

            if (await IsUserExists(regDTO.Username)) return BadRequest("User Name Already Exists");

            var user = _mapper.Map<AppUser>(regDTO);

            using var hmac = new HMACSHA512();

            user.UserName = regDTO.Username;
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(regDTO.Password));
            user.PasswordSalt = hmac.Key;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return new UserDTO()
            {
                UserName = user.UserName,
                Token = _tokenService.CreateToken(user),
                KnownAs = user.KnownAs,
                Gender = user.Gender
            };
        }

        private async Task<bool> IsUserExists(string UserName)
        {
            return await _context.Users.AnyAsync(u => u.UserName.ToLower() == UserName.ToLower());
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO logDTO)
        {
            var user = await _context.Users
                        .Include(p => p.Photos)
                        .SingleOrDefaultAsync(u => u.UserName.ToLower() == logDTO.Username.ToLower());

            if (user == null) return Unauthorized("Invalid User Name");

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedhash = hmac.ComputeHash(Encoding.UTF8.GetBytes(logDTO.Password));

            for (int i = 0; i < computedhash.Length; i++)
            {
                if (computedhash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
            }

            return new UserDTO()
            {
                UserName = user.UserName,
                Token = _tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain == true)?.Url,
                KnownAs = user.KnownAs,
                Gender = user.Gender
            };
        }

    }
}