using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly MyUniversityDBContext _context;
    private readonly IMapper _mapper;

    public StudentsController(MyUniversityDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<StudentViewModel>>> GetStudents()
    {
        var students = await _context.Students
            .Include(s => s.Enrollments)
            .ThenInclude(e => e.Class)
            .ThenInclude(c => c.Course)
            .ToListAsync();
        return Ok(_mapper.Map<IEnumerable<StudentViewModel>>(students));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<StudentViewModel>> GetStudent(int id)
    {
        var student = await _context.Students
            .Include(s => s.Enrollments)
            .ThenInclude(e => e.Class)
            .ThenInclude(c => c.Course)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (student == null)
            return NotFound();

        return Ok(_mapper.Map<StudentViewModel>(student));
    }

    [HttpPost]
    public async Task<ActionResult<StudentViewModel>> CreateStudent(CreateStudentDto studentDto)
    {
        var student = _mapper.Map<Student>(studentDto);
        _context.Students.Add(student);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetStudent), new { id = student.Id },
            _mapper.Map<StudentViewModel>(student));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStudent(int id, CreateStudentDto studentDto)
    {
        var student = await _context.Students.FindAsync(id);
        if (student == null)
            return NotFound();

        _mapper.Map(studentDto, student);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStudent(int id)
    {
        var student = await _context.Students.FindAsync(id);
        if (student == null)
            return NotFound();

        _context.Students.Remove(student);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPost("{studentId}/enroll/{classId}")]
    public async Task<ActionResult> EnrollInClass(int studentId, int classId)
    {
        var student = await _context.Students.FindAsync(studentId);
        var class_ = await _context.Classes
            .Include(c => c.Enrollments)
            .FirstOrDefaultAsync(c => c.Id == classId);

        if (student == null || class_ == null)
            return NotFound();

        if (class_.Enrollments.Count >= class_.MaxCapacity)
            return BadRequest("Class is at maximum capacity");

        if (class_.Enrollments.Any(e => e.StudentId == studentId))
            return BadRequest("Student is already enrolled in this class");



        var enrollment = new Enrollment
        {
            StudentId = studentId,
            ClassId = classId,
            EnrollmentDate = DateTime.Now.ToUniversalTime()
        };

        _context.Enrollments.Add(enrollment);
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpDelete("{studentId}/drop/{classId}")]
    public async Task<ActionResult> DropClass(int studentId, int classId)
    {
        var enrollment = await _context.Enrollments
            .FirstOrDefaultAsync(e => e.StudentId == studentId && e.ClassId == classId);

        if (enrollment == null)
            return NotFound();

        _context.Enrollments.Remove(enrollment);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
