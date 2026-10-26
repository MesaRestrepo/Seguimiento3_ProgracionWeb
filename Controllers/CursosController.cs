using EscuelaMusica.Models;
using EscuelaMusica.Services;
using Microsoft.AspNetCore.Mvc;

namespace EscuelaMusica.Controllers;

public class CursosController : Controller
{
    private readonly EscuelaRepositorio _repo;

    public CursosController(EscuelaRepositorio repo) => _repo = repo;

    public IActionResult Index(string? instrumento, Nivel? nivel)
    {
        ViewBag.Instrumentos = _repo.ObtenerInstrumentos();
        ViewBag.InstrumentoSeleccionado = instrumento;
        ViewBag.NivelSeleccionado = nivel;
        return View(_repo.ObtenerCursos(instrumento, nivel));
    }

    public IActionResult Detalle(int id)
    {
        var curso = _repo.ObtenerCurso(id);
        return curso is null ? NotFound() : View(curso);
    }
}
