class LocationInformationHandler {
    constructor() {
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
            this.alertMessage('An error occurred getting locations', true);
        }
    }

    populateLocationTable(locations) {
        this.tableBody.textContent = '';

        let tr;
        let td;
        let date;

        for (const location of locations) {
            tr = document.createElement('tr');
            tr.classList.add('text-light');

            td = document.createElement('td');
            date = new Date(location.dateRecorded);
            td.textContent = date.toLocaleString();

            tr.append(td);

            td = document.createElement('td');
            td.textContent = location.latitude;

            tr.append(td);

            td = document.createElement('td');
            td.textContent = location.longitude;

            tr.append(td);

            td = document.createElement('td');
            td.textContent = location.altitude;

            tr.append(td);

            td = document.createElement('td');
            td.textContent = location.confidence;

            tr.append(td);

            this.tableBody.append(tr);
        }
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

            this.alertMessage('Successfully cleared all locations');
        }
        catch (e) {
            this.alertMessage('An error occurred clearing locations');
        }
        finally {
            await this.refreshLocationTable();
        }
    }

    addEventListeners() {
        this.btnRefresh.addEventListener('click', () => this.refreshLocationTable());

        this.btnClear.addEventListener('click', () => this.clearLocations());
    }

    alertMessage(message, isError) {
        const alertMessageContainer = document.querySelector('#alertMessageContainer');

        const row = document.createElement('div');
        row.classList.add('row', 'text-light');

        const col = document.createElement('div');
        col.classList.add('col');

        const alert = document.createElement('div');
        alert.classList.add('alert', isError ? 'alert-danger' : 'alert-success');
        alert.textContent = message;

        col.append(alert);
        row.append(col);

        alertMessageContainer.append(row);

        new Promise(res => setTimeout(() => {
            alertMessageContainer.removeChild(row);
            res();
        }, 5000))
    }
}

new LocationInformationHandler();