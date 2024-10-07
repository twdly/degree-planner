namespace DegreePlanner.Data
{
    public class Major
    {
        public int MajorId { get; set; }
        public string? Name { get; set; }
        public int Credits { get; set; }
        public List<Subject>? Subjects { get; set; }
        public List<User>? Students { get; set; }
    }
}
