using AutoMapper;
using Core.Entidades;
using Core.Interfaces.IRepositories;
using Infraestructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using Shared.Requests;
using System.Collections.Generic;
using System.IO;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeliculaController: ControllerBase
    {
        private readonly IPeliculaRepository _peliculaRepository;
        private readonly IMapper _mapper;
        private readonly IGenero_PeliculaRepository _genero_PeliculaRepository;

        public PeliculaController(IPeliculaRepository peliculaRepository, IGenero_PeliculaRepository genero_PeliculaRepository, IMapper mapper) 
        {
            _peliculaRepository = peliculaRepository;
            _genero_PeliculaRepository = genero_PeliculaRepository;
            _mapper = mapper;
        }
        [HttpPost("agregar-pelicula")]
        public async Task<IActionResult> AgregarPelicula([FromBody] PeliculaRequest peliculaRequest) 
        {
            try
            {
                var pelicula = new Pelicula
                {
                    Titulo = peliculaRequest.Titulo,
                    Director = peliculaRequest.Director,
                    Duracion = peliculaRequest.Duracion, // Duración en minutos
                    Genero_PeliculasId = peliculaRequest.Genero_PeliculasId
                };
                await _peliculaRepository.AgregarPelicula(pelicula);
                return Ok("Pelicula agregada con exito");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPCION: {ex.Message}");
                throw;
            }
        }


        [HttpPost("obtener-peliculas")]
        public async Task<IActionResult> ObtenerPeliculas(FiltroPeliculasDTO filtro = null)
        {
            try
            {

                var peliculas = new List<Pelicula>();

                if (filtro is not null)
                {
                    peliculas = await ObtenerFuncionesFiltradas(filtro);

                }
                else
                {
                    peliculas = _peliculaRepository.ObtenerPeliculas().Result.ToList();
                }

                if (peliculas is null || !peliculas.Any())
                {
                    return NotFound("No se encontraron funciones.");
                }



                var listaDTO = _mapper.Map<List<PeliculaDTO>>(peliculas.ToList());


                return Ok(listaDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPCION: {ex.Message}");
                throw;
            }
        }
        private async Task<List<Pelicula>> ObtenerFuncionesFiltradas(FiltroPeliculasDTO? filtro)
        {
            try
            {
                var peliculasFiltradas = await _peliculaRepository.ObtenerTodosAsync(null, "Genero");

                if (filtro is not null)
                {
                    if (!string.IsNullOrWhiteSpace(filtro.Titulo))
                        peliculasFiltradas = peliculasFiltradas.Where(x => x.Titulo== filtro.Titulo);

                    if (!string.IsNullOrWhiteSpace(filtro.Genero))
                        peliculasFiltradas = peliculasFiltradas.Where(x => x.Genero.Descripcion == filtro.Genero);

                    if (!string.IsNullOrWhiteSpace(filtro.Director))
                        peliculasFiltradas = peliculasFiltradas.Where(x => x.Director == filtro.Director);
                }


                return peliculasFiltradas.OrderByDescending(x => x.Id).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("obtener-pelicula/{id}")]
        public async Task<IActionResult> ObtenerPelicula(int id)
        {
            try
            {

                var pelicula = await _peliculaRepository.ObtenerPeliculaPorId(id);
                if (pelicula == null)
                {
                    return NotFound($"No se encontró la película con el ID {id}");
                }
                return Ok(pelicula);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPCION: {ex.Message}");
                throw;
            }
        }
        [HttpPost("modificar-pelicula")]
        public async Task<IActionResult> ModificarPelicula([FromBody] PeliculaRequest peliculaRequest)
        {
            try
            {
                
                var peliculaExistente = await _peliculaRepository.ObtenerPeliculaPorId(peliculaRequest.Id);
                if (peliculaExistente == null)
                {
                    return NotFound($"No se encontró la película con el ID {peliculaRequest.Id}");
                }

                peliculaExistente.Titulo = peliculaRequest.Titulo;
                peliculaExistente.Director = peliculaRequest.Director;
                peliculaExistente.Duracion = peliculaRequest.Duracion;
                peliculaExistente.Genero_PeliculasId = peliculaRequest.Genero_PeliculasId;

                await _peliculaRepository.ModificarPelicula(peliculaExistente);
                return Ok("Película modificada con éxito");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPCION: {ex.Message}");
                throw;
            }
        }
        [HttpDelete("eliminar-pelicula/{id}")]
        public async Task<IActionResult> EliminarPelicula(int id)
        {
            try
            {

                var pelicula = await _peliculaRepository.ObtenerPeliculaPorId(id);
                if (pelicula == null)
                {
                    return NotFound($"No se encontró la película con el ID {id}");
                }

                await _peliculaRepository.EliminarPelicula(pelicula);
                return Ok("Película eliminada con éxito");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPCION: {ex.Message}");
                throw;
            }
        }
        [HttpGet("obtener-generos")]
        public async Task<IActionResult> ObtenerGeneros()
        {
            try
            {
                var generos = await _genero_PeliculaRepository.ObtenerTodosGenerosAsync();
                if (generos == null || !generos.Any())
                {
                    return NotFound("No se encontraron géneros de películas.");
                }

                var generosDTO = _mapper.Map<List<GeneroDTO>>(generos);
                return Ok(generosDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPCION: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }


    }
}
