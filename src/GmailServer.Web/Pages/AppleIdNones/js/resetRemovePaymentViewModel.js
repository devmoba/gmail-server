function ResetRemovePaymentStatusViewModel(statuses, usernames) {
    var self = this;
    self.usernames = usernames;
    self.targetStatuses = statuses;
    self.statuses = ko.observable([]);
    self.createdFrom = ko.observable(null);
    self.createdTo = ko.observable(null);
    self.removeTakenTimeTo = ko.observable(null);
    self.removeTakenTimeFrom = ko.observable(null);
    self.checkedOnDelete = ko.observable(false);
    self.selectedUsername = ko.observable(null);
    self.getStatusByConditions = ko.computed(() => {
        var username = self.selectedUsername();
        var createdFrom = self.createdFrom();
        var createdTo = self.createdTo();
        var removeTakenTimeFrom = self.removeTakenTimeFrom();
        var removeTakenTimeTo = self.removeTakenTimeTo();
        gmailServer.controllers.appleIdNone.getAppleIdNoneRemoveStatusSelections(username, createdFrom, createdTo, removeTakenTimeFrom, removeTakenTimeTo).done((res) => {
            self.statuses(res);
        });
    }).extend({ notify: 'always', rateLimit: 700 });
}