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

		protected override async Task OnInitializedAsync()
		{
			var authState = await authenticationState;
			degree = degreeService.GetDegreeForUser(int.Parse(authState.User.Identity.Name)); // User ID is stored in the name field of the authstate
		}
	}
}
