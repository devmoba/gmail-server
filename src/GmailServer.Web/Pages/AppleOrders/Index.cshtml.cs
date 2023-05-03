using GmailServer.Enums;
using GmailServer.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;

namespace GmailServer.Web.Pages.AppleOrders
{
    [Authorize(GmailServerPermissions.AppleOrders.Default)]
    public class IndexModel : GmailServerPageModel
    {
        public void OnGet()
        {
            var linkStatusSelections = Enum.GetValues(typeof(LinkStatus)).Cast<LinkStatus>()
               .Select(item => new SelectListItem()
               {
                   Text = item.ToString(),
                   Value = $"{(int)item}"
               }).ToList();

            var addPaymentStatusSelections = Enum.GetValues(typeof(AddPaymentStatus)).Cast<AddPaymentStatus>()
              .Select(item => new SelectListItem()
              {
                  Text = item.ToString(),
                  Value = $"{(int)item}"
              }).ToList();

            ViewData.Add("linkStatusSelections", SerializeObject(linkStatusSelections));
            ViewData.Add("addPaymentStatusSelections", SerializeObject(addPaymentStatusSelections));
        }
    }
}
