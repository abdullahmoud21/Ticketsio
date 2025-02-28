using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Ticketsio.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(60)]
        public string Name { get; set; }
        [ValidateNever]
        public List<Movie> Movies { get; set; }
    }
}
