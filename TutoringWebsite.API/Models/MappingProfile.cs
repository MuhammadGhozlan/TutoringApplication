using AutoMapper;
using TutoringWebsite.API.DTOs;

namespace TutoringWebsite.API.Models
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Student,StudentResponseDto>();
            CreateMap<Instructor,InstructorResponseDto>();
        }
    }
}
