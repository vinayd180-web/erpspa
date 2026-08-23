using System.Collections.Generic;

namespace Shivakala.Core.ViewModels
{
    public class HomePageViewModel
    {
        public SeoViewModel Seo { get; set; }

        // Hero Banner
        public string HeroBannerImageUrl { get; set; }
        public string HeroBannerAltText { get; set; }

        // Trending Banner
        public bool ShowTrendingBanner { get; set; }
        public string TrendingEyebrow { get; set; }
        public string TrendingTitle { get; set; }
        public string TrendingDescription { get; set; }
        public string TrendingLinkText { get; set; }
        public string TrendingLinkUrl { get; set; }
        public string TrendingImageUrl { get; set; }
        public string TrendingAltText { get; set; }

        // Statistics
        public bool ShowStatisticsSection { get; set; } = true;
        public List<StatViewModel> Statistics { get; set; } = new List<StatViewModel>();

        // Testimonials
        public bool ShowTestimonialsSection { get; set; }
        public string TestimonialsEyebrow { get; set; }
        public string TestimonialsTitle { get; set; }
        public List<TestimonialViewModel> Testimonials { get; set; }

        // Featured Courses
        public List<CourseCardViewModel> FeaturedCourses { get; set; }

        // Highlights (using existing HighlightViewModel)
        public List<HighlightViewModel> Highlights { get; set; }

        // Results
        public List<ResultViewModel> Results { get; set; }

        // Faculty Members
        public List<FacultyViewModel> FacultyMembers { get; set; }
    }

    public class StatViewModel
    {
        public string Value { get; set; }
        public string Label { get; set; }
    }
}
