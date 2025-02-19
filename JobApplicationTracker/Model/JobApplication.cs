using System.ComponentModel.DataAnnotations;

namespace JobApplicationTracker.Model
{
    public class JobApplication
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Company Name is required")]
        public required string CompanyName { get; set; }

        [Required(ErrorMessage = "Position is required")]
        public required string Position { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public required string Status { get; set; } // Applied, Interview, Offer, Rejected

        [Required(ErrorMessage = "Company Name is required")]
        public DateTime DateApplied { get; set; }
    }
}
