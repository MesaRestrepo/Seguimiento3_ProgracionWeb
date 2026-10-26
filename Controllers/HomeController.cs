using System.Diagnostics;
using EscuelaMusica.Models;
using EscuelaMusica.Services;
using Microsoft.AspNetCore.Mvc;

namespace EscuelaMusica.Controllers;

public class HomeController : Controller
{
    private readonly EscuelaRepositorio _repo;

    public HomeController(EscuelaRepositorio repo) => _repo = repo;

    public IActionResult Index()
    {
        // Los primeros cursos se muestran como destacados en la portada.
        return View(_repo.ObtenerCursos().Take(3).ToList());
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
