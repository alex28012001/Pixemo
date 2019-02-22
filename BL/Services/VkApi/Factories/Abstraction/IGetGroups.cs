using BL.Entities.Groups;
using BL.Entities.Params;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BL.Services.VkServices.Factories.Abstraction
{
    public interface IGetGroups
    {
        Task<IEnumerable<Group>> GetAsync(GroupSearchParams param);
        IEnumerable<Group> Get(GroupSearchParams param);
    }
}
