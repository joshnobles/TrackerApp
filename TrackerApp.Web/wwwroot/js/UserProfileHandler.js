const profileImageContainer = document.querySelector('#profileImageContainer');
const welcomeText = document.querySelector('#welcomeText');

const topParent = document.querySelector('#topParent');

const showErrorMessage = message => {
    const row = document.createElement('div');
    row.classList.add('row', 'my-2');

    const col = document.createElement('div');
    col.classList.add('col');

    const alert = document.createElement('div');
    alert.classList.add('alert', 'alert-danger');
    alert.role = 'alert';
    alert.textContent = message;

    col.append(alert);
    row.append(col);

    topParent.prepend(row);

    return new Promise(res => {
        setTimeout(() => {
            topParent.removeChild(row);
            res();
        }, 5000)
    })
}

const displayUserInfo = user => {
    if (user.profileImageSrc) {
        const img = document.createElement('img');
        img.src = user.profileImageSrc;
        img.classList.add('img-thumbmail', 'profile-image');
        img.alt = 'user profile image';

        profileImageContainer.append(img);
    }

    welcomeText.textContent = user.name ? `Welcome To The Map ${user.name}` : 'Welcome To The Map';
}

const getProfileInfo = async () => {
    try {
        const res = await fetch('?handler=UserProfile');

        if (!res.ok) {
            if (res.redirected) {
                location.replace(res.url);
                return;
            }

            throw new Error(res.status);
        }

        const json = await res.json();

        displayUserInfo(json);
    }
    catch (e) {
        if (e.message == 404)
            showErrorMessage("User information not found");
        else
            showErrorMessage("An error occurred getting user information");
    }
}

getProfileInfo();