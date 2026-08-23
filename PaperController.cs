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
            var papers = await _context.ExamPapers
                .Include(p => p.Questions)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
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
            if (!ModelState.IsValid)
            {
                return View(paper);
            }

            paper.CreatedAt = DateTime.UtcNow;
            paper.UpdatedAt = DateTime.UtcNow;
            paper.IsActive = true;
            paper.Questions = new List<Question>();

            if (string.IsNullOrEmpty(questionsText))
            {
                ModelState.AddModelError("", "Please add at least one question.");
                return View(paper);
            }

            var parsedQuestions = ParseQuestions(questionsText, paper.PaperType);
            if (!parsedQuestions.Any())
            {
                ModelState.AddModelError("", "No valid questions found. Please check the format.");
                return View(paper);
            }

            paper.Questions = parsedQuestions;
            paper.TotalMarks = paper.Questions.Sum(q => q.Marks);

            _context.ExamPapers.Add(paper);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Paper created successfully!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Preview(int id)
        {
            var paper = await _context.ExamPapers
                .Include(p => p.Questions)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (paper == null)
                return NotFound();

            return View(paper);
        }

        public async Task<IActionResult> TakeTest(int id)
        {
            var paper = await _context.ExamPapers
                .Include(p => p.Questions)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (paper == null)
                return NotFound();

            return View(paper);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitTest(int paperId, Dictionary<int, string> answers, string studentName)
        {
            if (paperId <= 0)
            {
                TempData["Error"] = "Invalid paper ID";
                return RedirectToAction(nameof(Index));
            }

            var paper = await _context.ExamPapers
                .Include(p => p.Questions)
                .FirstOrDefaultAsync(p => p.Id == paperId);

            if (paper == null)
            {
                TempData["Error"] = "Paper not found";
                return RedirectToAction(nameof(Index));
            }

            if (string.IsNullOrEmpty(studentName))
            {
                studentName = "Anonymous";
            }

            var attempt = new StudentTestAttempt
            {
                StudentName = studentName,
                ExamPaperId = paperId,
                StartTime = DateTime.UtcNow.AddMinutes(-paper.DurationMinutes),
                EndTime = DateTime.UtcNow,
                TotalMarks = paper.TotalMarks,
                Status = "Completed",
                Answers = new List<StudentAnswer>()
            };

            int obtainedMarks = 0;

            if (answers != null && answers.Any())
            {
                foreach (var question in paper.Questions)
                {
                    var studentAnswer = new StudentAnswer
                    {
                        QuestionId = question.Id,
                        Answer = answers.ContainsKey(question.Id) ? answers[question.Id] : ""
                    };

                    if (question.QuestionType == "MCQ")
                    {
                        studentAnswer.IsCorrect = studentAnswer.Answer == question.CorrectAnswer;
                        studentAnswer.MarksObtained = studentAnswer.IsCorrect ? question.Marks : 0;
                    }
                    else
                    {
                        studentAnswer.IsCorrect = false;
                        studentAnswer.MarksObtained = 0;
                    }

                    obtainedMarks += studentAnswer.MarksObtained;
                    attempt.Answers.Add(studentAnswer);
                }
            }
            else
            {
                foreach (var question in paper.Questions)
                {
                    attempt.Answers.Add(new StudentAnswer
                    {
                        QuestionId = question.Id,
                        Answer = "",
                        IsCorrect = false,
                        MarksObtained = 0
                    });
                }
            }

            attempt.ObtainedMarks = obtainedMarks;
            _context.StudentTestAttempts.Add(attempt);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"Test submitted! You scored {obtainedMarks}/{paper.TotalMarks}";
            return RedirectToAction(nameof(Result), new { id = attempt.Id });
        }

        public async Task<IActionResult> Result(int id)
        {
            var attempt = await _context.StudentTestAttempts
                .Include(a => a.Answers)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (attempt == null)
                return NotFound();

            return View(attempt);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var paper = await _context.ExamPapers.FindAsync(id);
            if (paper == null)
                return NotFound();
            return View(paper);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var paper = await _context.ExamPapers.FindAsync(id);
            _context.ExamPapers.Remove(paper);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Paper deleted!";
            return RedirectToAction(nameof(Index));
        }

        private List<Question> ParseQuestions(string text, string paperType)
        {
            var questions = new List<Question>();
            var lines = text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            int order = 1;

            foreach (var line in lines)
            {
                var trimmedLine = line.Trim();
                if (string.IsNullOrWhiteSpace(trimmedLine)) continue;

                var question = new Question
                {
                    QuestionText = trimmedLine,
                    DisplayOrder = order++,
                    Marks = 1,
                    QuestionType = "MCQ"
                };

                if (trimmedLine.Contains("|"))
                {
                    var parts = trimmedLine.Split('|');
                    if (parts.Length >= 6)
                    {
                        question.QuestionText = parts[0].Trim();
                        question.OptionA = parts[1].Trim();
                        question.OptionB = parts[2].Trim();
                        question.OptionC = parts[3].Trim();
                        question.OptionD = parts[4].Trim();
                        question.CorrectAnswer = parts[5].Trim().ToUpper();
                        question.QuestionType = "MCQ";
                        question.Marks = 1;
                    }
                    else if (parts.Length >= 5)
                    {
                        question.QuestionText = parts[0].Trim();
                        question.OptionA = parts.Length > 1 ? parts[1].Trim() : "";
                        question.OptionB = parts.Length > 2 ? parts[2].Trim() : "";
                        question.OptionC = parts.Length > 3 ? parts[3].Trim() : "";
                        question.OptionD = parts.Length > 4 ? parts[4].Trim() : "";
                        question.CorrectAnswer = "A";
                        question.QuestionType = "MCQ";
                        question.Marks = 1;
                    }
                }
                else if (paperType == "Subjective" || paperType == "Mixed")
                {
                    question.QuestionType = "Subjective";
                    question.Marks = 5;
                }

                questions.Add(question);
            }

            return questions;
        }
    }
}
