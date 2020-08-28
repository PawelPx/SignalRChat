"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

var userInput = document.getElementById("userInput");
var setUserButton = document.getElementById("setUserButton");
var messageInput = document.getElementById("messageInput");
var sendButton = document.getElementById("sendButton");
var messagesList = document.getElementById("messagesList");
var receiverInput = document.getElementById("receiverInput");
var sendDirectMessageButton = document.getElementById("sendDirectMessageButton");
var usersList = document.getElementById("usersList");
var directMessagesList = document.getElementById("directMessagesList");

var colsNumber = messagesList.cols;
var interval = "";

for (var i = 0; i < colsNumber + 20; i++) {
    interval += " ";
}

//Disable send button until connection is established
sendButton.disabled = true;
messageInput.disabled = true;
sendDirectMessageButton.disabled = true;

connection.on("ReceiveMessage", function (user, message, time, isDirect) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = user + " says: " + msg + '\r\n' + interval + time + '\r\n';
    if (isDirect) {
        directMessagesList.value += encodedMsg;
    } else {
        messagesList.value += encodedMsg;
    }
});

connection.start().then(function () {
    //connection.invoke("AppendToUserList").catch(function (err) {
    //    return console.error(err.toString());
    //});
    //sendButton.disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

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

sendDirectMessageButton.addEventListener("click", function (event) {
    var user = userInput.value;
    var message = messageInput.value;
    var receiver = receiverInput.value;
    connection.invoke("SendMessage", user, message, receiver).catch(function (err) {
        return console.error(err.toString());
    });
    messageInput.value = "";
    messagesList.scrollTop = messagesList.scrollHeight;
    event.preventDefault();
});


messageInput.addEventListener("keyup", function (event) {
    if (event.keyCode === 13) {
        sendButton.click();
    }
});


connection.on("AppendToUserList", function (user) {
    usersList.value += user + '\r\n';
});


setUserButton.addEventListener("click", function (event) {
    var user = userInput.value;
    sendButton.disabled = false;
    messageInput.disabled = false;
    sendDirectMessageButton.disabled = false;
    connection.invoke("Join", user).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});