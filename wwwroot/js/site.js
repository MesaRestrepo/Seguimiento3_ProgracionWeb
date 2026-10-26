// Cálculo en vivo del total en el formulario de inscripción.
// Debe reflejar la misma regla que CalculadoraMatricula.cs (el servidor recalcula siempre).
(function () {
    const datos = document.getElementById('datos-precios');
    const curso = document.getElementById('CursoId');
    const meses = document.getElementById('Meses');
    if (!datos || !curso || !meses) {
        return;
    }

    const precios = JSON.parse(datos.textContent);
    const formato = new Intl.NumberFormat('es-CO', { style: 'currency', currency: 'COP', maximumFractionDigits: 0 });

    const porcentaje = (m) => (m >= 12 ? 0.15 : m >= 6 ? 0.10 : 0);

    function actualizar() {
        const precio = precios[curso.value] || 0;
        const m = parseInt(meses.value, 10) || 0;
        const subtotal = precio * m;
        const descuento = Math.round(subtotal * porcentaje(m));

        document.getElementById('res-subtotal').textContent = formato.format(subtotal);
        document.getElementById('res-descuento').textContent = '- ' + formato.format(descuento);
        document.getElementById('res-total').textContent = formato.format(subtotal - descuento);
    }

    curso.addEventListener('change', actualizar);
    meses.addEventListener('input', actualizar);
    actualizar();
})();
