using BL.DTO;
using BL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services.DbAbstraction
{
    public interface IAdvertisingService
    {

        OperationDetails AddAdvertising(string userId, string imageId, DateTime expirationDate);
        Task<OperationDetails> AddAdvertisingAsync(string userId, string imageId, DateTime expirationDate);

        OperationDetails AddAdvertisingImage(string userId, string imageId, string imageUrl);
        Task<OperationDetails> AddAdvertisingImageAsync(string userId, string imageId, string imageUrl);
        OperationDetails AddAdvertisingLink(string userId, string imageId, string adLink);
        Task<OperationDetails> AddAdvertisingLinkAsync(string userId, string imageId, string adLink);

        OperationDetails RemoveAdvertising(AdvertisingDTO adDTO);
        Task<OperationDetails> RemoveAdvertisingAsync(AdvertisingDTO adDTO);

        AdvertisingDTO GetAdvertising(string imageId);
        bool IsTaken(string imageId);
        bool IsMyAd(string userId,string imageId);
    }
}
