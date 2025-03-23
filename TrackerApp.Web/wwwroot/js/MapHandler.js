class MapHandler {
    constructor() {
        this.chkPolyline = document.querySelector('#chkPolyline');

        this.map = L.map('map')
            .setView([40.7128, -74.0060], 8);

        L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
            maxZoom: 19,
            attribution: '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>'
        }).addTo(this.map);

        this.marker = null;
        this.confidenceCircle = null;
        this.polyline = null;
        this.icon = L.divIcon({ className: 'bi bi-person-standing' });

        this.currentLocation = null;

        this.interval = setInterval(() => this.refreshAndDisplayLocation(), 1000);
    }

    async refreshCurrentLocation() {
        try {
            const params = new URLSearchParams({ handler: 'NextLocation' });

            const res = await fetch(`?${params}`);

            if (!res.ok)
                throw new Error(res.status);

            const location = await res.json();

            if (!this.currentLocation || this.currentLocation.id != location.id) {
                this.currentLocation = location;
                return true;
            }

            return false;
        }
        catch (e) {
            if (e.message == 404)
                this.currentLocation = null;
            else
                console.log(e);
        }
    }

    displayCurrentLocation() {
        if (!this.currentLocation)
            return;

        const latLng = [this.currentLocation.latitude, this.currentLocation.longitude];

        if (!this.marker) {
            this.marker = L.marker(latLng, { icon: this.icon })
            this.marker.addTo(this.map);
        }
        else
            this.marker.setLatLng(latLng);

        if (!this.confidenceCircle) {
            this.confidenceCircle = L.circle(latLng, {
                radius: this.currentLocation.confidence,
                color: 'blue',
                fillColor: '#blue',
                fillOpacity: 0.5
            });
            this.confidenceCircle.addTo(this.map);
        }
        else {
            this.confidenceCircle.setLatLng(latLng);
            this.confidenceCircle.setRadius(this.currentLocation.confidence);
        }

        if (!this.chkPolyline.checked) {
            if (this.polyline)
                this.map.removeLayer(this.polyline);

            this.polyline = null;
            return;
        }

        if (!this.polyline) {
            this.polyline = L.polyline([latLng], {
                color: 'blue',
                weight: 4,
                opacity: 1,
                smoothFactor: 1
            });

            this.polyline.addTo(this.map);
        }
        else
            this.polyline.addLatLng(latLng);
    }

    async refreshAndDisplayLocation() {
        const isNewLocation = await this.refreshCurrentLocation();

        if (isNewLocation)
            this.displayCurrentLocation();
    }

}

new MapHandler();