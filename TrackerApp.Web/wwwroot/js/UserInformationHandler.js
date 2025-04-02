class UserInformationHandler {
    constructor() {
        this.dataTable = null;

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

        this.refreshUserTable();
    }

    async getAllUsers() {
        try {
            const res = await fetch('?handler=AllUsers');

            if (!res.ok)
                throw new Error(res.status);

            const json = await res.json();

            return json;
        }
        catch (e) {
            alertMessage('An error occurred getting all users', true);
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
                alertMessage('Could not identify user', true);
            else
                alertMessage('An error occurred getting user data', true);

            this.closeEditUserModal();
        }
    }

    populateUserTable(users) {
        document
            .querySelector('#userTable')
            .textContent = '';

        const userArrays = [];
        let userArray;

        for (const user of users) {
            userArray = [];

            for (const key in user) {
                if (!user[key]) {
                    userArray.push('NULL');
                    continue;
                }

                if (key == 'profileImageSrc')
                    userArray.push(`<img src="${user[key]}" class="profile-image" alt="user profile image" />`);
                else
                    userArray.push(user[key]);
            }

            userArray.push(`
                <button class="btn edit-user-button" userid="${user['userID']}" onclick="userInformationHandler.displayEditUserModal(this)">
                    <i class="bi bi-pencil-square text-light" userid="${user['userID']}"></i>
                </button>
            `);

            userArrays.push(userArray);
        }

        if (this.dataTable)
            this.dataTable.destroy();

        const options = {
            columns: [
                { title: 'User ID' },
                { title: 'Name' },
                { title: 'Email' },
                { title: 'Profile Image' },
                { title: ' ', orderable: false }
            ],
            data: userArrays,
            lengthMenu: [5, 10, 25, 50]
        }

        this.dataTable = new DataTable('#userTable', options);
    }

    async refreshUserTable() {
        const users = await this.getAllUsers();
        this.populateUserTable(users);
    }

    async displayEditUserModal(button) {
        const userID = button.getAttribute('userid');

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

            alertMessage('Successfully edited user', false);

            this.closeEditUserModal();

            await this.refreshUserTable();
        }
        catch (e) {
            if (e.message == 404)
                alertMessage('Could not identify user', true);
            else if (e.message == 400)
                alertMessage('Edited user contained invalid data', true);
            else
                alertMessage('An error occurred editing user', true);

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

            alertMessage('Successfully deleted user', false);

            await this.getAllUsers();
        }
        catch (e) {
            if (e.message == 400 || e.message == 404)
                alertMessage('Could not identify user to delete', true);
            else
                alertMessage('An error occurred deleting user', true);
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

    addEventListeners() {
        this.btnCloseEditUserModal
            .addEventListener('click', () => this.closeEditUserModal());

        this.btnSubmitEditUserForm
            .addEventListener('click', () => this.submitEditUserForm());

        this.btnDeleteUser
            .addEventListener('click', () => this.deleteUser());
    }
}

const userInformationHandler = new UserInformationHandler();