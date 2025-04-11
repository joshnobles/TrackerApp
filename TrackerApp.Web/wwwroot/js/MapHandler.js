class MapHandler {
    constructor() {
        this.chkPolyline = document.querySelector('#chkPolyline');

        this.txtRequestVerificationToken = document.querySelector('input[name=__RequestVerificationToken]');

        this.getTileLayers()
            .then(tileLayers => {
                this.map = L.map('map', {
                    center: [40.7128, -74.0060],
                    zoom: 8,
                    layers: [tileLayers[Object.keys(tileLayers)[0]]]
                });

                L.control
                    .layers(tileLayers, {})
                    .addTo(this.map);

                document
                    .querySelector('.leaflet-control-layers-list')
                    .classList
                    .add('text-start');
            });

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
        if (!this.currentLocation || !this.map)
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
                color: 'cyan',
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

    async getTileLayers() {
        const apiKey = await this.getThunderForestApiKey();

        const tileLayers = {
            'Open Street Map': L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
                maxZoom: 19,
                attribution: '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>'
            }),
            'Smooth Dark': L.tileLayer('https://tiles.stadiamaps.com/tiles/alidade_smooth_dark/{z}/{x}/{y}{r}.{ext}', {
                minZoom: 0,
                maxZoom: 20,
                attribution: '&copy; <a href="https://www.stadiamaps.com/" target="_blank">Stadia Maps</a> &copy; <a href="https://openmaptiles.org/" target="_blank">OpenMapTiles</a> &copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors',
                ext: 'png'
            }),
            'Water Color': L.tileLayer('https://tiles.stadiamaps.com/tiles/stamen_watercolor/{z}/{x}/{y}.{ext}', {
                minZoom: 1,
                maxZoom: 16,
                attribution: '&copy; <a href="https://www.stadiamaps.com/" target="_blank">Stadia Maps</a> &copy; <a href="https://www.stamen.com/" target="_blank">Stamen Design</a> &copy; <a href="https://openmaptiles.org/" target="_blank">OpenMapTiles</a> &copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors',
                ext: 'jpg'
            }),
            'Satellite': L.tileLayer('https://basemap.nationalmap.gov/arcgis/rest/services/USGSImageryTopo/MapServer/tile/{z}/{y}/{x}', {
                maxZoom: 16,
                attribution: 'Tiles courtesy of the <a href="https://usgs.gov/">U.S. Geological Survey</a>'
            })
        }

        if (apiKey) {
            tileLayers['Hell'] = L.tileLayer(`https://{s}.tile.thunderforest.com/spinal-map/{z}/{x}/{y}{r}.png?apikey=${apiKey}`, {
                attribution: '&copy; <a href="http://www.thunderforest.com/">Thunderforest</a>, &copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors',
                apikey: apiKey,
                maxZoom: 22
            });

            tileLayers['Western'] = L.tileLayer(`https://{s}.tile.thunderforest.com/pioneer/{z}/{x}/{y}{r}.png?apikey=${apiKey}`, {
                attribution: '&copy; <a href="http://www.thunderforest.com/">Thunderforest</a>, &copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors',
                apikey: apiKey,
                maxZoom: 22
            });
        }

        return tileLayers;
    }

    async getThunderForestApiKey() {
        const options = {
            method: 'GET',
            headers: {
                'RequestVerificationToken': this.txtRequestVerificationToken.value
            }
        }

        const params = new URLSearchParams({ handler: 'ThunderForestApiKey' });

        try {
            const response = await fetch(`?${params}`, options);

            if (!response.ok)
                throw new Error(response.status);

            return await response.json();
        }
        catch (e) {
            return null;
        }
    }

}

new MapHandler();