using AutoMapper;
using Core.Entidades;
using Core.Interfaces.IRepositories;
using Infraestructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using Shared.Requests;
using System.Globalization;


namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BibliotecaController : ControllerBase
    {
        private readonly ILibroRepository _libroRepository;
        private readonly IReservaRepository _reservaRepository;
        private readonly IGenero_LibrosRepository _genero_librosRepository;
        private readonly IMapper _mapper;
        public BibliotecaController(ILibroRepository libroRepository, IMapper _mapper, IGenero_LibrosRepository genero_librosRepository)
        {
            _libroRepository = libroRepository;
            this._mapper = _mapper;
            _genero_librosRepository = genero_librosRepository;
        }
        [HttpGet("consultar-libros")]
        public async Task<IActionResult> ConsultarLibros()
        {
            try
            {
                
                var list = await _libroRepository.ObtenerTodosAsync(x => x.Visible, "Genero");

                var libros = _mapper.Map<List<LibrosDTO>>(list.ToList());
                return Ok(libros);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPCIÓN: {ex.Message}");
                throw;
            }
        }

        [HttpPost("registrar-libro")]
        public async Task<IActionResult> RegistrarLibro([FromBody] LibroRequest libroRequest)
        {
            try
            {

                var libro = new Libro();


                libro.Autor = libroRequest.Autor;
                libro.Nombre = libroRequest.Titulo;
                libro.Disponible = true;
                libro.Visible = true;
                libro.Editorial = libroRequest.Editorial;
                libro.Genero_LibrosId = libroRequest.Genero_LibrosId;

                await _libroRepository.AddAsync(libro);
                await _libroRepository.GuardarCambiosAsync();
                return Ok();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPCION: {ex.Message}");
                throw;
            }

        }
        [HttpPut("modificar-libro/{id}")]
        public async Task<IActionResult> ModificarLibro(int id, [FromBody] LibroRequest libroRequest)
        {
            try
            {
                
                var libro = await _libroRepository.ObtenerPorIdAsync(id);

                if (libro == null)
                    return NotFound($"No se encontró un libro con el ID: {id}");

                if (!libro.Visible)
                    return BadRequest($"El libro con el ID: {id} está eliminado y no puede ser modificado.");

                
                libro.Autor = libroRequest.Autor;
                libro.Nombre = libroRequest.Titulo;
                libro.Editorial = libroRequest.Editorial;
                libro.Genero_LibrosId = libroRequest.Genero_LibrosId;


                _libroRepository.Actualizar(libro);
                await _libroRepository.GuardarCambiosAsync();

                return Ok($"El libro con el ID: {id} fue modificado correctamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPCIÓN: {ex.Message}");
                throw;
            }
        }

        [HttpPut("reactivar-libro/{id}")]
        public async Task<IActionResult> ReactivarLibro(int id)
        {
            try
            {
                
                var libro = await _libroRepository.ObtenerPorIdAsync(id);

                if (libro == null)
                    return NotFound($"No se encontró un libro con el ID: {id}");

                if (libro.Visible)
                    return BadRequest($"El libro con el ID: {id} ya está activo.");

                
                libro.Visible = true;

                
                _libroRepository.Actualizar(libro);
                await _libroRepository.GuardarCambiosAsync();

                return Ok($"El libro con el ID: {id} fue reactivado correctamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPCIÓN: {ex.Message}");
                throw;
            }
        }
        [HttpDelete("eliminar-libro/{id}")]
        public async Task<IActionResult> EliminarLibro(int id)
        {
            try
            {

                var libro = await _libroRepository.ObtenerPorIdAsync(id);

                if (libro == null)
                    return NotFound($"No se encontró un libro con el ID: {id}");

                if (!libro.Visible)
                    return BadRequest($"El libro con el ID: {id} ya está eliminado.");


                libro.Visible = false;


                _libroRepository.Actualizar(libro);
                await _libroRepository.GuardarCambiosAsync();

                return Ok($"El libro con el ID: {id} fue eliminado correctamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPCIÓN: {ex.Message}");
                throw;
            }
        }
        [HttpGet("obtener-generos-libros")]
        public async Task<IActionResult> ObtenerGenerosLibros()
        {
            try
            {
               
                var generos = await _genero_librosRepository.ObtenerTodosAsync(null);

                return Ok(generos);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPCIÓN: {ex.Message}");
                throw;
            }
        }




    }
}
