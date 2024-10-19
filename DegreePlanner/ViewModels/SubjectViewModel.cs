using DegreePlanner.Data;

namespace DegreePlanner.ViewModels
{
	public class SubjectViewModel(Subject subject)
	{
		public int SubjectCode { get; set; } = subject.SubjectCode;
		public string Name { get; set; } = subject.Name;
	}
}
