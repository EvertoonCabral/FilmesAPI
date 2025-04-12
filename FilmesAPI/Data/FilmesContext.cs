using FilmesAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FilmesAPI.Data
{

    public class FilmesContext : DbContext
    {

        public FilmesContext(DbContextOptions<FilmesContext>  options) : base(options) 
        {
                
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Sessao>()
                .HasKey(sessao => new { sessao.FilmeId, sessao.CinemaId });

            builder.Entity<Sessao>()
                .HasOne(sessao => sessao.Cinema)
                .WithMany(cinema => cinema.Sessoes)
                .HasForeignKey(sessao => sessao.CinemaId);

            builder.Entity<Sessao>()
                .HasOne(sessao => sessao.Filme)
                .WithMany(filme => filme.Sessoes)
                .HasForeignKey(sessao => sessao.FilmeId);
        }

        public DbSet<Filme> Filmes { get; set; }
        public DbSet<Cinema> Cinemas { get; set; }

        public DbSet<Sessao> Sessoes { get; set; }

    }
}
