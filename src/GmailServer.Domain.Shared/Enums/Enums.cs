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
}
