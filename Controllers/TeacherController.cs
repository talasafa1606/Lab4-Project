using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class TeachersController : ControllerBase
{
    private readonly MyUniversityDBContext _context;
    private readonly IMapper _mapper;

    public TeachersController(MyUniversityDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TeacherViewModel>>> GetTeachers()
    {
        var teachers = await _context.Teachers
            .Include(t => t.Courses)
            .ToListAsync();
        return Ok(_mapper.Map<IEnumerable<TeacherViewModel>>(teachers));
    }

    [HttpPost]
    public async Task<ActionResult<TeacherViewModel>> CreateTeacher(CreateTeacherDto teacherDto)
    {
        var teacher = _mapper.Map<Teacher>(teacherDto);
        _context.Teachers.Add(teacher);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTeachers), new { id = teacher.Id },
            _mapper.Map<TeacherViewModel>(teacher));
    }

}
