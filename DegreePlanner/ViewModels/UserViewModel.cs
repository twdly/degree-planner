using DegreePlanner.Data;

namespace DegreePlanner.ViewModels
{
	public class UserViewModel(User user)
	{
		public int Id { get; set; } = user.UserId;
		public string Name { get; set; } = user.Name;
		public int Mark { get; set; }
	}
}
