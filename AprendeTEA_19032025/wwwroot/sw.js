// Service Worker para NeuroPro PWA
const CACHE_NAME = 'neuropro-v2';
const urlsToCache = [
    '/',
    '/Home/Index',
    '/css/neuropro-styles.css',
    '/css/site.css',
    '/js/site.js',
    '/img/logo.png',
    '/img/logo-192.png',
    '/img/logo-512.png',
    '/manifest.json'
];

// Instalación del Service Worker
self.addEventListener('install', event => {
    console.log('[Service Worker] Instalando...');
    event.waitUntil(
        caches.open(CACHE_NAME)
            .then(cache => {
                console.log('[Service Worker] Cacheando archivos');
                return cache.addAll(urlsToCache);
            })
            .catch(err => {
                console.error('[Service Worker] Error al cachear:', err);
            })
    );
    self.skipWaiting();
});

// Activación del Service Worker
self.addEventListener('activate', event => {
    console.log('[Service Worker] Activando...');
    event.waitUntil(
        caches.keys().then(cacheNames => {
            return Promise.all(
                cacheNames.map(cacheName => {
                    if (cacheName !== CACHE_NAME) {
                        console.log('[Service Worker] Eliminando cache antiguo:', cacheName);
                        return caches.delete(cacheName);
                    }
                })
            );
        })
    );
    return self.clients.claim();
});

// Estrategia: Network First, Fallback to Cache
self.addEventListener('fetch', event => {
    // Solo cachear solicitudes GET
    if (event.request.method !== 'GET') {
        return;
    }

    // Ignorar solicitudes de API que requieren autenticación
    if (event.request.url.includes('/api/') ||
        event.request.url.includes('/Account/') ||
        event.request.url.includes('/Login/')) {
        return;
    }

    event.respondWith(
        fetch(event.request)
            .then(response => {
                // Si la respuesta es válida, cachearla
                if (response && response.status === 200) {
                    const responseToCache = response.clone();
                    caches.open(CACHE_NAME)
                        .then(cache => {
                            cache.put(event.request, responseToCache);
                        });
                }
                return response;
            })
            .catch(() => {
                // Si falla la red, intentar obtener del cache
                return caches.match(event.request)
                    .then(response => {
                        if (response) {
                            return response;
                        }
                        // Si no está en cache y es navegación, devolver la página de inicio
                        if (event.request.mode === 'navigate') {
                            return caches.match('/Home/Index');
                        }
                    });
            })
    );
});

// Sincronización en segundo plano (opcional, para futuras funcionalidades)
self.addEventListener('sync', event => {
    if (event.tag === 'sync-data') {
        console.log('[Service Worker] Sincronizando datos...');
        // Aquí puedes agregar lógica de sincronización
    }
});
