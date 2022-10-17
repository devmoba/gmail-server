function DownloadViewModel(usernames) {
    var self = this;

    self.usernames = usernames;
    self.statuses = ko.observable([]);
    self.selectedUsername = ko.observable(null);
    self.getStatusByUsername = ko.computed(() => {
        var username = self.selectedUsername();
        gmailServer.controllers.appleId.getAppleIdStatusSelection(username).done((res) => {
            self.statuses(res);
        });
    });
}