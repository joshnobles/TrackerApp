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