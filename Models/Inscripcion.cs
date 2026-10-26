using System.ComponentModel.DataAnnotations;

namespace EscuelaMusica.Models;

public class Inscripcion
{
    public int Id { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Seleccione un curso.")]
    [Display(Name = "Curso")]
    public int CursoId { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(80, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 80 caracteres.")]
    [Display(Name = "Nombre completo")]
    public string NombreEstudiante { get; set; } = string.Empty;

    [Required(ErrorMessage = "El correo es obligatorio.")]
    [EmailAddress(ErrorMessage = "Correo no válido.")]
    [Display(Name = "Correo electrónico")]
    public string Correo { get; set; } = string.Empty;

    [Required(ErrorMessage = "El teléfono es obligatorio.")]
    [Phone(ErrorMessage = "Teléfono no válido.")]
    [Display(Name = "Teléfono")]
    public string Telefono { get; set; } = string.Empty;

    [Range(5, 90, ErrorMessage = "La edad debe estar entre 5 y 90 años.")]
    public int Edad { get; set; }

    [Range(1, 12, ErrorMessage = "Los meses deben estar entre 1 y 12.")]
    [Display(Name = "Meses a contratar")]
    public int Meses { get; set; } = 1;

    [Required(ErrorMessage = "Seleccione un horario.")]
    [Display(Name = "Horario preferido")]
    public string Horario { get; set; } = string.Empty;

    [Display(Name = "Fecha de inscripción")]
    [DataType(DataType.DateTime)]
    public DateTime FechaInscripcion { get; set; } = DateTime.Now;

    // Calculados en el controlador a partir del curso elegido.
    public string NombreCurso { get; set; } = string.Empty;
    public decimal Descuento { get; set; }
    public decimal Total { get; set; }
}
