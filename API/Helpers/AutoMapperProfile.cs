using System;
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
            CreateMap<RegisterDTO, AppUser>();
            CreateMap<Message, MessageDto>()
            .ForMember(dest => dest.SenderPhotoUrl,
                opt => opt.MapFrom(src => src.Sender.Photos.FirstOrDefault(x => x.IsMain).Url)
                )
            .ForMember(dest => dest.RecipientPhotoUrl,
            opt => opt.MapFrom(src => src.Recipient.Photos.FirstOrDefault(x => x.IsMain).Url)
            );

        }
    }
}