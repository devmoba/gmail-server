using AutoMapper;
using GmailServer.AppleIds;
using GmailServer.Entities;
using GmailServer.FakeSettings;
using GmailServer.GmailPremiums;
using GmailServer.Gmails;
using GmailServer.RecoveryEmails;
using GmailServer.Web.Pages.AppleIds.ViewModels;
using GmailServer.Web.Pages.FakeSettings.ViewModels;
using GmailServer.Web.Pages.GmailPremiums.ViewModels;
using GmailServer.Web.Pages.RecoveryEmails.ViewModels;

namespace GmailServer.Web
{
    public class GmailServerWebAutoMapperProfile : Profile
    {
        public GmailServerWebAutoMapperProfile()
        {
            //Define your object mappings here, for the Web project
            CreateMap<Gmail, GmailExcelModel>()
                .AfterMap((a,b) => b.Date = a.Date.ToString("dd/MM/yyyy HH:mm"))
                .AfterMap((a,b) => b.Status = (int)a.Status);

            CreateMap<FakeSettingDto, CreateUpdateFakeSettingDto>();
            CreateMap<FakeSetting, FakeSettingDto>();
            CreateMap<FakeSettingViewModel, CreateUpdateFakeSettingDto>();
            CreateMap<FakeSettingDto, FakeSettingViewModel>();
            CreateMap<RecoveryEmailViewModel, CreateManyRecoveryEmailInputDto>();
            CreateMap<RecoveryEmailDto, RecoveryEmailViewModel>();
            CreateMap<GmailPremiumViewModel, CreateManyGmailPremiumInputDto>();
            CreateMap<AppleIdViewModel, CreateManyAppleIdInputDto>();
        }
    }
}
