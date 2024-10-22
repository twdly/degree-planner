using DegreePlanner.Data;
using DegreePlanner.ViewModels;

namespace DegreePlanner.Services.Interfaces
{
	public interface ISubjectService
	{
		/**
		 * Gets a list of subjects and marks from subjects with the states passed or failed
		 */
		List<SubjectViewModel> GetCompletedSubjects(int userId);

		/**
		* Returns a list of all available subjects the user is yet to enrol in with the "planned" bool set accordingly
		*/
		List<SubjectViewModel> GetDegreeSubjectsToPlan(int userId);

		/**
		 * Gets a list of subjects and marks from subjects with the states planned or enrolled
		 */
		List<SubjectViewModel> GetPredictionSubjects(int userId);

		/**
		 * Get the list of subjects that the user has planned or enrolled in
		 */
		List<SubjectViewModel> GetSubjectsToEnrol(int userId);
		List<SubjectViewModel> GetTeacherSubjects(int userId);

		/**
		 * Update the user state for each of the provided subjects
		 */
		void UpdateSubjects(List<SubjectViewModel> subjectViewModels, UserSubjectState state, int userId);
	}
}
