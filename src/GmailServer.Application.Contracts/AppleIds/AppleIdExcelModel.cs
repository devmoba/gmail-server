using System;

namespace GmailServer.AppleIds
{
    public class AppleIdExcelModel
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string Username { get; set; }

        public string Status { get; set; }

        public int PurchaseNumber { get; set; }

        public int TakenOutNumber { get; set; }

        public string Ccv { get; set; }

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }

        public DateTime TakenTime { get; set; }

    }
}
