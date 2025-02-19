const map = L.map('map')
    .setView([44, -77], 5);

L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
    maxZoom: 19,
    attribution: '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>'
}).addTo(map);

let popup = L.popup();

const showPopup = event => {
    popup
        .setLatLng(event.latlng)
        .setContent(`Clicked at: ${event.latlng.lat.toFixed(4)}, ${event.latlng.lng.toFixed(4) }`)
        .openOn(map);
}

map.on('click', showPopup);

const sampleLocations = [
    [41.2583, -77.0463],
    [41.2518, -77.0458],
    [41.2448, -77.0488],
    [41.2429, -77.0446],
    [41.2419, -77.0378],
    [41.2411, -77.0342],
    [41.2376, -77.0323],
    [41.2374, -77.0277],
    [41.2376, -77.0230],
    [41.2336, -77.0226],
    [41.2335, -77.0258]
];

let polyLine = L.polyline([], { color: 'red' });
polyLine.addTo(map);

let icon = L.divIcon({ className: 'bi bi-person-standing' });

let marker = L.marker(sampleLocations[0], { icon: icon });
marker.addTo(map);

let i = 0;

const displaySampleData = () => {
    if (!sampleLocations[i]) {
        polyLine.setLatLngs([]);
        i = 0;
    }

    polyLine.addLatLng(sampleLocations[i]);

    marker.setLatLng(sampleLocations[i++]);
}

const interval = setInterval(displaySampleData, 1000);