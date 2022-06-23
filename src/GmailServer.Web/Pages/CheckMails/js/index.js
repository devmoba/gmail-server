$(function () {
    var mailInputElement = document.getElementById("mail-input");
    var editorInput = CodeMirror.fromTextArea(mailInputElement, {
        lineNumbers: true,
        extraKeys: {
            "Ctrl-Space": "autocomplete"
        }
    });

    var mailOutputElement = document.getElementById("mail-ouput");
    var editorOutput = CodeMirror.fromTextArea(mailOutputElement, {
        lineNumbers: true,
        extraKeys: {
            "Ctrl-Space": "autocomplete"
        }
    });

    var viewModel = new CheckMailViewModel();
    ko.applyBindings(viewModel);

    var connection = new signalR.HubConnectionBuilder()
        .withUrl("/signalr-hubs/check-mail")
        .withAutomaticReconnect([0, 5000, 10000, 30000])
        .build();

    connection.on("ReceiveCountResultAsync", (res) => {
        console.log(res);
        viewModel.countResult(res);
    });

    connection.on("ReceiveEmailResultAsync", (res) => {
        editorOutput.setValue(`${res.emailResults.join('\n').toString()}`);
        viewModel.EmailResultGroup(res.emailResultGroups);
    });

    connection.start().then(function () {
        console.log("SignalR Started.");
    }).catch(function (err) {
        return console.error(err.toString());
    });

    $("#ckeck-now").on("click", function (e) {
        e.preventDefault();
        var mailInput = editorInput.getValue();
        var emails = mailInput.split('\n').filter(item => {
            return item ? true : false;
        });

        var emailChecks = emails.map(function (email, index) {
            var emailCheck = new EmailCheck(index, email);
            return emailCheck;
        });
        
        connection.invoke("GetCheckMailResultAsync", emailChecks).then(function () {
           
        }).catch(function (err) {
            return console.error(err.toString());
        });
        viewModel.totalEmail(emailChecks.length);
    });

    $("#clear-input").on("click", function (e) {
        e.preventDefault();
        editorInput.setValue("");
    });

    $("#clear-output").on("click", function (e) {
        e.preventDefault();
        editorOutput.setValue("");
    });
});

function EmailCheck(id, email) {
    this.id = id;
    this.email = email;
}

function CheckMailViewModel() {
    this.totalEmail = ko.observable(0);
    this.countResult = ko.observable(0);
    this.EmailResultGroup = ko.observable([]);
    this.downloadResult = function (res) {
        var joinResult = res.emailResults.join('\n').toString();
        var element = document.createElement('a');
        var filename = `txt_checkmail_${res.status}`;
        element.setAttribute('href', 'data:text/plain;charset=utf-8,' + encodeURIComponent(joinResult));
        element.setAttribute('download', filename);
        element.style.display = 'none';
        document.body.appendChild(element);
        element.click();
        document.body.removeChild(element);
    }
}