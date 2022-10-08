function DownloadFormViewModel(gmailTypeSelections) {
    this.checkedAll = ko.observable(false);
    this.checkedTimeRange = ko.observable(false);
    this.checkedGmailType = ko.observable(false);
    this.gmailTypeSelections = gmailTypeSelections;
}