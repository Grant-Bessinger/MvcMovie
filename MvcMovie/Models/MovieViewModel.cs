using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.Data.OData.Query.SemanticAst;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MvcMovie.Models
{
    public class MovieViewModel
    {
        public int Id { get; set; }

        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string Title { get; set; }

        [Display(Name = "Release Date")]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        
        public string Genre { get; set; }


        [RegularExpression("^[\\.?\\d+]*$", ErrorMessage = "Please enter a valid number.")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        public string Rating { get; set; }

        public string? pathFile { get; set; }   
        public IFormFile? FileFrame { get; set; }

        public List<SelectListItem> MovieRate { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> GenreMovies { get; set; } = new List<SelectListItem>();
    }
}
