$(function () {
    var elementResult = $("#result");
    var node = new PrettyJSON.view.Node({
        el: elementResult,
        data: fakeSettings
    });

    var data = "data:text/json;charset=utf-8," + encodeURIComponent(JSON.stringify(fakeSettings));
    var downloadElement = document.getElementById('downloadJson');
    downloadElement.setAttribute("href", data);
    downloadElement.setAttribute("download", "fake-setting.json");
});