using DegreePlanner.Data;

namespace DegreePlanner.ViewModels
{
	public class SubjectViewModel
	{
		public int SubjectCode { get; set; }
		public string Name { get; set; }
		public bool Selected = false;

		public SubjectViewModel(Subject subject)
		{
			SubjectCode = subject.SubjectCode;
			Name = subject.Name!;
		}

		public SubjectViewModel(DegreeSubject degreeSubject, string name, bool selected)
		{
			SubjectCode = degreeSubject.SubjectId;
			Name = name;
			Selected = selected;
		}
	}
}
