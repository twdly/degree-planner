namespace DegreePlanner.Data
{
    public class Degree
    {
        public int DegreeId { get; set; }
        public string? Name { get; set; }
        public int Credits { get; set; }
        public List<Major>? Majors { get; set; }
        public List<User>? Students { get; set; }
        public List<Subject>? Subjects { get; set; }
    }
}
