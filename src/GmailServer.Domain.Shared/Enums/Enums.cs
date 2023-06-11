namespace GmailServer.Enums
{
    #region Gmail
    public enum Status
    {
        Unknown = 0,
        Good = 1,
        Disable = 2,
        Notexist = 3,
        Verify = 4,
        Checking = 5,
        Uncheck = 6,
    }

    public enum Gender
    {
        Other = 0,
        Male = 1,
        Female = 2
    }
    #endregion

    #region Check Mail
    public enum CheckerStatus
    {
        Online = 1,
        Offline = 0
    }

    public enum TaskCheckStatus
    {
        Failed = -1,
        NA = 0,
        Checking = 1,
        Done = 2
    }

    public enum TypeCheck
    {
        OwnerDB = 1,
        Browser = 2
    }
    #endregion

    #region RecoveryEmail
    public enum RecoveryEmailStatus
    {
        Ready = 0,
        Completed = 1
    }
    #endregion

    #region GmailPremium
    public enum GmailPremiumStatus
    {
        Ready = 0,
        Completed = 1,
        Error = 8,
        Unknown = 9
    }
    #endregion

    #region AppleId
    public enum AppleIdStatus
    {
        Ready = 0,
        Completed1 = 1,
        Completed2 = 10,
        Completed3 = 11,
        Completed4 = 12,
        Pending = 2,
        WrongPass = 3,
        Subed = 4,
        Locked1 = 5,
        Locked2 = 6,
        Review = 7,
        Error = 8,
        Unknown = 9
    }

    public enum AppleIdNoneStatus
    {
        Ready = 0,
        Completed1 = 1,
        Completed2 = 10,
        Completed3 = 11,
        Completed4 = 12,
        Pending = 2,
        WrongPass = 3,
        Subed = 4,
        Locked1 = 5,
        Locked2 = 6,
        Review = 7,
        Error = 8,
        Unknown = 9
    }
    #endregion

    #region GmailResource
    public enum GmailResourceStatus
    {
        Ready = 0,
        Success = 1,
        Pending = 2,
        Used = 3,
        Failed = 5,
        Error = 8,
        Unknown = 9
    }

    public enum PremiumType
    {
        Unset = 1,
        Pending = 2,
        NoPremium = 3,
        OneMonth = 4,
        ThreeMonth = 5,
        UnknowError = 6,
        WrongPassword = 7,
        Disabled = 8,
        Verify = 9
    }
    #endregion

    #region MomoAccount
    public enum MomoAccountStatus
    {
        NotUse = 0,
        InUse = 1,
        Lock = 3,
        WrongPassword = 4,
        Unknown = 5
    }
    #endregion

    #region AppleOrder
    public enum LinkStatus
    {
        Ready = 0,
        InUse = 1,
        Expired = 2,
        Error = 3,
        Linked = 4
    }

    public enum AddPaymentStatus
    {
        None = 0,
        InUse = 1,
        Expired = 2,
        Error = 3,
        Completed = 4
    }

    #endregion

    #region AppleIdNone
    public enum RemovePaymentStatus
    {
        Ready = 0,
        InUse = 1,
        NoPayment = 2,
        Completed = 3,
        WrongPassword = 4,
        Locked = 5,
        Error = 6,
        Unknown = 7
    }
    #endregion

    #region Statistic
    public enum StatisticType
    {
        Overview = 1,
        Daily = 2
    }
    #endregion
}
