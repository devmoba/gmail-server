function DownloadViewModel() {
    var self = this;
    self.statuses = ko.observable([]);
    self.createdFrom = ko.observable(null);
    self.createdTo = ko.observable(null);
    self.getStatusByConditions = ko.computed(() => {
        var createdFrom = self.createdFrom();
        var createdTo = self.createdTo();

        gmailServer.controllers.gmail.getGmailStatusSelection(createdFrom, createdTo).done((res) => {
            self.statuses(res);
        });
    }).extend({ notify: 'always', rateLimit: 700 });
}