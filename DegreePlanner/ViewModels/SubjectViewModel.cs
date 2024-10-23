using DegreePlanner.Data;

namespace DegreePlanner.ViewModels
{
	public class SubjectViewModel
	{
		public int SubjectId { get; set; }
		public string Name { get; set; }
		public bool Selected = false;
		public DegreeSubjectType Type;
		public UserSubjectState State;
		public List<int> PrerequisiteIds = [];
		public int Mark = 0;

		public SubjectViewModel(Subject subject)
		{
			SubjectId = subject.SubjectCode;
			Name = subject.Name!;
		}

		public SubjectViewModel(UserSubject userSubject, string name)
		{
			SubjectId = userSubject.SubjectId;
			Name = name;
			Mark = userSubject.Mark;
			State = userSubject.State;
		}

		public SubjectViewModel(Subject subject, bool selected, DegreeSubjectType type, List<int> prerequisites)
		{
			SubjectId = subject.SubjectId;
			Name = subject.Name!;
			Selected = selected;
			Type = type;
			PrerequisiteIds = prerequisites;
		}

		public SubjectViewModel(DegreeSubject degreeSubject, string name, bool selected, bool hasPassed)
		{
			SubjectId = degreeSubject.SubjectId;
			Name = name;
			Selected = selected;
			Type = degreeSubject.Type;
			State = hasPassed ? UserSubjectState.Passed : UserSubjectState.Failed;
		}

		public SubjectViewModel(MajorSubject majorSubject, string name, bool selected, bool hasPassed)
		{
			SubjectId = majorSubject.SubjectId;
			Name = name;
			Selected = selected;
			Type = DegreeSubjectType.Major;
			State = hasPassed ? UserSubjectState.Passed : UserSubjectState.Failed;
		}
	}
}
