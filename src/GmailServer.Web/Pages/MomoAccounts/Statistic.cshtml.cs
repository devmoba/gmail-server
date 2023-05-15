using GmailServer.MomoAccounts;
using GmailServer.Permissions;
using Microsoft.AspNetCore.Authorization;

namespace GmailServer.Web.Pages.MomoAccounts
{
    [Authorize(GmailServerPermissions.MomoAccounts.Statistic)]
    public class StatisticModel : GmailServerPageModel
    {

    }
}
