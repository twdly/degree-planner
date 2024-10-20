using DegreePlanner.Data;
using DegreePlanner.ViewModels;

namespace DegreePlanner.Components.Pages.Student
{
	public partial class Subjects
    {
        public List<SubjectViewModel> subjects = [];
        public bool typeSelected = false;
        public UserSubjectState enrolType;

        public void SetEnrolType(UserSubjectState type)
        {
            enrolType = type;
        }

        public async void LoadSubjects()
        {
            typeSelected = true;
        }
    }
}
