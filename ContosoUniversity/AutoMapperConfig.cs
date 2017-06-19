using AutoMapper;
using ContosoUniversity.Models;
using ContosoUniversity.ViewModels;

namespace ContosoUniversity
{
    public class AutoMapperConfig
    {
        public static void Initialize()
        {
            Mapper.Initialize((config) =>
            {
                config.CreateMap<Course, CourseViewModel>().ReverseMap();
            });
        }
    }
}