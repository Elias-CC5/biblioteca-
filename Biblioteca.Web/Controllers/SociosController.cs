using Biblioteca.Web.Models;
using Biblioteca.Web.Repositorios;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Web.Controllers
{
    public class SociosController : Controller
    {
        private readonly SocioRepositorio _socioRepo;

        public SociosController(SocioRepositorio socioRepo)
        {
            _socioRepo = socioRepo;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Listado de Socios";
            var socios = await _socioRepo.ObtenerTodosAsync();
            return View(socios);
        }

        public IActionResult Create()
        {
            ViewData["Title"] = "Nuevo Socio";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Socio socio)
        {
            if (await _socioRepo.ExisteDNIAsync(socio.DNI))
            {
                ModelState.AddModelError("DNI", "El DNI ingresado ya existe.");
            }

            if (ModelState.IsValid)
            {
                await _socioRepo.InsertarAsync(socio);
                TempData["Mensaje"] = "Socio registrado con éxito.";
                return RedirectToAction(nameof(Index));
            }

            return View(socio);
        }
    }
}