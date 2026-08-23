using System;

namespace Shivakala.Core.Models
{
    public class StudentAnswer : BaseEntity
    {
        public int StudentTestAttemptId { get; set; }
        public int QuestionId { get; set; }
        public string? Answer { get; set; }
        public bool IsCorrect { get; set; }
        public int MarksObtained { get; set; }
        public string? TeacherFeedback { get; set; } // Made nullable
    }
}
