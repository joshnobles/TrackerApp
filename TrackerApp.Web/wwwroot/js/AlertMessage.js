/**
 * Function appends an error alert to the alertMessageContainer element,
 * then removes the alert after five seconds.
 * 
 * @param {String}  message
 * @param {Boolean} isError
 */
function alertMessage(message, isError) {
    const alertMessageContainer = document.querySelector('#alertMessageContainer');
    alertMessageContainer.classList.add('my-2');

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
        alertMessageContainer.classList.remove('my-2');
        res();
    }, 5000))
}