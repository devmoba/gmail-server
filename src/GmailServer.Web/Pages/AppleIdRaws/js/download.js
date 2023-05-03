function DownloadFormViewModel(gmailTypeSelections) {
    this.checkedAll = ko.observable(false);
    this.checkedTimeRange = ko.observable(false);
}

$(function () {
    var viewModel = new DownloadFormViewModel();
    ko.applyBindings(viewModel);
});