using AutoMapper;
using GmailServer.AppleIdNones;
using GmailServer.AppleIdRaws;
using GmailServer.AppleIds;
using GmailServer.Entities;
using GmailServer.FakeSettings;
using GmailServer.GmailPremiums;
using GmailServer.GmailResources;
using GmailServer.Gmails;
using GmailServer.GmailTypes;
using GmailServer.MomoAccounts;
using GmailServer.OwnerConfigs;
using GmailServer.RecoveryEmails;
using GmailServer.Web.Pages.AppleIdNones;
using GmailServer.Web.Pages.AppleIds.ViewModels;
using GmailServer.Web.Pages.FakeSettings.ViewModels;
using GmailServer.Web.Pages.GmailPremiums.ViewModels;
using GmailServer.Web.Pages.GmailResources;
using GmailServer.Web.Pages.GmailResources.ViewModels;
using GmailServer.Web.Pages.MomoAccounts;
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

            CreateMap<GmailTypeDto, CreateUpdateGmailTypeDto>();

            CreateMap<FakeSettingDto, CreateUpdateFakeSettingDto>();
            CreateMap<FakeSetting, FakeSettingDto>();
            CreateMap<FakeSettingViewModel, CreateUpdateFakeSettingDto>();
            CreateMap<FakeSettingDto, FakeSettingViewModel>();
            CreateMap<RecoveryEmailViewModel, CreateManyRecoveryEmailInputDto>();
            CreateMap<RecoveryEmailDto, RecoveryEmailViewModel>();
            CreateMap<GmailPremiumViewModel, CreateManyGmailPremiumInputDto>();
            CreateMap<AppleIdViewModel, CreateManyAppleIdInputDto>();
            CreateMap<GmailResourceViewModel, CreateManyGmailResourceInputDto>();
            CreateMap<ReupFormModel, ReupGmailResourceInputDto>();

            CreateMap<CreateManyMomoAccoutModel, CreateManyMomoAccountInputDto>();
            CreateMap<AppleIdNoneViewModel, CreateManyAppleIdNoneInputDto>();
            CreateMap<AppleIdRaw, AppleIdRawDto>();

            CreateMap<OwnerConfigDto, CreateUpdateOwnerConfigDto>();
        }
    }
}
