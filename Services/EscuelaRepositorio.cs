using EscuelaMusica.Models;

namespace EscuelaMusica.Services;

// Almacén en memoria (Singleton). Suficiente para la demostración; se puede
// reemplazar por Entity Framework sin cambiar los controladores.
public class EscuelaRepositorio
{
    private readonly object _lock = new();
    private readonly List<Curso> _cursos =
    [
        new() { Id = 1, Nombre = "Guitarra Acústica", Instrumento = "Guitarra", Profesor = "Carlos Mejía", Nivel = Nivel.Basico, PrecioMensual = 120000, Cupos = 8, Imagen = "guitarra.jpg", Descripcion = "Acordes, ritmos y primeras canciones desde la primera clase." },
        new() { Id = 2, Nombre = "Piano Clásico", Instrumento = "Piano", Profesor = "Laura Giraldo", Nivel = Nivel.Intermedio, PrecioMensual = 150000, Cupos = 5, Imagen = "piano.jpg", Descripcion = "Técnica, lectura de partitura y repertorio clásico." },
        new() { Id = 3, Nombre = "Violín", Instrumento = "Violín", Profesor = "Andrés Restrepo", Nivel = Nivel.Basico, PrecioMensual = 140000, Cupos = 4, Imagen = "violin.jpg", Descripcion = "Postura, afinación y arco para iniciar con buen oído." },
        new() { Id = 4, Nombre = "Canto Moderno", Instrumento = "Voz", Profesor = "Valentina Ruiz", Nivel = Nivel.Intermedio, PrecioMensual = 110000, Cupos = 10, Imagen = "canto.jpg", Descripcion = "Respiración, proyección y estilo en géneros populares." },
        new() { Id = 5, Nombre = "Batería", Instrumento = "Batería", Profesor = "Mateo Duque", Nivel = Nivel.Avanzado, PrecioMensual = 160000, Cupos = 3, Imagen = "bateria.jpg", Descripcion = "Coordinación, rudimentos y grooves de rock, funk y jazz." },
        new() { Id = 6, Nombre = "Teoría Musical", Instrumento = "Teoría", Profesor = "Laura Giraldo", Nivel = Nivel.Basico, PrecioMensual = 90000, Cupos = 15, Imagen = "teoria.jpg", Descripcion = "Solfeo, escalas, armonía básica y lectura rítmica." }
    ];
    private readonly List<Inscripcion> _inscripciones = [];
    private int _siguienteId = 1;

    public IReadOnlyList<Curso> ObtenerCursos(string? instrumento = null, Nivel? nivel = null)
    {
        lock (_lock)
        {
            return _cursos
                .Where(c => string.IsNullOrWhiteSpace(instrumento) || c.Instrumento.Equals(instrumento, StringComparison.OrdinalIgnoreCase))
                .Where(c => nivel is null || c.Nivel == nivel)
                .ToList();
        }
    }

    public IReadOnlyList<string> ObtenerInstrumentos()
    {
        lock (_lock)
        {
            return _cursos.Select(c => c.Instrumento).Distinct().OrderBy(i => i).ToList();
        }
    }

    public Curso? ObtenerCurso(int id)
    {
        lock (_lock)
        {
            return _cursos.FirstOrDefault(c => c.Id == id);
        }
    }

    public IReadOnlyList<Inscripcion> ObtenerInscripciones()
    {
        lock (_lock)
        {
            return _inscripciones.OrderByDescending(i => i.FechaInscripcion).ToList();
        }
    }

    public Inscripcion? ObtenerInscripcion(int id)
    {
        lock (_lock)
        {
            return _inscripciones.FirstOrDefault(i => i.Id == id);
        }
    }

    // Registra la inscripción y descuenta un cupo. Devuelve false si no hay cupos.
    public bool Registrar(Inscripcion inscripcion)
    {
        lock (_lock)
        {
            var curso = _cursos.FirstOrDefault(c => c.Id == inscripcion.CursoId);
            if (curso is null || curso.Cupos <= 0)
            {
                return false;
            }

            curso.Cupos--;
            inscripcion.Id = _siguienteId++;
            _inscripciones.Add(inscripcion);
            return true;
        }
    }

    // Elimina la inscripción y devuelve el cupo al curso.
    public bool Cancelar(int id)
    {
        lock (_lock)
        {
            var inscripcion = _inscripciones.FirstOrDefault(i => i.Id == id);
            if (inscripcion is null)
            {
                return false;
            }

            _inscripciones.Remove(inscripcion);
            var curso = _cursos.FirstOrDefault(c => c.Id == inscripcion.CursoId);
            if (curso is not null)
            {
                curso.Cupos++;
            }
            return true;
        }
    }
}
