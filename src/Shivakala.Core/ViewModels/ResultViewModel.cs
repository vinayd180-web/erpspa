namespace Shivakala.Core.ViewModels
{
    public sealed class ResultViewModel
    {
        public int Id { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string Class { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public decimal Marks { get; set; }
        public decimal TotalMarks { get; set; }
        public string Grade { get; set; } = string.Empty;
        public int Rank { get; set; }
        public int ExamId { get; set; }
        public string ExamName { get; set; } = string.Empty;
        public string School { get; set; } = string.Empty;
        public int Year { get; set; }
        // Additional properties for HomePage
        public string Icon { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
