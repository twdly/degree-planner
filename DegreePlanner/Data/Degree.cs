namespace DegreePlanner.Data
{
    public class Degree
    {
        public int DegreeId { get; set; }
        public string? Name { get; set; }
		public int CoreCredits { get; set; }
		public int ElectiveCredits { get; set; }
		public List<Major>? Majors { get; set; } // If this list is empty, it can be assumed that the degree doesn't require students to complete a major
		public List<User>? Students { get; set; }
        public List<Subject>? Subjects { get; set; }
    }
}
