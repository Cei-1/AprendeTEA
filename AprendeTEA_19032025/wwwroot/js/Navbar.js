// Seleccionar elementos del DOM
const menuToggle = document.querySelector('.menuToggle');
const navigation = document.querySelector('.navigation');
const listItems = document.querySelectorAll('.list');

// Al cargar la página, verifica el estado guardado en localStorage
document.addEventListener('DOMContentLoaded', () => {
    const isOpen = localStorage.getItem('menuOpen') === 'true';
    if (isOpen) {
        navigation.classList.add('open');
    }
});

// Maneja el toggle del menú
menuToggle.onclick = function () {
    navigation.classList.toggle('open');

    // Guarda el estado del menú en localStorage
    const isOpen = navigation.classList.contains('open');
    localStorage.setItem('menuOpen', isOpen);
};

// Manejo de enlaces del menú con animación antes de la redirección
listItems.forEach((item) => {
    item.addEventListener('click', function (event) {
        // Evitar la redirección inmediata
        event.preventDefault();

        // Remover la clase 'active' de todos los elementos
        listItems.forEach((list) => list.classList.remove('active'));

        // Agregar la clase 'active' al elemento seleccionado
        this.classList.add('active');

        // Redirigir después de un pequeño retraso para mostrar la animación
        const link = this.querySelector('a');
        if (link && link.href) {
            setTimeout(() => {
                window.location.href = link.href;
            }, 500); // Cambia el tiempo si necesitas un retraso diferente
        }
    });
});
