using System.ComponentModel.DataAnnotations;

namespace EscuelaMusica.Models;

public class Curso
{
    public int Id { get; set; }

    [Display(Name = "Curso")]
    public string Nombre { get; set; } = string.Empty;

    public string Instrumento { get; set; } = string.Empty;

    public string Profesor { get; set; } = string.Empty;

    public Nivel Nivel { get; set; }

    [Display(Name = "Descripción")]
    public string Descripcion { get; set; } = string.Empty;

    [Display(Name = "Precio mensual")]
    [DataType(DataType.Currency)]
    public decimal PrecioMensual { get; set; }

    [Display(Name = "Cupos disponibles")]
    public int Cupos { get; set; }

    public string Imagen { get; set; } = string.Empty;
}
