namespace DegreePlanner.ViewModels
{
	public class SubjectEnrolmentViewModel(List<UserViewModel> students, int plannedCount, string name)
	{
		public List<UserViewModel> Students { get; set; } = students;
		public int PlannedCount { get; set; } = plannedCount;
		public string Name { get; set; } = name;
	}
}
