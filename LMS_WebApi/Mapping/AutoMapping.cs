using AutoMapper;
using LMS_WebApi.DTO;
using LMS_WebApi.Models;

namespace LMS_WebApi.Mapping
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<Member, MemberResponseDto>();
            CreateMap<MemberRequestDto, Member>();
            CreateMap<MemberUpdateRequestDto, Member>();
            CreateMap<Book, BookResponseDto>();
            CreateMap<BookRequestDto, Book>();
            CreateMap<BookUpdateRequestDto, Book>();
        }
    }
}
