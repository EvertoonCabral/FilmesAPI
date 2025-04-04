﻿using AutoMapper;
using FilmesAPI.Data;
using FilmesAPI.Data.Dtos;
using FilmesAPI.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

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
    public IEnumerable<ReadFilmeDto> recuperaFilmes([FromQuery]int skip =0, [FromQuery] int take =50 )
    {

        return _mapper.Map<List<ReadFilmeDto>>(_context.Filmes.Skip(skip).Take(take));

    }


    [HttpGet("{id}")]
    public IActionResult? recuperaFilmesPorId(int id)
    {
        
        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
        if (filme == null) return NotFound();

        var filmeDto = _mapper.Map<ReadFilmeDto>(filme);

        return Ok(filmeDto);
        

    }

    [HttpPut("id")]
    public IActionResult atualizarFilme(int idFilme, [FromBody] UpdateFilmeDto filmeDto)
    {

        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == idFilme);

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
