using DegreePlanner.Data;
using DegreePlanner.Services.Interfaces;
using DegreePlanner.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace DegreePlanner.Components.Pages.Student
{
    public partial class Subjects
    {
        [CascadingParameter]
        private Task<AuthenticationState> AuthenticationState { get; set; }

        [Inject]
        public IUserService UserService { get; set; }

        [Inject]
        public ISubjectService SubjectService { get; set; }

        public List<SubjectViewModel> subjects = [];
        public bool typeSelected = false;
        public UserSubjectState enrolType;
        public DegreeViewModel? degree;
        public MajorViewModel? major;
        public int userId;
        public string message = "";

        public void SetEnrolType(UserSubjectState type)
        {
            enrolType = type;
        }

        public void LoadSubjects()
        {
            typeSelected = true;
			if (enrolType == UserSubjectState.Planned)
            {
                subjects = SubjectService.GetDegreeSubjectsToPlan(userId);
            }
            else
            {
                subjects = SubjectService.GetSubjectsToEnrol(userId);
            }
        }

        public void Back()
        {
            typeSelected = false;
        }

        public void UpdateSubjects()
        {
            SubjectService.UpdateSubjects(subjects, enrolType, userId);
            message = "Subjects have been updated!";
            Back();
        }

		protected override async Task OnInitializedAsync()
		{
            var authState = await AuthenticationState;
            userId = int.Parse(authState.User.Identity.Name);
            degree = UserService.GetDegreeForUser(userId);
            major = UserService.GetUserMajor(userId);
		}

        public string GetSubjectTypeTitle()
        {
            return enrolType == UserSubjectState.Planned ? "Planning Subjects" : "Enrolling in Subjects";
        }

        public bool CanEnrol()
        {
            if (enrolType == UserSubjectState.Planned) return true;

            return subjects.Where(x => x.Selected).Count() <= 4;
        }
	}
}
