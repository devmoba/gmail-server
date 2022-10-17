function ResetStatusViewModel(statuses) {
    var self = this;
    self.statuses = statuses.filter(item => item.text !== "Ready");
}