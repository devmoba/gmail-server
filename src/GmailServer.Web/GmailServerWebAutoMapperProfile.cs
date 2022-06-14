using AutoMapper;
using GmailServer.Entities;
using GmailServer.FakeSettings;
using GmailServer.Gmails;

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
        }
    }
}
