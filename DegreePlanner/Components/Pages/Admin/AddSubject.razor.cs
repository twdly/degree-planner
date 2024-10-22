using DegreePlanner.Data;
using DegreePlanner.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace DegreePlanner.Components.Pages.Admin
{
	public partial class AddSubject
	{
		[Inject]
		ISubjectService SubjectService { get; set; }

		[SupplyParameterFromForm]
		public SubjectDetails Details { get; set; }

		private string resultMessage = "";

		private void CreateSubject()
		{
			if(Details.Name == null || Details.Name == "")
			{
				resultMessage = "No name was entered, please enter a name";
				return;
			}
			if(Details.Code == null || Details.Code == "")
			{
				resultMessage = "No code was entered, please enter a code";
				return;
			}

			Subject subject = new()
			{
				Name = Details.Name,
				SubjectCode = int.Parse(Details.Code),
			};

			SubjectService.AddSubject(subject);
			resultMessage = "Subject " + subject.SubjectCode + " " + subject.Name + " was created";
		}

		public class SubjectDetails
		{
			public string? Name { get; set; }
			public string? Code { get; set; }
		}

		protected override Task OnInitializedAsync()
		{
			Details ??= new();
			return base.OnInitializedAsync();
		}
	}
}
