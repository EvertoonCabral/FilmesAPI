using FilmesAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace FilmesAPI.Data.Dtos
{
    public class ReadCinemaDto
    {


        public int Id { get; set; }
        public String Nome { get; set; }
        public ReadEnderecoDto Endereco { get; set; }

        public ICollection<Sessao> Sessoes { get; set; }

    }
}
