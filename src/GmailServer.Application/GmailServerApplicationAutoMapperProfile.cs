using AutoMapper;
using GmailServer.Checkers;
using GmailServer.Entities;
using GmailServer.FakeSettings;
using GmailServer.Gmails;
using GmailServer.RecoveryEmails;
using GmailServer.TaskChecks;

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

            CreateMap<TaskCheck, TaskCheckDto>();
            CreateMap<CreateUpdateTaskCheckDto, TaskCheck>();

            CreateMap<Checker, CheckerDto>();

            CreateMap<RecoveryEmail, RecoveryEmailDto>();
            CreateMap<CreateUpdateRecoveryEmailDto, RecoveryEmail>();
        }
    }
}
