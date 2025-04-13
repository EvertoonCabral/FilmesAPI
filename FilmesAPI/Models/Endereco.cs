using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FilmesAPI.Models
{
    public class Endereco
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public String Logradouro { get; set; }
        public int Numero { get; set; }
        public virtual Cinema Cinema { get; set; }


    }

}
