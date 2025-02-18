public class Class
{
     public int Id { get; set; }
    public int CourseId { get; set; }
    public string Semester { get; set; }
    public int Year { get; set; }
    public int MaxCapacity { get; set; }
    public virtual Course Course { get; set; }
    public virtual ICollection<Enrollment> Enrollments { get; set; }
}
