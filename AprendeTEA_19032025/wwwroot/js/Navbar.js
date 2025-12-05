// Seleccionar elementos del DOM
const menuToggle = document.querySelector('.menuToggle');
const navigation = document.querySelector('.navigation');
const listItems = document.querySelectorAll('.list'); // Mantenemos esta selección por si se necesita más tarde, pero simplificamos su uso.

// Al cargar la página, NO TOCAMOS el localStorage para evitar conflictos de estado.
// Permitimos que el CSS y los media queries controlen el estado inicial en móvil.

// Maneja el toggle del menú
menuToggle.onclick = function () {
    // Simplemente alterna la clase 'open' para abrir o cerrar el menú.
    navigation.classList.toggle('open');
};

// Manejo de enlaces:
// Eliminamos la lógica de `event.preventDefault()` y `setTimeout`
// ya que la clase 'active' es manejada por Razor y el retraso no es ideal.
// Permitimos que los enlaces naveguen de forma inmediata.

listItems.forEach((item) => {
    item.addEventListener('click', function (event) {
        // En lugar de prevenir la navegación, simplemente cerramos el menú
        // de forma inmediata si estamos en móvil para mejorar la UX.

        // Añadimos una comprobación básica de pantalla (si el ancho es pequeño)
        if (window.innerWidth <= 768) {
            // Cerramos el menú inmediatamente después de hacer clic en un enlace.
            // Esto es crucial para la experiencia móvil.
            if (navigation.classList.contains('open')) {
                navigation.classList.remove('open');
            }
        }

        // La navegación se realiza de forma natural, sin retraso (setTimeout),
        // permitiendo que el servidor maneje la clase 'active' al recargar la página.
    });
});