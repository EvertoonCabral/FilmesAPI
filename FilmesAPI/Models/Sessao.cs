using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FilmesAPI.Models
{
    public class Sessao
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int? FilmeId { get; set; }
        [Required]
        public int? CinemaId { get; set; } 

        public virtual Filme Filme { get; set; }
        public virtual Cinema Cinema { get; set; }



    }
}
