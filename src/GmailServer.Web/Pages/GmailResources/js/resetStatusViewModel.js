function ResetStatusViewModel(statuses, usernames) {
    var self = this;
    self.usernames = usernames;
    self.targetStatuses = statuses;
    //self.statuses = statuses.filter(item => item.text !== "Ready");
    self.statuses = ko.observable([]);
    self.createdFrom = ko.observable(null);
    self.createdTo = ko.observable(null);
    self.updatedHours = ko.observable(null).extend({ notify: 'always', rateLimit: 500 });
    self.checkedOnDelete = ko.observable(false);
    self.selectedUsername = ko.observable(null);
    self.getStatusByConditions = ko.computed(() => {
        var username = self.selectedUsername();
        var createdFrom = self.createdFrom();
        var createdTo = self.createdTo();
        var updatedHours = self.updatedHours();
        console.log(updatedHours);
        //console.log(createdTo);
        gmailServer.controllers.gmailResource.getGmailResourceStatusSelection(username, createdFrom, createdTo, updatedHours).done((res) => {
            self.statuses(res);
            //console.log(res);
        });
    }).extend({ notify: 'always', rateLimit: 700 });
}