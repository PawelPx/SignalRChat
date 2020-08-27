"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

var userInput = document.getElementById("userInput");
var messageInput = document.getElementById("messageInput");
var sendButton = document.getElementById("sendButton");
var messagesList = document.getElementById("messagesList");
var receiverInput = document.getElementById("receiverInput");
var sendDirectMessageButton = document.getElementById("sendDirectMessageButton");
var usersList = document.getElementById("usersList");

var colsNumber = messagesList.cols;
var interval = "";

for (var i = 0; i < colsNumber + 20; i++) {
    interval += " ";
}

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message, time) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = user + " says: " + msg + '\r\n' + interval + time + '\r\n';
    messagesList.value += encodedMsg;
});

connection.start().then(function () {
    connection.invoke("AppendToUserList").catch(function (err) {
        return console.error(err.toString());
    });
    sendButton.disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

sendButton.addEventListener("click", function (event) {
    var user = userInput.value;
    var message = messageInput.value;
    connection.invoke("SendMessage", user, message).catch(function (err) {
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


sendDirectMessageButton.addEventListener("click", function (event) {
    var user = userInput.value;
    var message = messageInput.value;
    var connectionId = receiverInput.value;
    connection.invoke("SendDirectMessage", user, message, connectionId).catch(function (err) {
        return console.error(err.toString());
    });
    messageInput.value = "";
    messagesList.scrollTop = messagesList.scrollHeight;
    event.preventDefault();
});


connection.on("AppendToUserList", function (connectionId) {
    usersList.value += connectionId + '\r\n';
});