using System;

namespace GmailServer.MomoAccounts.Statistics
{
    public class MomoAccountStatisticDto
    {
        public DateTime CreatedTime { get; set; }

        public string UploadGroup { get; set; }

        public int Total { get; set; }

        public int NotUse { get; set; }

        public int InUse { get; set; }

        public int Lock { get; set; }

        public int WrongPassword { get; set; }

        public int Unknown { get; set; }
    }
}
