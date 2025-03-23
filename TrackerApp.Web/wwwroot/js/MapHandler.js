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

        const iconHtml = `
            <div class="w-100 h-100">
                <svg xmlns="http://www.w3.org/2000/svg" width="30" height="30" fill="blue" class="bi bi-geo-alt-fill" viewBox="0 0 16 16">
                    <path d="M8 16s6-5.686 6-10A6 6 0 0 0 2 6c0 4.314 6 10 6 10m0-7a3 3 0 1 1 0-6 3 3 0 0 1 0 6"/>
                </svg>
            </div>
        `;

        this.icon = L.divIcon({ className: 'map-icon', html: iconHtml, iconSize: L.point(30, 30) });

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
                color: 'red',
                fillColor: 'red',
                fillOpacity: 0.25
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
                color: 'black',
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