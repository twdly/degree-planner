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

        public List<SubjectViewModel> subjects = [];
        public bool typeSelected = false;
        public UserSubjectState enrolType;
        public DegreeViewModel degree;
        public MajorViewModel major;

        public void SetEnrolType(UserSubjectState type)
        {
            enrolType = type;
        }

        public async void LoadSubjects()
        {
            typeSelected = true;
        }

		protected override async Task OnInitializedAsync()
		{
            var authState = await AuthenticationState;
            var userId = int.Parse(authState.User.Identity.Name);
            degree = UserService.GetDegreeForUser(userId);
            major = UserService.GetUserMajor(userId);
		}
	}
}
