using GmailServer.Enums;
using System;
using Volo.Abp.Application.Dtos;

namespace GmailServer.Checkers
{
    public class CheckerDto : EntityDto<long>
    {
        public Guid CheckerId { get; set; }

        public string CheckerIP { get; set; }

        public CheckerStatus Status { get; set; }

        public double FreeRam { get; set; }

        public double TotalRam { get; set; }

        public int UsingThread { get; set; }

        public int MaxThread { get; set; }

        public DateTime Created { get; set; }

        public DateTime LastCheck { get; set; }

    }
}
