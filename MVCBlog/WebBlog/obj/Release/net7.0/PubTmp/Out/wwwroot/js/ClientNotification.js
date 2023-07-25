"use strict";
var connection = new signalR.HubConnectionBuilder().withUrl("/notificationHub").build();
connection.start();
connection.on("ReceiveMsg", function (msg) {
    const randomId = Math.random().toString(36).substring(2, 7);
    var content = '<div id="' + randomId +'" class="toast" role="alert" aria-live="assertive" aria-atomic="true" data-bs-autohide="false"> '+
        '<div class="toast-header"> '+
                '<img src="https://icons.getbootstrap.com/assets/icons/bootstrap.svg" class="rounded me-2" alt="..."> '+
               ' <strong class="me-auto">Web Blog</strong> '+
               ' <small>just now</small> '+
               ' <button type="button" class="btn-close" data-bs-dismiss="toast" aria-label="Close"></button> '+
        '</div> '+
       ' <div class="toast-body" > '+ msg +
      '      </div > '+
       ' </div > ';
    $(".toast-container").append(content);
    bootstrap.Toast.getOrCreateInstance("#" + randomId).show();
});