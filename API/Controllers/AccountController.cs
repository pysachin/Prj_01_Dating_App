using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseAPIController
    {
        private readonly DataContext _context;
        public AccountController(DataContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AppUser>> Register(RegisterDTO regDTO)
        {

            if (await IsUserExists(regDTO.Username)) return BadRequest("User Name Already Exists");

            using var hmac = new HMACSHA512();

            var user = new AppUser()
            {
                UserName = regDTO.Username,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(regDTO.Password)),
                PasswordSalt = hmac.Key
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        private async Task<bool> IsUserExists(string UserName)
        {
            return await _context.Users.AnyAsync(u => u.UserName.ToLower() == UserName.ToLower());
        }

        [HttpPost("login")]
        public async Task<ActionResult<AppUser>> Login(LoginDTO logDTO)
        {
            var user = await _context.Users
                        .SingleOrDefaultAsync(u => u.UserName.ToLower() == logDTO.Username.ToLower());

            if (user == null) return Unauthorized("Invalid User Name");

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedhash = hmac.ComputeHash(Encoding.UTF8.GetBytes(logDTO.Password));

            for (int i = 0; i < computedhash.Length; i++)
            {
                if (computedhash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
            }

            return user;
        }

    }
}