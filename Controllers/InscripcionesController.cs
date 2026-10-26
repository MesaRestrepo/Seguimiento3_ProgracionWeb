using EscuelaMusica.Models;
using EscuelaMusica.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EscuelaMusica.Controllers;

public class InscripcionesController : Controller
{
    private static readonly string[] Horarios =
    [
        "Mañana (8:00 - 12:00)",
        "Tarde (14:00 - 18:00)",
        "Noche (18:00 - 21:00)",
        "Sábados (9:00 - 13:00)"
    ];

    private readonly EscuelaRepositorio _repo;

    public InscripcionesController(EscuelaRepositorio repo) => _repo = repo;

    // GET: /Inscripciones
    public IActionResult Index() => View(_repo.ObtenerInscripciones());

    // GET: /Inscripciones/Crear?cursoId=2
    [HttpGet]
    public IActionResult Crear(int? cursoId)
    {
        CargarListas(cursoId);
        return View(new Inscripcion { CursoId = cursoId ?? 0 });
    }

    // POST: /Inscripciones/Crear
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Crear(Inscripcion inscripcion)
    {
        var curso = _repo.ObtenerCurso(inscripcion.CursoId);
        if (curso is not null && curso.Cupos <= 0)
        {
            ModelState.AddModelError(nameof(Inscripcion.CursoId), "El curso no tiene cupos disponibles.");
        }

        if (!ModelState.IsValid || curso is null)
        {
            CargarListas(inscripcion.CursoId);
            return View(inscripcion);
        }

        var (descuento, total) = CalculadoraMatricula.Calcular(curso.PrecioMensual, inscripcion.Meses);
        inscripcion.NombreCurso = curso.Nombre;
        inscripcion.Descuento = descuento;
        inscripcion.Total = total;
        inscripcion.FechaInscripcion = DateTime.Now;

        if (!_repo.Registrar(inscripcion))
        {
            ModelState.AddModelError(nameof(Inscripcion.CursoId), "El curso se quedó sin cupos.");
            CargarListas(inscripcion.CursoId);
            return View(inscripcion);
        }

        return RedirectToAction(nameof(Confirmacion), new { id = inscripcion.Id });
    }

    // GET: /Inscripciones/Confirmacion/1
    public IActionResult Confirmacion(int id)
    {
        var inscripcion = _repo.ObtenerInscripcion(id);
        return inscripcion is null ? NotFound() : View(inscripcion);
    }

    // POST: /Inscripciones/Cancelar/1
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Cancelar(int id)
    {
        if (_repo.Cancelar(id))
        {
            TempData["Exito"] = "Inscripción cancelada.";
        }
        else
        {
            TempData["Error"] = "No se encontró la inscripción.";
        }
        return RedirectToAction(nameof(Index));
    }

    private void CargarListas(int? cursoId)
    {
        ViewBag.Cursos = new SelectList(
            _repo.ObtenerCursos().Where(c => c.Cupos > 0 || c.Id == cursoId),
            nameof(Curso.Id), nameof(Curso.Nombre), cursoId);
        ViewBag.Horarios = new SelectList(Horarios);
        // Precios para el cálculo en vivo en JavaScript.
        ViewBag.Precios = _repo.ObtenerCursos().ToDictionary(c => c.Id, c => c.PrecioMensual);
    }
}
