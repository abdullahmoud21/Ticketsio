using System.ComponentModel.DataAnnotations.Schema;

namespace Ticketsio.Models
{
    public class ActorMovie
    {
        [ForeignKey("Actors")]
        public int ActorsId { get; set; }
        public Actors Actors { get; set; }
        [ForeignKey("Movie")]
        public int MoviesId { get; set; }
        public Movie Movie { get; set; }
    }
}
