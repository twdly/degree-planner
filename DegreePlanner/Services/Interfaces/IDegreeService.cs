using DegreePlanner.Data;
using DegreePlanner.ViewModels;

namespace DegreePlanner.Services.Interfaces;

public interface IDegreeService
{
	/**
	 * Returns a complete list of all available degrees
	 */
	List<DegreeViewModel> GetAllDegrees();

	/**
	 * Attaches the subject to the chosen degree
	 */
	public void AttachToDegree(DegreeViewModel degree, int subjectID, DegreeSubjectType type);

	/**
	 * Attaches the subject to the chosen major
	 */
	public void AttachToMajor(MajorViewModel major, int subjectID);

	/**
	 * Gets all available majors for a given degree
	 */
	List<MajorViewModel> GetMajorsForDegree(DegreeViewModel degree);

	/**
	 * Enrols the user with the provided userId in the degree with the provided degreeId
	 */
	public void EnrolInDegree(int userId, int degreeId);

	/**
	 * Enrols the user with the provided userId in the major with the provided majorId
	*/
	void EnrolInMajor(int userId, int majorId);
}