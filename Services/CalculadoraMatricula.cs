namespace EscuelaMusica.Services;

public static class CalculadoraMatricula
{
    // 10% desde 6 meses, 15% con 12 meses.
    public static decimal PorcentajeDescuento(int meses) => meses switch
    {
        >= 12 => 0.15m,
        >= 6 => 0.10m,
        _ => 0m
    };

    public static (decimal descuento, decimal total) Calcular(decimal precioMensual, int meses)
    {
        var subtotal = precioMensual * meses;
        var descuento = Math.Round(subtotal * PorcentajeDescuento(meses), 0);
        return (descuento, subtotal - descuento);
    }
}
