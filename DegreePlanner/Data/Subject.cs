using System.Net;

namespace DegreePlanner.Data
{
    public class Subject
    {
        public int SubjectId { get; set; }
        public string? Name { get; set; }
        public int Credits { get; set; }
        public List<User>? Users { get; set; }
        public List<Degree>? Degrees { get; set; }
        public List<Major>? Majors { get; set; }
        public List<Prerequisite>? Prerequisites { get; set; }
    }
}
