using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext context;
        private readonly IMapper mapper;
        public UserRepository(DataContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }


        public async Task<IEnumerable<AppUser>> GetAllUsersAsync()
        {
            return await context.Users.Include(p => p.Photos).ToListAsync();
        }

        public async Task<MemberDTO> GetMemberByUserNameAsync(string userName)
        {
            return await context.Users
                         .Where(u => u.UserName.ToLower() == userName.ToLower())
                         .ProjectTo<MemberDTO>(mapper.ConfigurationProvider)
                         .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<MemberDTO>> GetMembersAsync()
        {
            return await context.Users
                        .ProjectTo<MemberDTO>(mapper.ConfigurationProvider)
                        .ToListAsync();
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await context.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUserNameAsync(string userName)
        {
            return await context.Users
            .Include(p => p.Photos)
            .SingleOrDefaultAsync(u => u.UserName.ToLower() == userName.ToLower());
        }

        public async Task<bool> SaveAllAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }
        public void UpdateUser(AppUser appUser)
        {
            context.Entry(appUser).State = EntityState.Modified;
        }
    }
}