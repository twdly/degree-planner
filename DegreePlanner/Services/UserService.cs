using DegreePlanner.Data;
using DegreePlanner.Services.Interfaces;
using DegreePlanner.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DegreePlanner.Services
{
	public class UserService(DatabaseContext databaseContext) : IUserService
	{
		public UserViewModel GetUserFromId(int userId)
		{
			var user = databaseContext.Users.FirstOrDefault(x => x.UserId == userId);
			return new UserViewModel(user);
		}

		public MajorViewModel? GetUserMajor(int userId)
		{
			var user = databaseContext.Users.Include(x => x.Major).ThenInclude(x => x.Subjects).FirstOrDefault(x => x.UserId == userId);
			return user.Major != null ? new(user.Major) : null;
		}

		public DegreeViewModel? GetDegreeForUser(int id)
		{
			var user = databaseContext.Users.Include(x => x.Major).Include(x => x.Degree).ThenInclude(x => x.Subjects).FirstOrDefault(x => x.UserId == id);
			return user.Degree != null ? new(user.Degree) : null;
		}

		public SubjectEnrolmentViewModel GetSubjectEnrolment(int subjectId)
		{
			var enrolledStudentIds = databaseContext.UserSubjects
				.Where(x => x.State == UserSubjectState.Enrolled && x.SubjectId == subjectId)
				.Select(x => x.UserId)
				.ToList();

			var subjectName = databaseContext.Subjects.Include(x => x.Users).FirstOrDefault(x => x.SubjectId == subjectId)?.Name;

			var enrolledStudents = databaseContext.Users.Where(x => enrolledStudentIds.Contains(x.UserId));
			List<UserViewModel> enrolledStudentsViewModel = [];
            foreach (var student in enrolledStudents)
            {
				enrolledStudentsViewModel.Add(new(student));
            }

			int plannedCount = databaseContext.UserSubjects.Count(x => x.SubjectId == subjectId && x.State == UserSubjectState.Planned);

			return new(enrolledStudentsViewModel, plannedCount, subjectName!);
		}
	}
}
