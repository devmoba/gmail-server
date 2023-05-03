function ResetStatusViewModel(statuses, usernames) {
    var self = this;
    self.usernames = usernames;
    self.targetStatuses = statuses;
    self.statuses = ko.observable([]);
    self.createdFrom = ko.observable(null);
    self.createdTo = ko.observable(null);
    self.checkedOnDelete = ko.observable(false);
    self.selectedUsername = ko.observable(null);
    self.getStatusByConditions = ko.computed(() => {
        var username = self.selectedUsername();
        var createdFrom = self.createdFrom();
        var createdTo = self.createdTo();
        gmailServer.controllers.appleIdNone.getAppleIdNoneStatusSelections(username, createdFrom, createdTo).done((res) => {
            self.statuses(res);
        });
    }).extend({ notify: 'always', rateLimit: 700 });
}