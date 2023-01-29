using AutoMapper;
using Sample.Models.Dto;
using Sample.Models.Entities;

namespace Sample.Profiles
{
    public class AuthorProfile : Profile
    {
        public AuthorProfile()
        {
            CreateMap<AuthorDto, Author>();
            CreateMap<Author, AuthorDto>();
        }
    }
}