using DegreePlanner.Data;
using DegreePlanner.Services.Interfaces;
using DegreePlanner.ViewModels;
using Microsoft.AspNetCore.Components;

namespace DegreePlanner.Components.Pages.Admin
{
	public partial class AddSubject
	{
		[Inject]
		ISubjectService SubjectService { get; set; }

		[Inject]
		IDegreeService DegreeService { get; set; }

		[SupplyParameterFromForm]
		public SubjectDetails Details { get; set; }

		private string resultMessage = "";
		private bool createdSubjectDetails, attachingToDegree, attachingToMajor, error;
		Subject? createdSubject;
		DegreeSubjectType subjectType;

		private void CreateSubject()
		{
			if(Details.Name == null || Details.Name == "")
			{
				resultMessage = "No name was entered, please enter a name";
				error = true;
				return;
			}
			if(Details.Code == null || Details.Code == "")
			{
				resultMessage = "No code was entered, please enter a code";
				error = true;
				return;
			}
			if(!int.TryParse(Details.Code, out int code))
			{
				resultMessage = "Entered code was not a number, please ensure the code is a valid number";
				error = true;
				return;
			}

			createdSubject = new()
			{
				Name = Details.Name,
				SubjectCode = int.Parse(Details.Code),
			};
		}

		private void SelectDegree()
		{
			CreateSubject();
			if (error)
			{
				error = false;
				return;
			}
			resultMessage = "";
			createdSubjectDetails = true;
			attachingToDegree = true;
		}

		private void SetSubjectType(DegreeSubjectType type)
		{
			subjectType = type;
		}

		private void AttachToDegree(DegreeViewModel degree)
		{
			SubjectService.AddSubject(createdSubject);
			DegreeService.AttachToDegree(degree, createdSubject.SubjectId, subjectType);
			ReturnToMenu();
			//resultMessage = "Subject " + subject.SubjectCode + " " + subject.Name + " was created";
		}

		private void SelectMajor()
		{
			CreateSubject();
			if (error)
			{
				error = false;
				return;
			}
			resultMessage = "";
			createdSubjectDetails = true;
			attachingToMajor = true;
		}

		private void AttachToMajor(MajorViewModel major)
		{
			SubjectService.AddSubject(createdSubject);
			DegreeService.AttachToMajor(major, createdSubject.SubjectId);
			ReturnToMenu();
		}

		public class SubjectDetails
		{
			public string? Name { get; set; }
			public string? Code { get; set; }
		}

		private void ReturnToMenu()
		{
			resultMessage = "Created Subject";
			Details ??= new();
			createdSubjectDetails = false;
			attachingToDegree = false;
			attachingToMajor = false;
			createdSubject = null;
			subjectType = DegreeSubjectType.Major;
		}

		protected override Task OnInitializedAsync()
		{
			Details ??= new();
			createdSubjectDetails = false;
			attachingToDegree = false;
			attachingToMajor = false;
			createdSubject = null;
			subjectType = DegreeSubjectType.Major;
			return base.OnInitializedAsync();
		}
	}
}
