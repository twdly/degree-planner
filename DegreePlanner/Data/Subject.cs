﻿namespace DegreePlanner.Data
{
    public class Subject
    {
        public int SubjectId { get; set; }
        public int SubjectCode { get; set; } // This will match the actual subject code as opposed to ID which is generated by EF
        public string? Name { get; set; }
        public List<User>? Users { get; set; }
        public List<Degree>? Degrees { get; set; }
        public List<Major>? Majors { get; set; }
        public List<Subject>? Prerequisites { get; set; }
        public List<Subject>? PrerequisiteFor { get; set; }
    }
}
