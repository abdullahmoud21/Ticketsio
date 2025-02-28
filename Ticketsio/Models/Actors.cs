using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Ticketsio.Models
{
    public class Actors
    {
        public int Id { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(60)]
        public string FirstName { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(60)]
        public string LastName { get; set; }
        [MinLength(3)]
        [MaxLength(500)]
        public string? Bio { get; set; }
        [MinLength(3)]
        [MaxLength(250)]
        public string? ProfilePicture { get; set; }
        [MinLength(3)]
        [MaxLength(500)]
        public string? News { get; set; }
        [ValidateNever]
        public List<Movie> Movies { get; set; }
        public List<ActorMovie> ActorMovies { get; set; } = new List<ActorMovie>();
    }
}
