using Biblioteca.Web.Models;
using Biblioteca.Web.Repositorios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Biblioteca.Web.Controllers
{
    public class LibrosController : Controller
    {
        private readonly LibroRepositorio _libroRepo;

        public LibrosController(LibroRepositorio libroRepo)
        {
            _libroRepo = libroRepo;
        }

        public async Task<IActionResult> Index(string? titulo)
        {
            ViewData["Title"] = "Listado de Libros";
            ViewData["Busqueda"] = titulo;

            var libros = string.IsNullOrWhiteSpace(titulo)
                ? await _libroRepo.ObtenerTodosAsync()
                : await _libroRepo.BuscarPorTituloAsync(titulo);

            return View(libros);
        }

        public async Task<IActionResult> Details(int id)
        {
            ViewData["Title"] = "Detalle del Libro";
            var libro = await _libroRepo.ObtenerPorIdAsync(id);
            if (libro == null) return NotFound();
            return View(libro);
        }

        public async Task<IActionResult> Create()
        {
            ViewData["Title"] = "Nuevo Libro";
            await CargarAutoresAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Libro libro)
        {
            if (ModelState.IsValid)
            {
                await _libroRepo.InsertarAsync(libro);
                TempData["Mensaje"] = "Libro registrado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            await CargarAutoresAsync();
            return View(libro);
        }

        public async Task<IActionResult> Edit(int id)
        {
            ViewData["Title"] = "Editar Libro";
            var libro = await _libroRepo.ObtenerPorIdAsync(id);
            if (libro == null) return NotFound();

            await CargarAutoresAsync();
            return View(libro);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Libro libro)
        {
            if (id != libro.LibroId) return BadRequest();

            if (ModelState.IsValid)
            {
                await _libroRepo.ActualizarAsync(libro);
                TempData["Mensaje"] = "Libro actualizado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            await CargarAutoresAsync();
            return View(libro);
        }

        public async Task<IActionResult> Delete(int id)
        {
            ViewData["Title"] = "Eliminar Libro";
            var libro = await _libroRepo.ObtenerPorIdAsync(id);
            if (libro == null) return NotFound();
            return View(libro);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _libroRepo.EliminarAsync(id);
            TempData["Mensaje"] = "Libro eliminado correctamente (eliminación lógica).";
            return RedirectToAction(nameof(Index));
        }

        private async Task CargarAutoresAsync()
        {
            var autores = await _libroRepo.ObtenerAutoresAsync();
            ViewBag.Autores = new SelectList(autores, "AutorId", "Nombre");
        }
    }
}