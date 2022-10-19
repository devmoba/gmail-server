function ResetStatusViewModel(statuses) {
    var self = this;
    self.targetStatuses = statuses;
    self.statuses = statuses.filter(item => item.text !== "Ready");
}