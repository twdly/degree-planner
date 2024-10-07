namespace DegreePlanner.Data
{
    public class User
    {
        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? Password { get; set; }
        public UserRole Role { get; set; }
        public Degree? Degree { get; set; }
        public Major? Major { get; set; }
        public List<Subject>? Subjects { get; set; }
    }
}
