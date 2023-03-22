$(function () {
    $("#alertMessage").hide();

    var connection = new signalR.HubConnectionBuilder()
        .withUrl("/signalr-hubs/reup-gmailresource")
        .withAutomaticReconnect([0, 5000, 10000, 30000])
        .build();
    
    connection.on("ReceiveNotiAsync", (message, type) => {
        var html = ``;
        html += `<div class="alert alert-${type} alert-dismissible fade show" role="alert">`;
        html += `<strong>${message}</strong>`;
        html += `<button type="button" class="close" data-dismiss="alert" aria-label="Close">`;
        html += `<span aria-hidden="true">&times;</span>`;
        html += `</button>`;
        html += `</div>`;
        $("#alertMessage").append(html);
        $("#alertMessage").fadeTo(5000, 1000).slideUp(1000, function () {
            $("#alertMessage").slideUp(1000);
        });
        $('#reupGmailResourceForm')[0].reset();
    });

    connection.start().then(function () {
        console.log("SignalR Started.");
    }).catch(function (err) {
    });

});