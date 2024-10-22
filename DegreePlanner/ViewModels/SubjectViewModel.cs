using DegreePlanner.Data;

namespace DegreePlanner.ViewModels
{
	public class SubjectViewModel
	{
		public int SubjectId { get; set; }
		public string Name { get; set; }
		public bool Selected = false;
		public DegreeSubjectType Type;

		public SubjectViewModel(Subject subject)
		{
			SubjectId = subject.SubjectCode;
			Name = subject.Name!;
		}

		public SubjectViewModel(Subject subject, bool selected, DegreeSubjectType type)
		{
			SubjectId = subject.SubjectId;
			Name = subject.Name!;
			Selected = selected;
			Type = type;
		}

		public SubjectViewModel(DegreeSubject degreeSubject, string name, bool selected)
		{
			SubjectId = degreeSubject.SubjectId;
			Name = name;
			Selected = selected;
			Type = degreeSubject.Type;
		}

		public SubjectViewModel(MajorSubject majorSubject, string name, bool selected)
		{
			SubjectId = majorSubject.SubjectId;
			Name = name;
			Selected = selected;
			Type = DegreeSubjectType.Major;
		}
	}
}
