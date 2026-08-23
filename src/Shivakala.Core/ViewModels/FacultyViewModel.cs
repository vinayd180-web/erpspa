namespace Shivakala.Core.ViewModels
{
    public sealed class FacultyViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Designation { get; set; } = string.Empty;
        public string Qualification { get; set; } = string.Empty;
        public string Experience { get; set; } = string.Empty;
        public string PhotoUrl { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public bool ShowOnAboutPage { get; set; }
        public string Speciality { get; set; } = string.Empty;  // Added
    }
}
