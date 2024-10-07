namespace DegreePlanner.Data
{
    public class UserSubject
    {
        public int UserId { get; set; }
        public int SubjectId { get; set; }
        public UserSubjectState State { get; set; }
        public int Mark { get; set; }
    }
}
