using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly MyUniversityDBContext _context;
    private readonly IMapper _mapper;

    public CoursesController(MyUniversityDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CourseViewModel>>> GetCourses()
    {
        var courses = await _context.Courses
            .Include(c => c.Teacher)
            .Include(c => c.Classes)
            .ToListAsync();
        return Ok(_mapper.Map<IEnumerable<CourseViewModel>>(courses));
    }

    [HttpPost]
    public async Task<ActionResult<CourseViewModel>> CreateCourse(CreateCourseDto courseDto)
    {
        var teacher = await _context.Teachers.FindAsync(courseDto.TeacherId);
        if (teacher == null)
            return BadRequest("Teacher not found");

        var course = _mapper.Map<Course>(courseDto);
        _context.Courses.Add(course);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCourses), new { id = course.Id },
            _mapper.Map<CourseViewModel>(course));
    }

}
