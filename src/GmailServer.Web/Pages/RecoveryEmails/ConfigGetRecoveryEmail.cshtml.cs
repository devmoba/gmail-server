using GmailServer.Permissions;
using GmailServer.Web.Extensions;
using GmailServer.Web.Pages.RecoveryEmails.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace GmailServer.Web.Pages.RecoveryEmails
{
    [Authorize(GmailServerPermissions.RecoveryEmails.Config)]
    public class ConfigGetRecoveryEmailModel : GmailServerPageModel
    {
        [BindProperty]
        public ConfigGetRecoveryEmailFormModel Config { get; set; }

        private readonly IConfiguration _cfg;

        public ConfigGetRecoveryEmailModel(IConfiguration configuration)
        {
            _cfg = configuration;
        }

        public void OnGet()
        {
            var mailCodes = _cfg.GetSection("Workers:GetAndInsertHotmailWorker:ApiConfig:MailCodes").Get<List<string>>();

            Config = new ConfigGetRecoveryEmailFormModel()
            {
                Username = _cfg.GetValue<string>("Workers:GetAndInsertHotmailWorker:Username"),
                ApiUrl = _cfg.GetValue<string>("Workers:GetAndInsertHotmailWorker:ApiConfig:ApiUrl"),
                ApiKey = _cfg.GetValue<string>("Workers:GetAndInsertHotmailWorker:ApiConfig:ApiKey"),
                Quantity = _cfg.GetValue<int>("Workers:GetAndInsertHotmailWorker:ApiConfig:Quantity"),
                ReserveQuantity = _cfg.GetValue<int>("Workers:GetAndInsertHotmailWorker:ReserveQuantity"),
                MailCodes = string.Join("|", mailCodes),
            };
        }

        public void OnPost()
        {
            SettingHelper.AddOrUpdateAppSetting("Workers:GetAndInsertHotmailWorker:ReserveQuantity", Config.ReserveQuantity);
            SettingHelper.AddOrUpdateAppSetting("Workers:GetAndInsertHotmailWorker:Username", Config.Username);
            SettingHelper.AddOrUpdateAppSetting("Workers:GetAndInsertHotmailWorker:ApiConfig:ApiUrl", Config.ApiUrl);
            SettingHelper.AddOrUpdateAppSetting("Workers:GetAndInsertHotmailWorker:ApiConfig:ApiKey", Config.ApiKey);
            SettingHelper.AddOrUpdateAppSetting("Workers:GetAndInsertHotmailWorker:ApiConfig:Quantity", Config.Quantity);
            SettingHelper.AddOrUpdateAppSetting("Workers:GetAndInsertHotmailWorker:MailCodes", JToken.FromObject(Config.MailCodes.Split("|")));

        }
    }
}
