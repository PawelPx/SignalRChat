const uri = 'api/Messages';
let messages = [];
var temp = 0;

function getItems() {
    fetch(uri)
        .then(response => response.json())
        .then(data => _displayItems(data))
        .catch(error => console.error('Unable to get items.', error));
}

function addItem() {
    const addUserTextBox = document.getElementById('add-user');
    const addTextTextBox = document.getElementById('add-text');

    const item = {
        user: addUserTextBox.value.trim(),
        text: addTextTextBox.value.trim()
    };

    fetch(uri, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
        .then(response => response.json())
        .then(() => {
            getItems();
            addUserTextBox.value = '';
            addTextTextBox.value = '';
        })
        .catch(error => console.error('Unable to add item.', error));
}

function deleteItem(id) {
    fetch(`${uri}/${id}`, {
        method: 'DELETE'
    })
        .then(() => getItems())
        .catch(error => console.error('Unable to delete item.', error));
}

function displayEditForm(id) {
    const item = messages.find(item => item.id === id);

    temp = item.sendDate;

    document.getElementById('edit-id').value = item.id;
    document.getElementById('edit-user').value = item.user;
    document.getElementById('edit-text').value = item.text;
    document.getElementById('editForm').style.display = 'block';
}

function updateItem() {
    const itemId = document.getElementById('edit-id').value;
    const item = {
        id: parseInt(itemId, 10),
        user: document.getElementById('edit-user').value.trim(),
        text: document.getElementById('edit-text').value.trim(),
        sendDate: temp
    };

    fetch(`${uri}/${itemId}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
        .then(() => getItems())
        .catch(error => console.error('Unable to update item.', error));

    closeInput();

    return false;
}

function closeInput() {
    document.getElementById('editForm').style.display = 'none';
}

function _displayCount(itemCount) {
    const user = (itemCount === 1) ? 'message' : 'messages';

    document.getElementById('counter').innerText = `${itemCount} ${user}`;
}

function _displayItems(data) {
    const tBody = document.getElementById('messages');
    tBody.innerHTML = '';

    _displayCount(data.length);

    const button = document.createElement('button');

    data.forEach(item => {
        let textTextBox = document.createElement('input');
        textTextBox.type = 'text';
        textTextBox.disabled = true;
        textTextBox.value = item.text;

        let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `displayEditForm(${item.id})`);

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteItem(${item.id})`);

        let tr = tBody.insertRow();

        let td1 = tr.insertCell(0);
        let textNode = document.createTextNode(item.user);
        td1.appendChild(textNode);

        //let td2 = tr.insertCell(1);
        //let textNode2 = document.createTextNode(item.text);
        //td2.appendChild(textNode2);

        //let td3 = tr.insertCell(2);
        //td3.appendChild(editButton);

        //let td4 = tr.insertCell(3);
        //td4.appendChild(deleteButton);

        let td2 = tr.insertCell(1);
        let textNode2 = document.createTextNode(item.text);
        td2.appendChild(textNode2);

        let td3 = tr.insertCell(2);
        var date = item.sendDate;
        date = date.slice(0, 4) + '/' +
            date.slice(5, 7) + '/' +
            date.slice(8, 10) + ' ' +
            date.slice(11, 19);
        let textNode3 = document.createTextNode(date);
        td3.appendChild(textNode3);

        let td4 = tr.insertCell(3);
        td4.appendChild(editButton);

        let td5 = tr.insertCell(4);
        td5.appendChild(deleteButton);
    });

    messages = data;
}