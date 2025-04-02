class LocationInformationHandler {
    constructor() {
        this.dataTable = null;

        this.tableBody = document.querySelector('#locationTableBody');

        this.btnRefresh = document.querySelector('#btnRefreshLocations');

        this.btnClear = document.querySelector('#btnClearLocations');

        this.requestVerificationToken = document.querySelector('input[name=__RequestVerificationToken]');

        this.addEventListeners();

        this.refreshLocationTable();
    }

    async getAllLocations() {
        try {
            const params = new URLSearchParams({ handler: 'AllLocations' });

            const res = await fetch(`?${params}`);

            if (!res.ok)
                throw new Error(res.status);

            return await res.json();
        }
        catch (e) {
            alertMessage('An error occurred getting locations', true);
        }
    }

    populateLocationTable(locations) {
        document
            .querySelector('#locationTable')
            .textContent = '';

        const locationArrays = [];
        let locationArray;
        let date;

        for (const location of locations) {
            locationArray = [];

            for (const key in location) {
                if (key === 'id')
                    continue;

                if (key === 'dateRecorded') {
                    date = new Date(location[key]);
                    locationArray.push(`${date.toLocaleDateString()} | ${date.toLocaleTimeString()}`);
                }
                else
                    locationArray.push(location[key]);
            }

            locationArrays.push(locationArray);
        }

        if (this.dataTable)
            this.dataTable.destroy();

        const options = {
            columns: [
                { title: 'Date Recorded' },
                { title: 'Latitude' },
                { title: 'Longitude' },
                { title: 'Confidence' }
            ],
            data: locationArrays,
            lengthMenu: [5, 10, 25, 50]
        }

        this.dataTable = new DataTable('#locationTable', options);
    }

    async refreshLocationTable() {
        const locations = await this.getAllLocations();
        this.populateLocationTable(locations);
    }

    async clearLocations() {
        if (!confirm('Are you sure you want to delete all locations?'))
            return;

        try {
            const options = {
                method: 'POST',
                headers: {
                    'RequestVerificationToken': this.requestVerificationToken.value
                }
            }

            const params = new URLSearchParams({ handler: 'DeleteLocations' });

            const res = await fetch(`?${params}`, options);

            if (!res.ok)
                throw new Error(res.status);

            alertMessage('Successfully cleared all locations');
        }
        catch (e) {
            alertMessage('An error occurred clearing locations');
        }
        finally {
            await this.refreshLocationTable();
        }
    }

    addEventListeners() {
        this.btnRefresh.addEventListener('click', () => this.refreshLocationTable());

        this.btnClear.addEventListener('click', () => this.clearLocations());
    }
}

new LocationInformationHandler();