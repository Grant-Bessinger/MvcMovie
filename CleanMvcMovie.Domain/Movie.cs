using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanMvcMovie.Domain
{
    internal class Movie
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
        public decimal Price { get; set; }

        public string Rating { get; set; }

        public string? pathFile { get; set; }
    }
}
