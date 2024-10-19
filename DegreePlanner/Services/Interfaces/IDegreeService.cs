using DegreePlanner.ViewModels;

namespace DegreePlanner.Services.Interfaces
{
	public interface IDegreeService
	{
		/**
		 * Returns the degree of the given user or null if they have not yet enrolled in a degree
		 **/
		public DegreeViewModel? GetDegreeForUser(int id);
	}
}
