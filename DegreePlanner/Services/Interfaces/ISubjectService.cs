using DegreePlanner.Data;
using DegreePlanner.ViewModels;

namespace DegreePlanner.Services.Interfaces;

public interface ISubjectService
{       
	/**
	 * Creates a new subject according to the given subject details
	 */
	public void AddSubject(Subject subject);

	public bool CheckAllSubjectCodes(int subjectCode);

	/**
	 * Gets every subject in the database
	 */
	public List<SubjectViewModel> GetAllSubjects();

	/**
	 * Updates subject details according to incoming details
	 */
	public void UpdateSubject(SubjectViewModel subject);

	/**
	 * Gets a list of subjects and marks from subjects with the states passed or failed
	 */
	List<SubjectViewModel> GetCompletedSubjects(int userId);

	/**
	 * Gets a list of subjects that the given user coordinates
	 */
	List<SubjectViewModel> GetCoordinatedSubjects(int userId);

	/**
	* Returns a list of all available subjects the user is yet to enrol in with the "planned" bool set accordingly
	*/
	List<SubjectViewModel> GetDegreeSubjectsToPlan(int userId);

	/**
	 * Gets a list of subjects and marks from subjects with the states planned or enrolled
	 */
	List<SubjectViewModel> GetPredictionSubjects(int userId);

	/**
	 * Returns the name of the subject with the given ID
	 */
	string GetSubjectNameFromId(int subjectId);

	/**
	 * Get the list of subjects that the user has planned or enrolled in
	 */
	List<SubjectViewModel> GetSubjectsToEnrol(int userId);

	/**
	 * Returns a list of subjects that the given user tutors or coordinates
	 */
	List<SubjectViewModel> GetTeacherSubjects(int userId);

	/**
	 * Update the user state for each of the provided subjects
	 */
	void UpdateSubjects(List<SubjectViewModel> subjectViewModels, UserSubjectState state, int userId);

	/**
	 * Update the results of the provided students in the given subject
	 */
	void UpdateUserResultsForSubject(List<UserViewModel> updatedStudents, int subjectId);
}