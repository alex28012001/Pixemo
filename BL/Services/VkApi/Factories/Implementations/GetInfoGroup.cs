using BL.Helper;
using BL.Entities.Groups;
using BL.Services.VkApi.Factories.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkNet.Abstractions;


namespace BL.Services.VkApi.Factories.Implementations
{
    public class GetInfoGroup : IGetInfoGroup
    {
        private readonly IVkApiCategories _api;
        public GetInfoGroup(IVkApiCategories api)
        {
            _api = api;
        }

        public async Task<Group> GetInfoAsync(string groupId)
        {
            var vkGroup = (await _api.Groups.GetByIdAsync(null, groupId, null)).FirstOrDefault();
            return GenericMapperHelper<VkNet.Model.Group, Group>.Convert(vkGroup);
           
        }

        public Group GetInfo(string groupId)
        {
            var vkGroup = _api.Groups.GetById(null, groupId, null).FirstOrDefault();
            return GenericMapperHelper<VkNet.Model.Group, Group>.Convert(vkGroup);
        }

    }
}
