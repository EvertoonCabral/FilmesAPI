using System.ComponentModel.DataAnnotations;

namespace FilmesAPI.Models;

public class Filme
{
    [Required]
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage ="Titulo é obrigatorio")]
    [MaxLength (50, ErrorMessage = "Tamanho maximo do titulo 50 caracteres")]
    public String Titulo { get; set; }

    [Required(ErrorMessage = "Genero é obrigatorio")]
    public String Genero { get; set; }
    [Required(ErrorMessage = "Duracao do filme é obrigatorio")]
    [Range(70, 600)]
    public int Duracao { get; set; }

}
