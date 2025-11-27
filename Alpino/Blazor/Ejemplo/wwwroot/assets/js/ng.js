
export function formatoNuermos (numberInputID) {
    const numberInput = document.getElementById(numberInputID);

    // Variable para almacenar la posición del cursor
    let cursorPosition;

    // Función para formatear el número con separador de miles
    function formatNumber(num) {
        // Eliminar todos los puntos existentes y caracteres no numéricos excepto comas decimales
        const cleanNum = num.replace(/\./g, '').replace(/[^\d,]/g, '');

        // Si está vacío, devolver vacío
        if (cleanNum === '') return '';

        // Separar parte entera y decimal
        const parts = cleanNum.split(',');

        // Formatear la parte entera con puntos cada tres dígitos
        parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ".");

        // Reunir las partes (solo incluir la parte decimal si existe)
        return parts.length > 1 ? parts.join(',') : parts[0];
    }

    // Función para ajustar la posición del cursor después del formateo
    function adjustCursorPosition(originalValue, formattedValue, originalPosition) {
        // Contar cuántos puntos hay antes de la posición del cursor en el valor formateado
        const dotsBeforeCursor = (formattedValue.substring(0, originalPosition).match(/\./g) || []).length;

        // Contar cuántos puntos había antes de la posición del cursor en el valor original
        const originalDotsBeforeCursor = (originalValue.substring(0, originalPosition).match(/\./g) || []).length;

        // Ajustar la posición del cursor sumando la diferencia de puntos
        return originalPosition + (dotsBeforeCursor - originalDotsBeforeCursor);
    }

    numberInput.addEventListener('input', function (e) {
        // Guardar la posición del cursor y el valor original
        const originalPosition = this.selectionStart;
        const originalValue = this.value;

        // Formatear el valor
        const formattedValue = formatNumber(originalValue);

        // Actualizar el valor solo si ha cambiado
        if (formattedValue !== originalValue) {
            this.value = formattedValue;

            // Calcular y ajustar la nueva posición del cursor
            const newPosition = adjustCursorPosition(originalValue, formattedValue, originalPosition);
            this.setSelectionRange(newPosition, newPosition);
        }
    });

    // Formatear al cargar si hay un valor inicial
    if (numberInput.value) {
        numberInput.value = formatNumber(numberInput.value);
    }
}
