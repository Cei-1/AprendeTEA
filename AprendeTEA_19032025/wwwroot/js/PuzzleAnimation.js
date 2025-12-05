/*
Este script (`PuzzleAnimation.js`) es responsable de generar y animar
piezas de rompecabezas aleatorias en el fondo de la página.
Ajusta la cantidad, tamaño, posición, duración y tipo de animación de cada pieza
para crear un efecto dinámico y variado.
*/

document.addEventListener('DOMContentLoaded', () => {
    const body = document.body;
    const numberOfPieces = 25; // Define la cantidad total de piezas de rompecabezas que se mostrarán en pantalla.
    const puzzleEmoji = "🧩"; // El emoji que se utilizará para representar cada pieza.
    const colors = ["#729BFF", "#E38888", "#88D48E", "#B2A1FF", "#09cbb4"]; // Lista de colores proporcionados por el usuario

    /**
     * Crea un nuevo elemento div que representa una pieza de rompecabezas
     * y le asigna estilos y animaciones aleatorias.
     */
    function createPuzzlePiece() {
        const piece = document.createElement('div');
        piece.classList.add('puzzle-piece'); // Aplica la clase CSS base para las piezas
        piece.textContent = puzzleEmoji; // Establece el emoji como contenido de la pieza

        // Genera un tamaño aleatorio para la pieza, entre 40px y 100px.
        const size = Math.random() * (100 - 40) + 40;
        piece.style.fontSize = `${size}px`;

        // Establece una posición inicial aleatoria para la pieza dentro de la ventana del navegador.
        const startX = Math.random() * window.innerWidth;
        const startY = Math.random() * window.innerHeight;
        piece.style.left = `${startX}px`;
        piece.style.top = `${startY}px`;

        // Genera una duración de animación aleatoria entre 15 y 40 segundos.
        const duration = Math.random() * (40 - 15) + 15;
        piece.style.animationDuration = `${duration}s`;

        // Aplica un retraso de animación aleatorio (puede ser negativo para que algunas piezas ya estén en movimiento).
        const delay = Math.random() * -duration;
        piece.style.animationDelay = `${delay}s`;

        // Elige un tipo de animación aleatorio de la lista disponible.
        const animationTypes = ['float', 'fadeInOut', 'subtleFloat', 'bubbleUp'];
        const randomAnimation = animationTypes[Math.floor(Math.random() * animationTypes.length)];
        piece.style.animationName = randomAnimation;
        piece.style.animationIterationCount = 'infinite'; // Asegura que la animación se repita indefinidamente

        // Si la animación es 'float', se asignan variables CSS para un movimiento único por pieza.
        if (randomAnimation === 'float') {
            piece.style.setProperty('--float-x', (Math.random() - 0.5) * 200); // Desplazamiento horizontal +/- 100px
            piece.style.setProperty('--float-y', (Math.random() - 0.5) * 200); // Desplazamiento vertical +/- 100px
            piece.style.setProperty('--float-rotate', (Math.random() - 0.5) * 180); // Rotación +/- 90 grados
        }

        // Para las animaciones de 'fadeInOut' y 'bubbleUp', se ajusta la posición inicial
        // ya que estas animaciones manejan su propio `transform` y `opacity` de forma más controlada.
        if (randomAnimation === 'fadeInOut' || randomAnimation === 'bubbleUp') {
            piece.style.position = 'fixed'; // Se asegura de que la posición sea relativa a la ventana
            piece.style.bottom = '-100px'; // Para que las "burbujas" comiencen fuera de la vista inferior
            piece.style.left = `${Math.random() * 100}vw`; // Posición horizontal aleatoria en el viewport
            piece.style.opacity = '0'; // La opacidad inicial es 0, controlada por la animación
        }

        // Selecciona un color aleatorio de la lista y aplica la sombra de texto.
        const randomColor = colors[Math.floor(Math.random() * colors.length)];
        piece.style.textShadow = `2px 2px 5px ${randomColor}`;

        body.appendChild(piece); // Añade la pieza al cuerpo del documento

        // Manejador del evento `animationend` para animaciones que no son infinitas o necesitan reiniciarse.
        piece.addEventListener('animationend', () => {
            // Si la animación termina (como 'bubbleUp' o 'fadeInOut' que tienen un ciclo completo)
            // se remueve la pieza existente y se crea una nueva para mantener un flujo constante.
            if (randomAnimation === 'bubbleUp' || randomAnimation === 'fadeInOut') {
                piece.remove(); // Elimina la pieza actual del DOM
                createPuzzlePiece(); // Crea una nueva pieza para reemplazarla
            }
        });
    }

    // Bucle para crear el número inicial de piezas de rompecabezas al cargar la página.
    for (let i = 0; i < numberOfPieces; i++) {
        createPuzzlePiece();
    }

    // Opcional: Reajuste de la posición de las piezas al cambiar el tamaño de la ventana.
    // Esto ayuda a que las piezas se adapten a la nueva disposición sin agruparse o desaparecer.
    let resizeTimer;
    window.addEventListener('resize', () => {
        clearTimeout(resizeTimer); // Limpia cualquier temporizador previo para evitar múltiples ejecuciones.
        resizeTimer = setTimeout(() => {
            document.querySelectorAll('.puzzle-piece').forEach(piece => {
                // Solo reajusta la posición de las piezas con animaciones de flotación libre.
                // Las animaciones de 'bubbleUp' o 'fadeInOut' manejan su propia posición dinámica.
                if (piece.style.animationName !== 'bubbleUp' && piece.style.animationName !== 'fadeInOut') {
                    piece.style.left = `${Math.random() * window.innerWidth}px`;
                    piece.style.top = `${Math.random() * window.innerHeight}px`;
                }
            });
        }, 250); // Un pequeño retraso para evitar sobrecargar el navegador durante el redimensionamiento.
    });
});