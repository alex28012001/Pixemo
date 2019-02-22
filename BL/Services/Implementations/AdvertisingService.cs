using AutoMapper;
using BL.Helper;
using BL.DTO;
using BL.Infrastructure;
using BL.Services.DbAbstraction;
using DAL.Entities;
using DAL.UnitOfWork;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services.Implementations
{
    public class AdvertisingService : IAdvertisingService
    {
        private readonly IUnitOfWork _db;
        public AdvertisingService(IUnitOfWork db)
        {
            _db = db;
        }


        public OperationDetails AddAdvertising(string userId, string imageId, DateTime expirationDate)
        {
            var ad = _db.Advertising.AnyWithExpressionTree(p => p.ImageId.Equals(imageId));
            if (!ad)
            {
                var user = _db.UserManager.FindById(userId);
                if (user != null)
                {
                    if (_db.UserManager.IsInRole(userId, "moderatorAdvertising"))
                    {
                        _db.Advertising.Create(new Advertising() { User = user, ImageId = imageId, ExpirationDate = expirationDate });
                        _db.Save();
                        return new OperationDetails(true, "Реклама добавлены", "");
                    }

                    var result = _db.UserManager.AddToRole(userId, "moderatorAdvertising");
                    if (result.Errors.Count() > 0)
                        return new OperationDetails(false, result.Errors.FirstOrDefault(), "");

                    _db.Advertising.Create(new Advertising() { User = user, ImageId = imageId , ExpirationDate = expirationDate});
                    _db.Save();
                    return new OperationDetails(true, "Реклама добавлена", "");
                }
                else
                {
                    return new OperationDetails(false, "Пользователь ненайден", "User");
                }
            }
            else
            {
                return new OperationDetails(false, "Реклама на этом месте занята", "ImageId");
            }
        }


        public async Task<OperationDetails> AddAdvertisingAsync(string userId, string imageId, DateTime expirationDate)
        {
            var ad = _db.Advertising.AnyWithExpressionTree(p => p.ImageId.Equals(imageId));
            if (!ad)
            {
                var user = await _db.UserManager.FindByIdAsync(userId);
                if (user != null)
                {
                    if (await _db.UserManager.IsInRoleAsync(userId, "moderatorAdvertising"))
                    {
                        _db.Advertising.Create(new Advertising() { User = user, ImageId = imageId, ExpirationDate = expirationDate });
                        await _db.SaveAsync();
                        return new OperationDetails(true, "Реклама добавлены", "");
                    }

                    var result = await _db.UserManager.AddToRoleAsync(userId, "moderatorAdvertising");
                    if (result.Errors.Count() > 0)
                        return new OperationDetails(false, result.Errors.FirstOrDefault(), "");

                    _db.Advertising.Create(new Advertising() { User = user, ImageId = imageId, ExpirationDate = expirationDate });
                    await _db.SaveAsync();
                    return new OperationDetails(true, "Реклама добавлена", "");
                }
                else
                {
                    return new OperationDetails(false, "Пользователь ненайден", "User");
                }
            }
            else
            {
                return new OperationDetails(false, "Реклама на этом месте занята", "ImageId");
            }
        }

        public AdvertisingDTO GetAdvertising(string imageId)
        {
            var ad = _db.Advertising.FindWithExpressionTree(p => p.ImageId.Equals(imageId)).FirstOrDefault();
            if(ad!=null)
            {
                var mapper = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Advertising, AdvertisingDTO>()
                    .ForMember("OwnerId", src => src.MapFrom(p => p.User.Id)); 
                } ).CreateMapper();           
                return mapper.Map<Advertising, AdvertisingDTO>(ad);
            }
            return null;
        }


        public bool IsTaken(string imageId)
        {
            return _db.Advertising.Any(p => p.ImageId.Equals(imageId));
        }


        public bool IsMyAd(string userId, string imageId)
        {
            return _db.Advertising.FindWithExpressionTree(p => p.ImageId.Equals(imageId) && p.User.Id.Equals(userId)).Any();
        }


        public OperationDetails AddAdvertisingImage(string userId, string imageId, string imageUrl)
        {
            return AddAdverisingOption(userId, imageId, imageUrl, null);
        }


        public async Task<OperationDetails> AddAdvertisingImageAsync(string userId, string imageId, string imageUrl)
        {
            return await AddAdverisingOptionAsync(userId, imageId, imageUrl, null);
        }

        public OperationDetails AddAdvertisingLink(string userId, string imageId, string adLink)
        {
            return AddAdverisingOption(userId, imageId, null, adLink);
        }


        public async Task<OperationDetails> AddAdvertisingLinkAsync(string userId, string imageId, string adLink)
        {
            return await AddAdverisingOptionAsync(userId, imageId, null, adLink);
        }


        private async Task<OperationDetails> AddAdverisingOptionAsync(string userId, string imageId, string imageUrl, string adLink)
        {
            var ad = _db.Advertising.Find(p => p.User.Id.Equals(userId) && p.ImageId.Equals(imageId)).FirstOrDefault();
            if (ad != null)
            {
                UpdateArgument(imageUrl, adLink, ad);
                await _db.SaveAsync();
                return new OperationDetails(true, "Реклама успешно обновлена", "");
            }
            return new OperationDetails(false, "Не найдено место для рекламы, проверте id пользователя и id картинки", "");
        }


        private OperationDetails AddAdverisingOption(string userId, string imageId, string imageUrl, string adLink)
        {
            var ad = _db.Advertising.Find(p => p.User.Id.Equals(userId) && p.ImageId.Equals(imageId)).FirstOrDefault();
            if (ad != null)
            {
                UpdateArgument(imageUrl, adLink, ad);
                _db.SaveAsync();
                return new OperationDetails(true, "Реклама успешно обновлена", "");
            }
            return new OperationDetails(false, "Не найдено место для рекламы, проверте id пользователя и id картинки", "");
        }


        private void UpdateArgument(string imageUrl, string adLink, Advertising ad)
        {
            if (imageUrl != null)
                ad.ImageUrl = imageUrl;
            if (adLink != null)
                ad.AdvertisingLink = adLink;

            _db.Advertising.Update(ad);
        }


        public OperationDetails RemoveAdvertising(AdvertisingDTO adDTO)
        {
            var ad = _db.Advertising.Find(p => p.ImageId.Equals(adDTO.ImageId)).FirstOrDefault();
            if (ad != null)
            {
                 _db.Advertising.Remove(ad); //удаляю по id т.к при нахождении рекламы я получаю не все данный, а для удаления по сущности нужны все...
                 _db.Save();
                int countAd = _db.Advertising.CountWithExpressionTree(p => p.User.Id.Equals(adDTO.OwnerId));
                if (countAd < 1)
                    _db.UserManager.RemoveFromRoles(adDTO.OwnerId, "moderatorAdvertising");
                return new OperationDetails(true, "", "");
            }
            return new OperationDetails(false, "не найдена реклама с таким id пользователя или id рекламного места", "");
        }


        public async Task<OperationDetails> RemoveAdvertisingAsync(AdvertisingDTO adDTO)
        {
            var ad = _db.Advertising.Find(p => p.ImageId.Equals(adDTO.ImageId)).FirstOrDefault();
            if (ad != null)
            {
                _db.Advertising.Remove(ad); //удаляю по id т.к при нахождении рекламы я получаю не все данный, а для удаления по сущности нужны все...
                await _db.SaveAsync();
                int countAd = _db.Advertising.CountWithExpressionTree(p => p.User.Id.Equals(adDTO.OwnerId));
                if (countAd < 1)
                    await _db.UserManager.RemoveFromRolesAsync(adDTO.OwnerId, "moderatorAdvertising");
                return new OperationDetails(true, "", "");
            }
            return new OperationDetails(false, "не найдена реклама с таким id пользователя или id рекламного места", "");
        }

    }
}
