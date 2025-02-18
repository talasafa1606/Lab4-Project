public class Enrollment
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public int ClassId { get; set; }
    public DateTime EnrollmentDate { get; set; }
    public Grade? Grade { get; set; }
    public virtual Student Student { get; set; }
    public virtual Class Class { get; set; }
}
