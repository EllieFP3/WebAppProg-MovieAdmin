using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace MoviesAdmin.Models
{
    public class Genre
    {
        // primary key
        public int Id { get; set; }

        // navigation property
        [ValidateNever]
        public List<Movie> Movies { get; set; } = new List<Movie>();
        public string Title { get; set; } = string.Empty;
    }
}
