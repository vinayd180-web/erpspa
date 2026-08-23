using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shivakala.Core.Models;
using Shivakala.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shivakala.Web.Controllers
{
    public class PaperController : Controller
    {
        private readonly ShivakalaDbContext _context;

        public PaperController(ShivakalaDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var papers = await _context.ExamPapers.ToListAsync();
            return View(papers);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ExamPaper paper, string questionsText)
        {
            Console.WriteLine($"Create called - Title: {paper.Title}, questionsText length: {questionsText?.Length ?? 0}");

            paper.CreatedAt = DateTime.UtcNow;
            paper.UpdatedAt = DateTime.UtcNow;
            paper.IsActive = true;
            paper.Questions = new List<Question>();

            if (!string.IsNullOrEmpty(questionsText))
            {
                var parsedQuestions = ParseQuestions(questionsText);
                if (parsedQuestions.Any())
                {
                    paper.Questions = parsedQuestions;
                    paper.TotalMarks = paper.Questions.Sum(q => q.Marks);
                }
            }

            _context.ExamPapers.Add(paper);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Paper created!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Preview(int id)
        {
            var paper = await _context.ExamPapers
                .Include(p => p.Questions)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (paper == null) return NotFound();
            return View(paper);
        }

        [Authorize(Roles = "Student,Parent,Teacher,Admin")]
        public async Task<IActionResult> TakeTest(int id)
        {
            var paper = await _context.ExamPapers
                .Include(p => p.Questions)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (paper == null) return NotFound();
            return View(paper);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitTest(int paperId, string studentName)
        {
            var paper = await _context.ExamPapers
                .Include(p => p.Questions)
                .FirstOrDefaultAsync(p => p.Id == paperId);
            if (paper == null) return NotFound();

            var attempt = new StudentTestAttempt
            {
                StudentName = studentName,
                ExamPaperId = paperId,
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow,
                TotalMarks = paper.TotalMarks,
                ObtainedMarks = 0,
                Status = "Completed",
                Answers = new List<StudentAnswer>()
            };

            foreach (var q in paper.Questions)
            {
                attempt.Answers.Add(new StudentAnswer
                {
                    QuestionId = q.Id,
                    Answer = "",
                    IsCorrect = false,
                    MarksObtained = 0
                });
            }

            _context.StudentTestAttempts.Add(attempt);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Test submitted!";
            return RedirectToAction(nameof(Result), new { id = attempt.Id });
        }

        public async Task<IActionResult> Result(int id)
        {
            var attempt = await _context.StudentTestAttempts
                .Include(a => a.Answers)
                .FirstOrDefaultAsync(a => a.Id == id);
            if (attempt == null) return NotFound();
            return View(attempt);
        }

        private List<Question> ParseQuestions(string text)
        {
            var questions = new List<Question>();
            var lines = text.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            int order = 1;

            foreach (var line in lines)
            {
                var trimmed = line.Trim();
                if (string.IsNullOrWhiteSpace(trimmed)) continue;

                var q = new Question
                {
                    QuestionText = trimmed,
                    DisplayOrder = order++,
                    Marks = 1,
                    QuestionType = "MCQ"
                };

                if (trimmed.Contains("|"))
                {
                    var parts = trimmed.Split('|');
                    if (parts.Length >= 6)
                    {
                        q.QuestionText = parts[0].Trim();
                        q.OptionA = parts[1].Trim();
                        q.OptionB = parts[2].Trim();
                        q.OptionC = parts[3].Trim();
                        q.OptionD = parts[4].Trim();
                        q.CorrectAnswer = parts[5].Trim().ToUpper();
                    }
                }

                questions.Add(q);
            }

            return questions;
        }
    }
}
