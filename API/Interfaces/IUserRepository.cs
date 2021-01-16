using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
    public interface IUserRepository
    {
        void UpdateUser(AppUser appUser);
        Task<bool> SaveAllAsync();
        Task<IEnumerable<AppUser>> GetAllUsersAsync();
        Task<AppUser> GetUserByIdAsync(int id);
        Task<AppUser> GetUserByUserNameAsync(string userName);
        Task<IEnumerable<MemberDTO>> GetMembersAsync();
        Task<MemberDTO> GetMemberByUserNameAsync(string userName);

    }
}