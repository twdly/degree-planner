using DegreePlanner.Data;

namespace DegreePlanner.ViewModels;

public class TutorViewModel(User user, bool isTutor)
{
	public int Id { get; set; } = user.UserId;
	public string Name { get; set; } = user.Name;
	public bool IsTutor { get; set; } = isTutor; // This attribute represents whether a tutor is assigned to a certain subject
}