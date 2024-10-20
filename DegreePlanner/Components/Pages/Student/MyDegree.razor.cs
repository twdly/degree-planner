using DegreePlanner.Services.Interfaces;
using DegreePlanner.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace DegreePlanner.Components.Pages.Student
{
	public partial class MyDegree
	{
		[Inject]
		IDegreeService degreeService { get; set; }

		[CascadingParameter]
		private Task<AuthenticationState> authenticationState { get; set; }

		public DegreeViewModel? degree;
		public List<DegreeViewModel>? availableDegrees;
		public List<MajorViewModel>? majors;
		public int degreeSelection = 0;
		public string enrolMessage = "";

		protected override async Task OnInitializedAsync()
		{
			var authState = await authenticationState;
			degree = degreeService.GetDegreeForUser(int.Parse(authState.User.Identity.Name)); // User ID is stored in the name field of the authstate
			if (degree == null)
			{
				availableDegrees = degreeService.GetAllDegrees();
			}
			else
			{
				majors = degreeService.GetMajorsForDegree(degree);
			}
		}

		private async void UpdateSelection(int degreeId)
		{
			degreeSelection = degreeId;
		}

		private async void EnrolInDegree()
		{
			var authState = await authenticationState;
			degreeService.EnrolInDegree(int.Parse(authState.User.Identity.Name), degreeSelection);
			enrolMessage = "You have successfully enrolled!";
			await OnInitializedAsync();
		}
	}
}
