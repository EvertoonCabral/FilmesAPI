using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using FilmesAPI.Data.Dtos;
using FilmesAPI.Models;
using FilmesAPI.Data;

namespace FilmesApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SessaoController : ControllerBase
    {
        private FilmesContext _context;
        private IMapper _mapper;

        public SessaoController(FilmesContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost]
        public IActionResult AdicionaSessao(CreateSessaoDto dto)
        {
            Sessao sessao = _mapper.Map<Sessao>(dto);
            _context.Sessoes.Add(sessao);
            _context.SaveChanges();
            return CreatedAtAction(nameof(RecuperaSessoesPorId), new { filmeId = sessao.FilmeId, cinemaId = sessao.CinemaId }, sessao);
        }

        [HttpGet]
        public IEnumerable<ReadSessaoDto> RecuperaSessoes()
        {
            return _mapper.Map<List<ReadSessaoDto>>(_context.Sessoes.ToList());
        }

        [HttpGet("{filmeId}/{cinemaId}")]
        public IActionResult RecuperaSessoesPorId(int filmeId, int cinemaId)
        {
            Sessao sessao = _context.Sessoes.FirstOrDefault(sessao => sessao.FilmeId == filmeId && sessao.CinemaId == cinemaId);
            if (sessao != null)
            {
                ReadSessaoDto sessaoDto = _mapper.Map<ReadSessaoDto>(sessao);

                return Ok(sessaoDto);
            }
            return NotFound();
        }
    }
}