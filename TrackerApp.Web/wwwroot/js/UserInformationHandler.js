﻿class UserInformationHandler {
    constructor() {
        this.userTableBody = document.querySelector('#userTableBody');
        this.editUserModal = new bootstrap.Modal('#editUserModal');

        this.btnSubmitEditUserForm = document.querySelector('#btnSubmitEditUserForm');
        this.btnCloseEditUserModal = document.querySelector('#btnCloseEditUserModal');
        this.btnDeleteUser = document.querySelector('#btnDeleteUser');

        this.txtRequestVerificationToken = document.querySelector('input[name=__RequestVerificationToken]');

        this.editUserForm = {
            userID: document.querySelector('#txtUserID'),
            name: document.querySelector('#txtName'),
            email: document.querySelector('#txtEmail'),
            profileImageSrc: document.querySelector('#txtProfileImageSrc'),
        }

        this.editUserValidation = {
            'Name': document.querySelector('#nameValidation'),
            'Email': document.querySelector('#emailValidation'),
            'ProfileImageSrc': document.querySelector('#profileImageSrcValidation')
        }

        this.addEventListeners();

        this.getAllUsers();
    }

    async getAllUsers() {
        try {
            const res = await fetch('?handler=AllUsers');

            if (!res.ok)
                throw new Error(res.status);

            const json = await res.json();

            this.populateUserTable(json);
        }
        catch (e) {
            this.alertMessage('An error occurred getting all users', true);
        }
    }

    async getSpecificUser(userID) {
        try {
            const params = new URLSearchParams({
                handler: 'SpecificUser',
                userID: userID
            })

            const res = await fetch(`?${params}`);

            if (!res.ok)
                throw new Error(res.status);

            return await res.json();
        }
        catch (e) {
            if (e.message == 400 || e.message == 404)
                this.alertMessage('Could not identify user', true);
            else
                this.alertMessage('An error occurred getting user data', true);

            this.closeEditUserModal();
        }
    }

    populateUserTable(users) {
        this.userTableBody.textContent = '';

        let tr;
        let td;
        let img;
        let button;
        let icon;

        for (const user of users) {
            tr = document.createElement('tr');
            tr.classList.add('text-light');

            td = document.createElement('td');
            td.textContent = user.userID;

            tr.append(td);

            td = document.createElement('td');
            td.textContent = user.name ?? 'NULL';

            tr.append(td);

            td = document.createElement('td');
            td.textContent = user.email ?? 'NULL';

            tr.append(td);

            td = document.createElement('td');

            if (user.profileImageSrc) {
                img = document.createElement('img');
                img.classList.add('profile-image');
                img.src = user.profileImageSrc;
                img.alt = 'user profile image';

                td.append(img);
            }
            else
                td.textContent = 'NULL';

            tr.append(td);

            td = document.createElement('td');

            button = document.createElement('button');
            button.classList.add('btn', 'edit-user-button');
            button.setAttribute('userid', user.userID);

            button.addEventListener('click', event => this.displayEditUserModal(event));

            icon = document.createElement('i')
            icon.classList.add('bi', 'bi-pencil-square', 'text-light');
            icon.setAttribute('userid', user.userID);

            button.append(icon);
            td.append(button);

            tr.append(td);

            this.userTableBody.append(tr);
        }
    }

    async displayEditUserModal(event) {
        const userID = event.target.getAttribute('userid');

        const user = await this.getSpecificUser(userID);

        for (const key in user)
            this.editUserForm[key].value = user[key];

        this.editUserModal.show();
    }

    async submitEditUserForm() {
        const editedUser = {};

        for (const key in this.editUserForm)
            editedUser[key] = this.editUserForm[key].value;

        try {
            const options = {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': this.txtRequestVerificationToken.value
                },
                body: JSON.stringify(editedUser)
            }

            const params = new URLSearchParams({ handler: 'EditUser' });

            const res = await fetch(`?${params}`, options);

            if (!res.ok) {
                if (res.status == 400) {
                    const errorResults = await res.json();

                    if (!errorResults || errorResults.length < 1)
                        throw new Error(400);

                    for (const e of errorResults) {
                        if (this.editUserValidation[e.memberNames[0]])
                            this.editUserValidation[e.memberNames[0]].textContent = e.errorMessage;
                    }

                    return;
                }

                throw new Error(res.status);
            }

            this.alertMessage('Successfully edited user', false);

            this.closeEditUserModal();

            await this.getAllUsers();
        }
        catch (e) {
            if (e.message == 404)
                this.alertMessage('Could not identify user', true);
            else if (e.message == 400)
                this.alertMessage('Edited user contained invalid data', true);
            else
                this.alertMessage('An error occurred editing user', true);

            this.closeEditUserModal();
        }
    }

    async deleteUser() {
        const userID = this.editUserForm.userID.value;

        try {
            const options = {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': this.txtRequestVerificationToken.value
                },
                body: JSON.stringify(userID)
            }

            const params = new URLSearchParams({ handler: 'DeleteUser' });

            const res = await fetch(`?${params}`, options);

            if (!res.ok)
                throw new Error(res.status);

            this.alertMessage('Successfully deleted user', false);

            await this.getAllUsers();
        }
        catch (e) {
            if (e.message == 400 || e.message == 404)
                this.alertMessage('Could not identify user to delete', true);
            else
                this.alertMessage('An error occurred deleting user', true);
        }
        finally {
            this.closeEditUserModal();
        }
    }

    closeEditUserModal() {
        for (const key in this.editUserForm)
            this.editUserForm[key].value = '';

        for (const key in this.editUserValidation)
            this.editUserValidation[key].textContent = '';

        this.editUserModal.hide();
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

    addEventListeners() {
        this.btnCloseEditUserModal
            .addEventListener('click', () => this.closeEditUserModal());

        this.btnSubmitEditUserForm
            .addEventListener('click', () => this.submitEditUserForm());

        this.btnDeleteUser
            .addEventListener('click', () => this.deleteUser());
    }

}

new UserInformationHandler();