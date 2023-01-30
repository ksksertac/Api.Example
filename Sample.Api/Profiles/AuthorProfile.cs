using AutoMapper;
using Sample.Api.Models.Dto;
using Sample.Api.Models.Entities;

namespace Sample.Api.Profiles
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