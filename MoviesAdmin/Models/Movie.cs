using System.ComponentModel.DataAnnotations;
namespace MoviesAdmin.Models;

public class Movie
{
    public int Id { get; set; }

    [StringLength(210)]
    [Required]
    public string Title { get; set; } = string.Empty;

    [StringLength(700)]
    [Required]
    public string Synopsys { get; set; } = string.Empty;

    [StringLength(20)]
    [RegularExpression(@"^[a-zA-Z\-]+$", ErrorMessage = "Genre name can only contain '-' and letters.")]
    [Required]
    public string Genre { get; set; } = string.Empty;

    [Display(Name = "Age Rating")]
    [StringLength(5)]
    [RegularExpression(@"(?i)^(G|PG|PG\-13|R|TV\-Y|TV\-Y7|TV\-G|TV\-PG|TV\-14|TV\-MA)$", ErrorMessage = "Rating must match MPA or TV format. (e.g. PG-13)")]
    [Required]
    public string Rating { get; set; } = string.Empty;

    [Display(Name = "Movie Runtime (In Minutes)")]
    [Range(0, 300)]
    [Required]
    public int RuntimeInMinutes { get; set; }

    [Display(Name = "Movie Release Date")]
    [DataType(DataType.Date)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
    [Required]
    public DateTime ReleaseDate { get; set; }
}