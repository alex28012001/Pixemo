using BL.Entities.Groups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services.VkApi.Factories.Abstraction
{
    public interface IGetInfoGroup
    {
        Task<Group> GetInfoAsync(string groupId);
        Group GetInfo(string groupId);
    }
}
