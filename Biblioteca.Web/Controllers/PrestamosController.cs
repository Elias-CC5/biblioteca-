using Biblioteca.Web.Repositorios;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Web.Controllers
{
    public class PrestamosController : Controller
    {
        private readonly SocioRepositorio _socioRepo;

        public PrestamosController(SocioRepositorio socioRepo)
        {
            _socioRepo = socioRepo;
        }

        public async Task<IActionResult> Reporte(DateTime? desde, DateTime? hasta)
        {
            ViewData["Title"] = "Reporte de Préstamos";

            DateTime fechaDesde = desde ?? DateTime.Now.AddDays(-30);
            DateTime fechaHasta = hasta ?? DateTime.Now;

            ViewData["FechaDesde"] = fechaDesde.ToString("yyyy-MM-dd");
            ViewData["FechaHasta"] = fechaHasta.ToString("yyyy-MM-dd");

            var reporte = await _socioRepo.ObtenerReportePrestamosAsync(fechaDesde, fechaHasta);
            return View(reporte);
        }
    }
}