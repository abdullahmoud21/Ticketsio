using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Ticketsio.Models
{
    public class Cinema
    {
        public int Id { get; set; }
        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        public string Name { get; set; }
        [MinLength(5)]
        [MaxLength(500)]
        public string? Description { get; set; }
        [MinLength(3)]
        [MaxLength(80)]
        public string? CinemaLogo { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public string Address { get; set; }
        [ValidateNever]
        public List<Movie> Movies { get; set; }
    }
}
