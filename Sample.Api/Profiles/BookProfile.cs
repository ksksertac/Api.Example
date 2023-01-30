using AutoMapper;
using Sample.Api.Models.Dto;
using Sample.Api.Models.Entities;

namespace Sample.Api.Profiles
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<BookDto, Book>();
            CreateMap<Book, BookDto>();
            
        }
    }
}