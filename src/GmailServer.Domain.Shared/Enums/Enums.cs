namespace GmailServer.Enums
{
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
    
    public enum RecoveryEmailStatus
    {
        Ready = 0,
        Completed = 1
    }

    public enum GmailPremiumStatus
    {
        Ready = 0,
        Completed = 1,
        Error = 8,
        Unknown = 9
    }

    public enum AppleIdStatus
    {
        Ready = 0,
        Completed1 = 1,
        Completed2 = 10,
        Pending = 2,
        WrongPass = 3,
        Subed = 4,
        Locked1 = 5,
        Locked2 = 6,
        Review = 7,
        Error = 8,
        Unknown = 9
    }

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
}
