using GmailServer.Enums;
using System;
using Volo.Abp.Application.Dtos;

namespace GmailServer.AppleIdNones
{
    public class AppleIdNoneGetOutputDto : EntityDto<long>
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string Username { get; set; }

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }

        public DateTime TakenTime { get; set; }

        public AppleIdNoneStatus Status { get; set; }

        public int PurchaseNumber { get; set; }

        public int TakenOutNumber { get; set; }

        public string Ccv { get; set; }

        public string SecretAnswer1 { get; set; }

        public string SecretAnswer2 { get; set; }

        public string SecretAnswer3 { get; set; }

        public string DateOfBirth { get; set; }

        public bool AddPaymentCompleted { get; set; }

        public RemovePaymentStatus RemovePaymentStatus { get; set; }

        public DateTime RemoveTakenTime { get; set; }

        public DateTime RemoveUpdateTime { get; set; }
    }
}
