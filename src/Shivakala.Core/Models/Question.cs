using System;

namespace Shivakala.Core.Models
{
    public class Question : BaseEntity
    {
        public int ExamPaperId { get; set; }
        public string QuestionText { get; set; }
        public string QuestionType { get; set; }
        public int Marks { get; set; }
        public string OptionA { get; set; }
        public string OptionB { get; set; }
        public string OptionC { get; set; }
        public string OptionD { get; set; }
        public string CorrectAnswer { get; set; }
        public string? ModelAnswer { get; set; } // Made nullable with ?
        public int DisplayOrder { get; set; }
    }
}
