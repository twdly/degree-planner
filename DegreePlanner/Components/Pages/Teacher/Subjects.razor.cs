using DegreePlanner.Services.Interfaces;
using DegreePlanner.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace DegreePlanner.Components.Pages.Teacher
{
	public partial class Subjects
	{
		[CascadingParameter]
		private Task<AuthenticationState> AuthenticationState { get; set; }

		[Inject]
		private ISubjectService SubjectService { get; set; }

		[Inject]
		private IUserService UserService { get; set; }

		private List<SubjectViewModel> teacherSubjects { get; set; }
		private SubjectEnrolmentViewModel subjectEnrolment { get; set; }
		private int userId;

		protected override async Task OnInitializedAsync()
		{
			var authState = await AuthenticationState;
			userId = int.Parse(authState.User.Identity.Name);

			teacherSubjects = SubjectService.GetTeacherSubjects(userId);
		}

		private void SelectSubject(int subjectId)
		{
			subjectEnrolment = UserService.GetSubjectEnrolment(subjectId);
		}
	}
}
