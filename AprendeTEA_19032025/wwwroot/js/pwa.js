// Registro del Service Worker
if ('serviceWorker' in navigator) {
    window.addEventListener('load', () => {
        navigator.serviceWorker.register('/sw.js')
            .then(registration => {
                console.log('✅ Service Worker registrado:', registration.scope);

                // Verificar actualizaciones cada 24 horas
                setInterval(() => {
                    registration.update();
                }, 24 * 60 * 60 * 1000);
            })
            .catch(error => {
                console.error('❌ Error al registrar Service Worker:', error);
            });
    });

    // Detectar cuando hay una nueva versión disponible
    navigator.serviceWorker.addEventListener('controllerchange', () => {
        console.log('🔄 Nueva versión disponible');
        // Opcional: Mostrar notificación al usuario
        if (confirm('Hay una nueva versión disponible. ¿Deseas actualizar?')) {
            window.location.reload();
        }
    });
}

// Detectar si la app está instalada
let deferredPrompt;
window.addEventListener('beforeinstallprompt', (e) => {
    console.log('💾 Evento de instalación detectado');
    e.preventDefault();
    deferredPrompt = e;

    // Mostrar tu propio botón de instalación si lo deseas
    const installButton = document.getElementById('installButton');
    if (installButton) {
        installButton.style.display = 'block';
        installButton.addEventListener('click', () => {
            installButton.style.display = 'none';
            deferredPrompt.prompt();
            deferredPrompt.userChoice.then((choiceResult) => {
                if (choiceResult.outcome === 'accepted') {
                    console.log('✅ Usuario aceptó la instalación');
                } else {
                    console.log('❌ Usuario rechazó la instalación');
                }
                deferredPrompt = null;
            });
        });
    }
});

// Detectar cuando la app se instala
window.addEventListener('appinstalled', () => {
    console.log('🎉 ¡AprendeTEA instalada exitosamente!');
    deferredPrompt = null;
});

// Verificar si la app está siendo ejecutada como PWA
function isPWA() {
    return window.matchMedia('(display-mode: standalone)').matches ||
        window.navigator.standalone === true;
}

if (isPWA()) {
    console.log('📱 Ejecutando como PWA');
    // Agregar clases específicas si la app está instalada
    document.body.classList.add('pwa-mode');
}
