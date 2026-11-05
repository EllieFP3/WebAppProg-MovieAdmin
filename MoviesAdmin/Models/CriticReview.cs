using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace MoviesAdmin.Models
{
    public class CriticReview
    {
        // primary key
        public int Id { get; set; }

        // foreign key
        public int MovieId { get; set; }

        //navigation property
        [ValidateNever]
        [ScaffoldColumn(false)]
        public Movie Movie { get; set; } = null!;

        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        public int Rating { get; set; }
        [Required]
        public bool IsPublished { get; set; }
        
        public string CreatedBy { get; set; } = string.Empty;

        [ScaffoldColumn(false)]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
