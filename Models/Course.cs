public class Course
{
     public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Credits { get; set; }
    public int TeacherId { get; set; }
    public virtual Teacher Teacher { get; set; }
    public virtual ICollection<Class> Classes { get; set; }
}
