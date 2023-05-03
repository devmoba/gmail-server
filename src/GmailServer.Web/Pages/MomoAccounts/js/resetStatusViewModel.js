function ResetStatusViewModel(statuses, uploadGroups) {
    var self = this;
    self.uploadGroups = uploadGroups;
    self.targetStatuses = statuses;
    self.statuses = ko.observable([]);
    self.createdTimeFrom = ko.observable(null);
    self.createdTimeTo = ko.observable(null);
    self.checkedOnDelete = ko.observable(false);
    self.selectedUploadGroup = ko.observable(null);
    self.getStatusByConditions = ko.computed(() => {
        var uploadGroup = self.selectedUploadGroup();
        var createdTimeFrom = self.createdTimeFrom();
        var createdTimeTo = self.createdTimeTo();
        gmailServer.controllers.momoAccount.getMomoAcountStatusSelections(uploadGroup, createdTimeFrom, createdTimeTo).done((res) => {
            self.statuses(res);
        });
    }).extend({ notify: 'always', rateLimit: 700 });
}