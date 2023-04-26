using GmailServer.Enums;

namespace GmailServer.MomoAccounts
{
    public class UpdateMomoAccountInputDto
    {
        public string Password { get; set; }

        public string Email { get; set; }

        public MomoAccountStatus Status { get; set; }

        public string UDid1 { get; set; }

        public string UDid2 { get; set; }

        public string RefreshToken { get; set; }

        public string AuthenticateToken { get; set; }

        public string SessionKey { get; set; }

        public string SessionKey2 { get; set; }

        public string SetupKey { get; set; }

        public int CurrentLinkCount { get; set; }

        public int TotalLinkCount { get; set; }

        public string CustmArg1 { get; set; }

        public string CustmArg2 { get; set; }

        public string CustmArg3 { get; set; }

        public string InUseDevice { get; set; }
    }
}
