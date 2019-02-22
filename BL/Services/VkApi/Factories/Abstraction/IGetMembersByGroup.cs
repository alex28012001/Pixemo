using BL.Entities.Groups;
using System.Threading.Tasks;


namespace BL.Services.VkServices.Factories.Abstraction
{
    public interface IGetMembersByGroup
    {
        Task<InfoGroup> GetMembersAsync(string OwnerId);
        InfoGroup GetMembers(string OwnerId);
    }
}
