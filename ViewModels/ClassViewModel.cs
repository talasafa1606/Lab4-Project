public class ClassViewModel
{
    public int Id { get; set; }
    public string CourseCode { get; set; }
    public string CourseName { get; set; }
    public string TeacherName { get; set; }
    public string Semester { get; set; }
    public int Year { get; set; }
    public int CurrentEnrollment { get; set; }
    public int MaxCapacity { get; set; }
    public bool IsFullyEnrolled => CurrentEnrollment >= MaxCapacity;
}
