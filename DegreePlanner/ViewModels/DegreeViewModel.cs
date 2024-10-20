using DegreePlanner.Data;

namespace DegreePlanner.ViewModels
{
	public class DegreeViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public List<SubjectViewModel> Subjects { get; set; }

		public DegreeViewModel(Degree degree)
		{
			Id = degree.DegreeId;
			Name = degree.Name!;
			Subjects = [];
			foreach (var subject in degree.Subjects!)
			{
				Subjects.Add(new(subject));
			}
		}
	}
}
