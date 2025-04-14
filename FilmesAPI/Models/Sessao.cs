using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FilmesAPI.Models
{
   
        public class Sessao
        {
            [Required]
            public int? FilmeId { get; set; }
            [Required]
            public int? CinemaId { get; set; }

            public virtual Filme Filme { get; set; }
            public virtual Cinema Cinema { get; set; }
        }

    
}
