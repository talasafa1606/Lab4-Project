using AutoMapper;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Student, StudentViewModel>()
                  .ForMember(dest => dest.FullName,
                      opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                  .ForMember(dest => dest.Age,
                      opt => opt.MapFrom(src => CalculateAge(src.DateOfBirth)));
        CreateMap<CreateStudentDto, Student>();


        CreateMap<Teacher, TeacherViewModel>()
            .ForMember(dest => dest.FullName,
                opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));
        CreateMap<CreateTeacherDto, Teacher>();

        CreateMap<Course, CourseViewModel>()
            .ForMember(dest => dest.TeacherName,
                opt => opt.MapFrom(src => $"{src.Teacher.FirstName} {src.Teacher.LastName}"));
        CreateMap<CreateCourseDto, Course>();

        CreateMap<Class, ClassViewModel>()
            .ForMember(dest => dest.CourseCode,
                opt => opt.MapFrom(src => src.Course.Code))
            .ForMember(dest => dest.CourseName,
                opt => opt.MapFrom(src => src.Course.Name))
            .ForMember(dest => dest.TeacherName,
                opt => opt.MapFrom(src => $"{src.Course.Teacher.FirstName} {src.Course.Teacher.LastName}"))
            .ForMember(dest => dest.CurrentEnrollment,
                opt => opt.MapFrom(src => src.Enrollments.Count));

        CreateMap<CreateClassDto, Class>();
    }
    private int CalculateAge(DateTime dateOfBirth)
    {
        int age = DateTime.Now.Year - dateOfBirth.Year;
        if (DateTime.Now.DayOfYear < dateOfBirth.DayOfYear)
            age--;
        return age;
    }
}
