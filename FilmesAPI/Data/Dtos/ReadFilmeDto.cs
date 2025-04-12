using FilmesAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace FilmesAPI.Data.Dtos;

public class ReadFilmeDto
{



    public int Titulo { get; set; }
    public String Genero { get; set; }
    public int Duracao { get; set; }
    public DateTime HoraConsulta { get; set; } = DateTime.Now;
    public ICollection<Sessao> Sessoes { get; set; }


}
