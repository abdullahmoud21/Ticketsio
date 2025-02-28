using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Ticketsio.Data;

namespace Ticketsio.Models
{
    public class Movie
    {
        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(55)]
        public string Name { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(500)]
        public string Description { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
        public double Price { get; set; }

        [Required]
        [Url(ErrorMessage = "Invalid image URL format.")]
        public string ImgUrl { get; set; }

        [Required]
        [Url(ErrorMessage = "Invalid trailer URL format.")]
        public string TrailerUrl { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        [ValidateNever]
        public MovieStatus MovieStatus { get; set; }
        [ValidateNever]
        public Cinema Cinema { get; set; }
        [ValidateNever]
        public Category Category { get; set; }
        public List<ActorMovie> ActorMovies { get; set; } = new List<ActorMovie>();

    }
}

