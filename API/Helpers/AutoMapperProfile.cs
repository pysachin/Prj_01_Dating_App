using System.Linq;
using API.DTOs;
using API.Entities;
using API.Extensions;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AppUser, MemberDTO>()
            .ForMember(d => d.PhotoUrl,
            o => o.MapFrom(
                s => s.Photos.FirstOrDefault(p => p.IsMain).Url))
            .ForMember(d => d.Age,
                o => o.MapFrom(
                s => s.DateOfBirth.CalculateAge()));

            CreateMap<Photo, PhotoDTO>();
            CreateMap<MemberUpdateDTO, AppUser>();
        }
    }
}