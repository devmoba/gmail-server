using AutoMapper;
using GmailServer.Entities;
using GmailServer.FakeSettings;
using GmailServer.Gmails;

namespace GmailServer
{
    public class GmailServerApplicationAutoMapperProfile : Profile
    {
        public GmailServerApplicationAutoMapperProfile()
        {
            /* You can configure your AutoMapper mapping configuration here.
             * Alternatively, you can split your mapping configurations
             * into multiple profile classes for a better organization. */

            CreateMap<Gmail, GmailDto>();
            CreateMap<CreateGmailDto, Gmail>();

            CreateMap<FakeSetting, FakeSettingDto>();
            CreateMap<CreateUpdateFakeSettingDto, FakeSetting>();
        }
    }
}
