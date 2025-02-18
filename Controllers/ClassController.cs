using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class ClassesController : ControllerBase
{
    private readonly MyUniversityDBContext _context;
    private readonly IMapper _mapper;

    public ClassesController(MyUniversityDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClassViewModel>>> GetClasses()
    {
        var classes = await _context.Classes
            .Include(c => c.Course)
                .ThenInclude(c => c.Teacher)
            .Include(c => c.Enrollments)
            .ToListAsync();

        return Ok(_mapper.Map<IEnumerable<ClassViewModel>>(classes));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ClassViewModel>> GetClass(int id)
    {
        var class_ = await _context.Classes
            .Include(c => c.Course)
                .ThenInclude(c => c.Teacher)
            .Include(c => c.Enrollments)
                .ThenInclude(e => e.Student)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (class_ == null)
            return NotFound();

        return Ok(_mapper.Map<ClassViewModel>(class_));
    }

    [HttpGet("course/{courseId}")]
    public async Task<ActionResult<IEnumerable<ClassViewModel>>> GetClassesByCourse(int courseId)
    {
        var classes = await _context.Classes
            .Include(c => c.Course)
                .ThenInclude(c => c.Teacher)
            .Include(c => c.Enrollments)
            .Where(c => c.CourseId == courseId)
            .ToListAsync();

        return Ok(_mapper.Map<IEnumerable<ClassViewModel>>(classes));
    }

    [HttpPost]
    public async Task<ActionResult<ClassViewModel>> CreateClass(CreateClassDto classDto)
    {
        var course = await _context.Courses
            .Include(c => c.Teacher)
            .FirstOrDefaultAsync(c => c.Id == classDto.CourseId);

        if (course == null)
            return BadRequest("Course not found");

        var existingClass = await _context.Classes
            .FirstOrDefaultAsync(c =>
                c.CourseId == classDto.CourseId &&
                c.Semester == classDto.Semester &&
                c.Year == classDto.Year);

        if (existingClass != null)
            return BadRequest("A class for this course already exists in the specified semester/year");

        var class_ = _mapper.Map<Class>(classDto);
        _context.Classes.Add(class_);
        await _context.SaveChangesAsync();

        class_ = await _context.Classes
            .Include(c => c.Course)
                .ThenInclude(c => c.Teacher)
            .Include(c => c.Enrollments)
            .FirstAsync(c => c.Id == class_.Id);

        return CreatedAtAction(nameof(GetClass),
            new { id = class_.Id },
            _mapper.Map<ClassViewModel>(class_));
    }


    [HttpPatch("{id}/capacity")]
    public async Task<IActionResult> UpdateClassCapacity(int id, [FromBody] int newCapacity)
    {
        var class_ = await _context.Classes
            .Include(c => c.Enrollments)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (class_ == null)
            return NotFound();

        if (newCapacity < class_.Enrollments.Count)
            return BadRequest("New capacity cannot be less than current enrollment");

        class_.MaxCapacity = newCapacity;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpGet("semester")]
    public async Task<ActionResult<IEnumerable<ClassViewModel>>> GetClassesBySemester(
        [FromQuery] string semester, [FromQuery] int year)
    {
        var classes = await _context.Classes
            .Include(c => c.Course)
                .ThenInclude(c => c.Teacher)
            .Include(c => c.Enrollments)
            .Where(c => c.Semester == semester && c.Year == year)
            .ToListAsync();

        return Ok(_mapper.Map<IEnumerable<ClassViewModel>>(classes));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteClass(int id)
    {
        var class_ = await _context.Classes
            .Include(c => c.Enrollments)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (class_ == null)
            return NotFound();

        if (class_.Enrollments.Any())
            return BadRequest("Cannot delete a class with enrolled students");

        _context.Classes.Remove(class_);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
