using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Shivakala.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPaperGeneratorTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_Batches_BatchId",
                table: "Attendances");

            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_Students_StudentId",
                table: "Attendances");

            migrationBuilder.DropForeignKey(
                name: "FK_ExamResults_Students_StudentId",
                table: "ExamResults");

            migrationBuilder.DropForeignKey(
                name: "FK_FeePayments_Students_StudentId",
                table: "FeePayments");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentBatches_Batches_BatchId",
                table: "StudentBatches");

            migrationBuilder.DropIndex(
                name: "IX_FeePayments_ReceiptNumber",
                table: "FeePayments");

            migrationBuilder.DropIndex(
                name: "IX_ExamResults_ExamId_StudentId",
                table: "ExamResults");

            migrationBuilder.DropIndex(
                name: "IX_Courses_Slug",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Attendances_StudentId_BatchId_Date_Subject",
                table: "Attendances");

            migrationBuilder.DropIndex(
                name: "IX_AppUsers_Email",
                table: "AppUsers");

            migrationBuilder.DropIndex(
                name: "IX_AppUsers_Username",
                table: "AppUsers");

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.AlterColumn<decimal>(
                name: "MonthlySalary",
                table: "Teachers",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "PaidAmount",
                table: "FeePayments",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Fine",
                table: "FeePayments",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Discount",
                table: "FeePayments",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "FeePayments",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)");

            migrationBuilder.CreateTable(
                name: "ExamPapers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Class = table.Column<string>(type: "TEXT", nullable: false),
                    Subject = table.Column<string>(type: "TEXT", nullable: false),
                    PaperType = table.Column<string>(type: "TEXT", nullable: false),
                    TotalMarks = table.Column<int>(type: "INTEGER", nullable: false),
                    DurationMinutes = table.Column<int>(type: "INTEGER", nullable: false),
                    ExamDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Instructions = table.Column<string>(type: "TEXT", nullable: false),
                    IsPublished = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedBy = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamPapers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StudentTestAttempts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StudentId = table.Column<int>(type: "INTEGER", nullable: false),
                    StudentName = table.Column<string>(type: "TEXT", nullable: false),
                    ExamPaperId = table.Column<int>(type: "INTEGER", nullable: false),
                    StartTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TotalMarks = table.Column<int>(type: "INTEGER", nullable: false),
                    ObtainedMarks = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentTestAttempts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ExamPaperId = table.Column<int>(type: "INTEGER", nullable: false),
                    QuestionText = table.Column<string>(type: "TEXT", nullable: false),
                    QuestionType = table.Column<string>(type: "TEXT", nullable: false),
                    Marks = table.Column<int>(type: "INTEGER", nullable: false),
                    OptionA = table.Column<string>(type: "TEXT", nullable: false),
                    OptionB = table.Column<string>(type: "TEXT", nullable: false),
                    OptionC = table.Column<string>(type: "TEXT", nullable: false),
                    OptionD = table.Column<string>(type: "TEXT", nullable: false),
                    CorrectAnswer = table.Column<string>(type: "TEXT", nullable: false),
                    ModelAnswer = table.Column<string>(type: "TEXT", nullable: false),
                    DisplayOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questions_ExamPapers_ExamPaperId",
                        column: x => x.ExamPaperId,
                        principalTable: "ExamPapers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentAnswers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StudentTestAttemptId = table.Column<int>(type: "INTEGER", nullable: false),
                    QuestionId = table.Column<int>(type: "INTEGER", nullable: false),
                    Answer = table.Column<string>(type: "TEXT", nullable: false),
                    IsCorrect = table.Column<bool>(type: "INTEGER", nullable: false),
                    MarksObtained = table.Column<int>(type: "INTEGER", nullable: false),
                    TeacherFeedback = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentAnswers_StudentTestAttempts_StudentTestAttemptId",
                        column: x => x.StudentTestAttemptId,
                        principalTable: "StudentTestAttempts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExamResults_ExamId",
                table: "ExamResults",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_StudentId",
                table: "Attendances",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_ExamPaperId",
                table: "Questions",
                column: "ExamPaperId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAnswers_StudentTestAttemptId",
                table: "StudentAnswers",
                column: "StudentTestAttemptId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_Batches_BatchId",
                table: "Attendances",
                column: "BatchId",
                principalTable: "Batches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_Students_StudentId",
                table: "Attendances",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExamResults_Students_StudentId",
                table: "ExamResults",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FeePayments_Students_StudentId",
                table: "FeePayments",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentBatches_Batches_BatchId",
                table: "StudentBatches",
                column: "BatchId",
                principalTable: "Batches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_Batches_BatchId",
                table: "Attendances");

            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_Students_StudentId",
                table: "Attendances");

            migrationBuilder.DropForeignKey(
                name: "FK_ExamResults_Students_StudentId",
                table: "ExamResults");

            migrationBuilder.DropForeignKey(
                name: "FK_FeePayments_Students_StudentId",
                table: "FeePayments");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentBatches_Batches_BatchId",
                table: "StudentBatches");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "StudentAnswers");

            migrationBuilder.DropTable(
                name: "ExamPapers");

            migrationBuilder.DropTable(
                name: "StudentTestAttempts");

            migrationBuilder.DropIndex(
                name: "IX_ExamResults_ExamId",
                table: "ExamResults");

            migrationBuilder.DropIndex(
                name: "IX_Attendances_StudentId",
                table: "Attendances");

            migrationBuilder.AlterColumn<decimal>(
                name: "MonthlySalary",
                table: "Teachers",
                type: "decimal(10,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "PaidAmount",
                table: "FeePayments",
                type: "decimal(10,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<decimal>(
                name: "Fine",
                table: "FeePayments",
                type: "decimal(10,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<decimal>(
                name: "Discount",
                table: "FeePayments",
                type: "decimal(10,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "FeePayments",
                type: "decimal(10,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "TEXT");

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "Description", "DescriptionMarathi", "DisplayOrder", "DurationMonths", "IsFeatured", "Slug", "Standard", "Title", "TitleMarathi" },
                values: new object[,]
                {
                    { 1, "Strong conceptual coaching for classes 8 to 10 with daily practice, tests, and mentorship.", "इयत्ता ८ वी ते १० वी साठी दैनंदिन सराव, चाचण्या आणि मार्गदर्शनासह मजबूत पाया घडवणारे प्रशिक्षण.", 1, 12, true, "foundation-batch", "8th - 10th", "Foundation Batch", "फाउंडेशन बॅच" },
                    { 2, "Exam-focused batch for SSC students with revision plans, paper solving, and result tracking.", "एसएससी विद्यार्थ्यांसाठी पुनरावृत्ती योजना, पेपर सोडवणे आणि निकाल विश्लेषणासह परीक्षा-केंद्रित बॅच.", 2, 10, true, "board-excellence", "10th SSC", "Board Excellence Program", "बोर्ड उत्कृष्टता कार्यक्रम" },
                    { 3, "Dedicated advanced coaching for Mathematics and Science with doubt-solving labs.", "गणित आणि विज्ञान विषयांसाठी विशेष शंका समाधान सत्रांसह प्रगत प्रशिक्षण.", 3, 8, true, "science-maths-mastery", "9th - 10th", "Science & Maths Mastery", "सायन्स आणि मॅथ्स मास्टरी" },
                    { 4, "Reasoning, language, and aptitude sessions tailored for scholarship aspirants.", "शिष्यवृत्ती विद्यार्थ्यांसाठी रिझनिंग, भाषा आणि अॅप्टिट्यूडचे नियोजित मार्गदर्शन.", 4, 6, false, "scholarship-prep", "5th - 8th", "Scholarship Preparation", "शिष्यवृत्ती तयारी" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeePayments_ReceiptNumber",
                table: "FeePayments",
                column: "ReceiptNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExamResults_ExamId_StudentId",
                table: "ExamResults",
                columns: new[] { "ExamId", "StudentId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Courses_Slug",
                table: "Courses",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_StudentId_BatchId_Date_Subject",
                table: "Attendances",
                columns: new[] { "StudentId", "BatchId", "Date", "Subject" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppUsers_Email",
                table: "AppUsers",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppUsers_Username",
                table: "AppUsers",
                column: "Username",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_Batches_BatchId",
                table: "Attendances",
                column: "BatchId",
                principalTable: "Batches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_Students_StudentId",
                table: "Attendances",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExamResults_Students_StudentId",
                table: "ExamResults",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FeePayments_Students_StudentId",
                table: "FeePayments",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentBatches_Batches_BatchId",
                table: "StudentBatches",
                column: "BatchId",
                principalTable: "Batches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
