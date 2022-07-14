const Status = {
    Good: 1,
    Verify: 4,
    Unknown: 0
};

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

    connection.on("ReceiveEmailResultAsync", (res) => {
        var emailChunkResults = viewModel.emailChunkResults();
        emailChunkResults.push(...res);
        viewModel.emailChunkResults(emailChunkResults);
    });

    //connection.on("ClearResultAsync", () => {
    //    ClearResult();
    //});

    //connection.on("ReceiveCountResultAsync", (res) => {
    //    var count = viewModel.countResult();
    //    count += res;
    //    viewModel.countResult(count);
    //});

    //connection.on("ReceiveTotalCheckAsync", (res) => {
    //    viewModel.totalEmail(res);
    //    $("#ckeck-now").prop('disabled', true);
    //});

    //connection.on("ReceiveNotiAsync", (message, type) => {
    //    alert(message);
    //});

    //connection.on("ReceiveEmailResultOutputAsync", (res) => {
    //    var output = editorOutput.getValue();
    //    if (output) {
    //        output = `${output}\n${res}`;
    //        editorOutput.setValue(output);
    //    } else {
    //        editorOutput.setValue(res);
    //    }
    //    $("#ckeck-now").prop('disabled', false);
    //});

    connection.on("ReceiveEmailResultGroupAsync", (emailResultOuput, status, count) => {
        switch (status) {
            case Status.Good:
                var good = viewModel.emailResultGood();
                good.count += count;
                good.emailResultOuput = good.emailResultOuput
                    ? `${good.emailResultOuput}\n${emailResultOuput}`
                    : emailResultOuput;
                viewModel.emailResultGood(good);
                break;
            case Status.Verify:
                var verify = viewModel.emailResultVerify();
                verify.count += count;
                verify.emailResultOuput = verify.emailResultOuput
                    ? `${verify.emailResultOuput}\n${emailResultOuput}`
                    : emailResultOuput;
                viewModel.emailResultVerify(verify);
                break;
            default:
                var unknown = viewModel.emailResultUnknown();
                unknown.count += count;
                unknown.emailResultOuput = unknown.emailResultOuput
                    ? `${unknown.emailResultOuput}\n${emailResultOuput}`
                    : emailResultOuput;
                viewModel.emailResultUnknown(unknown);
                break;
        }
    });

    connection.start().then(function () {
        console.log("SignalR Started.");
    }).catch(function (err) {
    });


    $("#checkMailForm").submit(function (e) {
        e.preventDefault();
        var emailInput = editorInput.getValue();
        if (!emailInput) {
            alert("Mail input null!")
            return;
        }

        $("#ckeck-now").prop('disabled', true);
        ClearResult();

        var emails = emailInput.split('\n');
        viewModel.totalEmail(emails.length);

        var emailChecks = emails.map((element, index) => ({ id: index, email: element }));
        var emailChunks = SplitArray(emailChecks, emailLimitRequest);
        emailChunks.forEach(async function (emailChunk, index) {
            await connection.invoke("InputEmailCheckAsync", emailChunk);

            while (true) {
                await sleep(500);
                var emailChunkResults = viewModel.emailChunkResults();
                if (emailChunkResults.length == emailChunk.length) {
                   
                    emailChunkResults.sort(function (a, b) {
                        return a.id - b.id;
                    });
                    var emailOutput = editorOutput.getValue();
                    var emailJoin = emailChunkResults
                        .map((item) => `${item.email}|${item.status}`)
                        .join('\n');
                    if (emailOutput) {
                        emailOutput += `\n${emailJoin}`;
                    } else {
                        emailOutput = emailJoin;
                    }
                    editorOutput.setValue(emailOutput);
                    viewModel.emailChunkResults([]);
                    break;
                }
            }
        });
        $("#ckeck-now").prop('disabled', false);
    });

    $("#clear-input").on("click", function (e) {
        e.preventDefault();
        editorInput.setValue("");
        viewModel.countResult(0);
        viewModel.totalEmail(0);
    });

    $("#clear-output").on("click", function (e) {
        e.preventDefault();
        ClearResult();
    });

    function ClearResult() {
        viewModel.countResult(0);
        viewModel.totalEmail(0);
        viewModel.emailResultGood(new EmailResultGroup(``, 'Good', 0));
        viewModel.emailResultVerify(new EmailResultGroup(``, 'Verify', 0));
        viewModel.emailResultUnknown(new EmailResultGroup(``, 'Unknown', 0));
        editorOutput.setValue('');
    }
});

function EmailCheck(id, email) {
    this.id = id;
    this.email = email;
}

function CheckMailViewModel() {
    this.totalEmail = ko.observable(0);
    this.countResult = ko.observable(0);
    this.emailChunkResults = ko.observable([]);
    this.emailResultGood = ko.observable(new EmailResultGroup(``, 'Good', 0));
    this.emailResultVerify = ko.observable(new EmailResultGroup(``, 'Verify', 0));
    this.emailResultUnknown = ko.observable(new EmailResultGroup(``, 'Unknown', 0));

    this.downloadResult = function (res) {
        var element = document.createElement('a');
        var filename = `txt_checkmail_${res.status}`;
        element.setAttribute('href', 'data:text/plain;charset=utf-8,' + encodeURIComponent(res.emailResultOuput));
        element.setAttribute('download', filename);
        element.style.display = 'none';
        document.body.appendChild(element);
        element.click();
        document.body.removeChild(element);
    }
}

function EmailResultGroup(emailResultOutput, status, count) {
    this.emailResultOuput = emailResultOutput;
    this.status = status;
    this.count = count;
}

function SplitArray(inputArray, perChunk) {
    return inputArray.reduce((resultArray, item, index) => {
        const chunkIndex = Math.floor(index / perChunk);
        if (!resultArray[chunkIndex]) {
            resultArray[chunkIndex] = []; // start a new chunk
        }
        resultArray[chunkIndex].push(item);
        return resultArray;
    }, []);
}