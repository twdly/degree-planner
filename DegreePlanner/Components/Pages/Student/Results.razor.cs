using DegreePlanner.Services.Interfaces;
using DegreePlanner.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace DegreePlanner.Components.Pages.Student
{
	public partial class Results
	{
		[CascadingParameter]
		private Task<AuthenticationState> authenticationState { get; set; }
		
		[Inject]
		ISubjectService SubjectService { get; set; }

		private List<SubjectViewModel> predictionSubjects;
		private List<SubjectViewModel> completedSubjects;
		private int userId;

		protected override async Task OnInitializedAsync()
		{
			var authState = await authenticationState;
			userId = int.Parse(authState.User.Identity.Name);
			
			predictionSubjects = SubjectService.GetPredictionSubjects(userId);
			completedSubjects = SubjectService.GetCompletedSubjects(userId);
		}

		private double GetCompletedWam()
		{
			int totalMarks = completedSubjects.Sum(x => x.Mark);
			if (completedSubjects.Count == 0) return 0;
			return (double)totalMarks / completedSubjects.Count;
		}

		private double GetPredictedWam()
		{
			predictionSubjects.ForEach(x => ValidateInputs(ref x.Mark));
			var predictedSubjects = predictionSubjects.Where(x => x.Mark != 0).ToList(); // Remove subjects with a mark of 0 as the user has not made a prediction
			int totalMarks = predictionSubjects.Sum(x => x.Mark);
			totalMarks += completedSubjects.Sum(x => x.Mark); // Completed subjects should still be counted in the prediction WAM
			int totalSubjects = predictedSubjects.Count + completedSubjects.Count;
			if (totalSubjects == 0) return 0;
			return (double)totalMarks / totalSubjects;
		}

		private void ValidateInputs(ref int value)
		{
			if (value < 0) value = 0;
			else if (value > 100) value = 100;
		}
	}
}
