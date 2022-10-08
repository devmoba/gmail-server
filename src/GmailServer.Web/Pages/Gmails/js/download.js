$(function () {
    var viewModel = new DownloadFormViewModel(gmailTypeSelections);
    ko.applyBindings(viewModel);
});