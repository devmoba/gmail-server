using GmailServer.Enums;
using System;
using Volo.Abp.Domain.Entities;

namespace GmailServer.Entities
{
    public class Statistic : Entity<long>
    {
        public string EntityName { get; set; }

        public DateTime Date { get; set; }
         
        public string Username { get; set; }

        public int Total { get; set; } 

        public string Data { get; set; }

        public StatisticType Type { get; set; }

        public string HashCode { get; set; }

        public string Arg1 { get; set; }

        public string Arg2 { get; set; }

        public string Arg3 { get; set; }
    }
}
