using AutoMapper;
using FilmesAPI.Data;
using FilmesAPI.Data.Dtos;
using FilmesAPI.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FilmesAPI.Controllers;

[ApiController]
[Route("Filme")]
public class FilmesController : ControllerBase
{

    private FilmesContext _context;
    private IMapper _mapper; 


    public FilmesController(FilmesContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpPost]
    public IActionResult AdicionarFilme([FromBody]CreateFilmeDto filmeDto)
    {

        Filme filme = _mapper.Map<Filme>(filmeDto);

        _context.Filmes.Add(filme);
        _context.SaveChanges();
        return CreatedAtAction(nameof(recuperaFilmesPorId), new { id = filme.Id }, filme);


    }

    [HttpGet]
    public IActionResult recuperaFilmes([FromQuery] int skip = 0,
        [FromQuery] int take = 50,
        [FromQuery] string? nomeCinema = null)
    {
        try
        {
            if (string.IsNullOrEmpty(nomeCinema))
            {
                var filmesSemFiltro = _context.Filmes
                    .Include(f => f.Sessoes)
                    .Skip(skip)
                    .Take(take)
                    .ToList();

                return Ok(_mapper.Map<List<ReadFilmeDto>>(filmesSemFiltro));
            }

            var filmes = _context.Filmes
                .Include(f => f.Sessoes)
                    .ThenInclude(s => s.Cinema)
                .Where(filme => filme.Sessoes.Any(sessao =>
                    sessao.Cinema.Nome.ToLower() == nomeCinema.ToLower()))
                .OrderBy(f=> f.Id)
                .Skip(skip)
                .Take(take)
                .ToList();

            if (filmes.Count == 0)
            {
                var cinemaExiste = _context.Cinemas
                    .Any(c => c.Nome.ToLower() == nomeCinema.ToLower());

                if (!cinemaExiste)
                {
                    return NotFound($"Nenhum cinema encontrado com o nome '{nomeCinema}'");
                }

                var sessoesDoCinema = _context.Sessoes
                    .Include(s => s.Cinema)
                    .Where(s => s.Cinema.Nome.ToLower() == nomeCinema.ToLower())
                    .ToList();

                if (sessoesDoCinema.Count == 0)
                {
                    return NotFound($"O cinema '{nomeCinema}' existe, mas não tem sessões cadastradas");
                }

                return NotFound($"O cinema '{nomeCinema}' tem {sessoesDoCinema.Count} sessões, mas nenhum filme correspondente aos critérios");
            }

            return Ok(_mapper.Map<List<ReadFilmeDto>>(filmes));
        }
        catch (Exception ex)
        {
            // Log a exceção
            return StatusCode(500, $"Erro interno: {ex.Message}");
        }
    }


    [HttpGet("{id}")]
    public IActionResult? recuperaFilmesPorId(int id)
    {
        
        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);

        if (filme == null) return NotFound();

        var filmeDto = _mapper.Map<ReadFilmeDto>(filme);

        return Ok(filmeDto);
        

    }

    [HttpPut("{idFilme}")]
    public IActionResult AtualizarFilme(int idFilme, [FromBody] UpdateFilmeDto filmeDto)
    {
        if (filmeDto == null)
        {
            return BadRequest("O corpo da requisição não foi enviado.");
        }

        var filme = _context.Filmes.FirstOrDefault(f => f.Id == idFilme);

        if (filme == null)
        {
            return NotFound();
        }

        _mapper.Map(filmeDto, filme);
        _context.SaveChanges();

        return NoContent();
    }



    [HttpPatch("id")]
    public IActionResult atualizarFilmeParcialmente(int idFilme, JsonPatchDocument<UpdateFilmeDto> patch)
    {

        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == idFilme);

        if (filme == null)
        {
            return NotFound();
        }

        var filmeParaAtualizar = _mapper.Map < UpdateFilmeDto >(filme);
        patch.ApplyTo(filmeParaAtualizar, ModelState);


        if (!TryValidateModel(filmeParaAtualizar))
        {

            return ValidationProblem(ModelState);

        }

        _mapper.Map(filmeParaAtualizar, filme);
        _context.SaveChanges();

        return NoContent();
    }

    [HttpDelete]
    public IActionResult DeletarFilmes(int idFilme)
    {
       var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == idFilme);

        if (filme == null)
        {
            return NotFound();
        }

        _context.Remove(filme);
        _context.SaveChanges();
        return NoContent();


    }

}
