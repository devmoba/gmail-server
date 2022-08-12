using System;

namespace GmailServer.Gmails
{
    public class GmailReportDto
    {
        public DateTime Created { get; set; }

        public int TotalDaily { get; set; }

        public int Unknown { get; set; }

        public int Good { get; set; }

        public int Disable { get; set; }

        public int Notexist { get; set; }

        public int Verify { get; set; }

        public int Checking { get; set; }

        public int Uncheck { get; set; }
    }
}
