using System.Collections.Generic;

namespace GmailServer.Hotmails
{
    public class Data
    {
        public string TransId { get; set; }

        public string Product { get; set; }

        public int Quantity { get; set; }   

        public decimal UnitPrice { get; set; }

        public double UnitPriceUsd { get; set; }

        public decimal TotalAmount { get; set; }

        public double TotalAmountUsd { get; set; }

        public List<Hotmail> Emails { get; set; }
    }
}
