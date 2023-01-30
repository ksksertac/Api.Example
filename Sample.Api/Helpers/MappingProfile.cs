using AutoMapper;
using Sample.Api.Models.Dto;
using Sample.Api.Models.Entities;

namespace Sample.Api.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
              CreateMap<AuthorDto, Author>();
              CreateMap<Author, AuthorDto>();

              CreateMap<BookDto, Book>();
              CreateMap<Book, BookDto>();
        }
          
    }
}