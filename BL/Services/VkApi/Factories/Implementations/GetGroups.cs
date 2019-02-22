using VkNet.Model.RequestParams;
using System.Collections.Generic;
using System.Threading.Tasks;
using BL.Entities.Groups;
using BL.Entities.Params;
using VkNet.Abstractions;
using AutoMapper;
using VkNet.Enums.SafetyEnums;
using BL.Services.VkServices.Factories.Abstraction;

namespace BL.Services.VkServices.Factories.Implementations
{
    public class GetGroups : IGetGroups
    {
        private readonly IVkApiCategories _api;
        public GetGroups(IVkApiCategories api)
        {
            _api = api;
        }

        public async Task<IEnumerable<Group>> GetAsync(GroupSearchParams param)
        {
            var groups = await _api.Groups.SearchAsync(new GroupsSearchParams()
            { Query = param.Query, Count = param.Count, Offset = param.Offset, Type = GroupType.Undefined });

            return GetMapperGroups(groups);
        }

        public IEnumerable<Group> Get(GroupSearchParams param)
        {
            var groups = _api.Groups.Search(new GroupsSearchParams()
            { Query = param.Query, Count = param.Count, Offset = param.Offset, Type = GroupType.Undefined });

            return GetMapperGroups(groups);
        }


        private IEnumerable<Group> GetMapperGroups(IEnumerable<VkNet.Model.Group> groups)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<VkNet.Model.Group, Group>()).CreateMapper();
            return mapper.Map<IEnumerable<VkNet.Model.Group>, IEnumerable<Group>>(groups);
        }
    }
}
