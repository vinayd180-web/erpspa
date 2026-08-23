using System;
using System.Collections.Generic;

namespace Shivakala.Core.Models
{
    public class ExamPaper : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Class { get; set; }
        public string Subject { get; set; }
        public string PaperType { get; set; } // MCQ, Subjective, Mixed
        public int TotalMarks { get; set; }
        public int DurationMinutes { get; set; }
        public DateTime ExamDate { get; set; }
        public string Instructions { get; set; }
        public bool IsPublished { get; set; }
        public int CreatedBy { get; set; } // Teacher/Admin ID
        public List<Question> Questions { get; set; }
    }
}
