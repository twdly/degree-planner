using DegreePlanner.Services;
using DegreePlanner.Services.Interfaces;
using DegreePlanner.ViewModels;
using Microsoft.AspNetCore.Components;

namespace DegreePlanner.Components.Pages.Admin
{
	public partial class EditSubjectDetails
	{
		[Inject]
		ISubjectService SubjectService { get; set; }

		SubjectViewModel? selectedSubject = null;
		List<SubjectViewModel>? allSubjects;
		string? newName, newCode, errorMessage;

		protected override async Task OnInitializedAsync()
		{
			//var authState = await AuthenticationState;
			allSubjects = SubjectService.GetAllSubjects();
		}

		public void SelectSubject(SubjectViewModel chosenSubject)
		{
			selectedSubject = chosenSubject;
			newName = chosenSubject.Name;
			newCode = chosenSubject.SubjectCode.ToString();
		}

		public void SaveSubject(SubjectViewModel subject)
		{
			if (newName == null || newName == "")
			{
				errorMessage = "No name entered, please enter a name";
				return;
			}
			else if (newCode == null || newCode.Length <= 0)
			{
				errorMessage = "No Subject Code entered, please enter a code";
				return;
			}
			else if (!int.TryParse(newCode, out int code))
			{
				errorMessage = "Subject Code was not a number, please enter a valid number";
				return;
			}
			selectedSubject.Name = newName;
			selectedSubject.SubjectCode = int.Parse(newCode);
			SubjectService.UpdateSubject(subject);
			DeselectSubject();
		}

		public void DeselectSubject()
		{
			newName = null;
			newCode = null;
			errorMessage = null;
			selectedSubject = null;
		}
	}
}
