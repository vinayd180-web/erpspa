// Service Worker for Shree Parth Academy PWA
const CACHE_NAME = 'spa-v1';

self.addEventListener('install', function(e) {
    e.waitUntil(
        caches.open(CACHE_NAME).then(function(cache) {
            return cache.addAll([
                '/',
                '/css/site.css',
                '/js/site.js',
                '/img/icon-512x512.png',
                '/img/icon-192x192.png'
            ]);
        })
    );
});

self.addEventListener('fetch', function(e) {
    e.respondWith(
        caches.match(e.request).then(function(response) {
            return response || fetch(e.request);
        })
    );
});
