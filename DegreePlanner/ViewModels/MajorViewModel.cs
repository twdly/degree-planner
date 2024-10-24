using DegreePlanner.Data;

namespace DegreePlanner.ViewModels;

public class MajorViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<SubjectViewModel> subjects { get; set; }

    public MajorViewModel(Major major)
    {
        Id = major.MajorId;
        Name = major.Name;
        subjects = [];
        foreach (var subject in major.Subjects)
        {
            subjects.Add(new SubjectViewModel(subject));
        }
    }
}