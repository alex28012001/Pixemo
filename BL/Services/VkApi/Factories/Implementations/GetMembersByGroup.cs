using System.Threading.Tasks;
using BL.Entities.Groups;
using VkNet.Abstractions;
using VkNet.Model.RequestParams;
using VkNet.Model;
using VkNet.Utils;
using AutoMapper;
using BL.Services.VkServices.Factories.Abstraction;

namespace BL.Services.Factories.VkServices.Implementations
{
    public class GetMembersByGroup : IGetMembersByGroup
    {
        private readonly IVkApiCategories _api;
        public GetMembersByGroup(IVkApiCategories api)
        {
            _api = api;
        }

        public async Task<InfoGroup> GetMembersAsync(string OwnerId)
        {
            var groupMember = await _api.Groups.GetMembersAsync(new GroupsGetMembersParams()
            { GroupId = OwnerId, Count = 0 });
            return GetMapperMembersGroup(groupMember);
        }

        public InfoGroup GetMembers(string OwnerId)
        {
            var groupMember = _api.Groups.GetMembers(new GroupsGetMembersParams()
            { GroupId = OwnerId, Count = 0 });
            return GetMapperMembersGroup(groupMember);
        }

        private InfoGroup GetMapperMembersGroup(VkCollection<User> groupInfo)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<VkCollection<User>,InfoGroup>()
            .ForMember("CountSubscribers",src =>src.MapFrom(p=>p.TotalCount))).CreateMapper();
            return mapper.Map<VkCollection<User>,InfoGroup>(groupInfo);
        }
    }
}
