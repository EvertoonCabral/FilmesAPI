using System.ComponentModel.DataAnnotations;

namespace FilmesAPI.Data.Dtos;

public class CreateFilmeDto
{



    [Required(ErrorMessage = "Titulo é obrigatorio")]
    [StringLength(50, ErrorMessage = "Tamanho maximo do titulo 50 caracteres")]
    public String Titulo { get; set; }

    [Required(ErrorMessage = "Genero é obrigatorio")]
    public String Genero { get; set; }
    [Required(ErrorMessage = "Duracao do filme é obrigatorio")]
    [Range(70, 600)]
    public int Duracao { get; set; }


}
