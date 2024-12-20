﻿using DegreePlanner.ViewModels;

namespace DegreePlanner.Data
{
    public class UserSubject
    {
        public int UserId { get; set; }
        public int SubjectId { get; set; }
        public UserSubjectState State { get; set; }
        public int Mark { get; set; }

        // No args constructor for EF
        public UserSubject() { }

        // Args constructor for development
        public UserSubject(SubjectViewModel viewModel, int userId, UserSubjectState state)
        {
            UserId = userId;
            SubjectId = viewModel.SubjectId;
            State = state;
            Mark = 0;
        }

        public UserSubject(int userId, int subjectId, UserSubjectState state)
        {
            UserId = userId;
            SubjectId = subjectId;
            State = state;
        }

        public UserSubject(TutorViewModel tutor, int subjectId)
        {
            UserId = tutor.Id;
            SubjectId = subjectId;
            State = UserSubjectState.Tutor;
        }

        public UserSubject(int userId, int subjectId, UserSubjectState state, int mark)
        {
            UserId = userId;
            SubjectId = subjectId;
            State = state;
            Mark = mark;
        }
    }
}
