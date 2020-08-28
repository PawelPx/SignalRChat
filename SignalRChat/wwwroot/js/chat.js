"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

var userInput = document.getElementById("userInput");
var messageInput = document.getElementById("messageInput");
var setUserButton = document.getElementById("setUserButton");
var sendButton = document.getElementById("sendButton");
var messagesList = document.getElementById("messagesList");
var usersList = document.getElementById("usersList");

// interval for time of sending message
var colsNumber = messagesList.cols;
var interval = "";

for (var i = 0; i < colsNumber + 20; i++) {
    interval += " ";
}

// Disable send button until username is established
sendButton.disabled = true;
messageInput.disabled = true;

// On connecting to hub; currently empty
connection.start().then(function () {
}).catch(function (err) {
    return console.error(err.toString());
});

// On receiving message it's being append to the appropriate chat
connection.on("ReceiveMessage", function (user, message, time, isDirect, receiver) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = user + " says: " + msg + '\r\n' + interval + time + '\r\n';
    if (isDirect) {
        // Usernames are used in Id of private chats, so distinction depending on how sent message
        if (user == userInput.value) {
            document.getElementById("directMessagesList" + receiver).value += encodedMsg;
        } else {
            document.getElementById("directMessagesList" + user).value += encodedMsg;
        }
    } else {
        messagesList.value += encodedMsg;
    }
});

// Sending message to all
sendButton.addEventListener("click", function (event) {
    var user = userInput.value;
    var message = messageInput.value;
    var receiver = null;
    connection.invoke("SendMessage", user, message, receiver).catch(function (err) {
        return console.error(err.toString());
    });
    messageInput.value = "";
    messagesList.scrollTop = messagesList.scrollHeight;
    event.preventDefault();
});

// Listener for enter key
messageInput.addEventListener("keyup", function (event) {
    if (event.keyCode === 13) {
        sendButton.click();
    }
});

// Appends user to the users' list;
connection.on("AppendToUserList", function (user) {
    var li = document.createElement("li");
    var a = document.createElement("a");
    var link = document.createTextNode(user);
    a.appendChild(link);
    a.href = "#";

    // Clicking on the users on list opens a private chat with them
    a.addEventListener('click', function () {
        connection.invoke("CreateChatToOtherUser", user, userInput.value).catch(function (err) {
            return console.error(err.toString());
        });
        var label = document.createElement("label");
        label.innerHTML = user;
        document.getElementById("div1").appendChild(label);
        StartDirectChat(user);
    });
    li.appendChild(a);
    usersList.appendChild(li);
});

// Sets your username and adds you to users' list
setUserButton.addEventListener("click", function (event) {
    var user = userInput.value;
    sendButton.disabled = false;
    messageInput.disabled = false;
    setUserButton.disabled = true;
    connection.invoke("Join", user).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

// Creates a private chat on other user's request
connection.on("StartDirectChat", function (user) {
    StartDirectChatLabel(user);
    StartDirectChat(user);
});

// Creates label with username of person how started private chat
function StartDirectChatLabel(receiver) {
    var label = document.createElement("label");
    label.innerHTML = receiver;
    document.getElementById("div1").appendChild(label);
}

// Creates a private chat
function StartDirectChat(receiver) {
    var br = document.createElement("br");
    document.getElementById("div1").appendChild(br);

    var textarea = document.createElement("textarea");
    textarea.id = "directMessagesList" + receiver;
    textarea.rows = "20";
    textarea.cols = "80";
    document.getElementById("div1").appendChild(textarea);

    var input = document.createElement("input");
    input.id = "messagesInput" + receiver;
    input.type = "text";
    input.size = "77";
    input.addEventListener("keyup", function (event) {
        if (event.keyCode === 13) {
            document.getElementById("sendDirectMessage" + receiver).click();
        }
    });
    document.getElementById("div1").appendChild(input);

    var button = document.createElement("button");
    button.id = "sendDirectMessage" + receiver;
    button.value = "Send Message";
    button.addEventListener("click", function (event) {
        var user = userInput.value;
        var message = document.getElementById("messagesInput" + receiver).value;
        connection.invoke("SendMessage", user, message, receiver).catch(function (err) {
            return console.error(err.toString());
        });
        document.getElementById("messagesInput" + receiver).value = "";
        document.getElementById("messagesList" + receiver).scrollTop = document.getElementById("messagesList" + receiver).scrollHeight;
        event.preventDefault();
    });
    document.getElementById("div1").appendChild(button);
}

