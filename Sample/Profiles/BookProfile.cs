using AutoMapper;
using Sample.Models.Dto;
using Sample.Models.Entities;

namespace Sample.Profiles
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