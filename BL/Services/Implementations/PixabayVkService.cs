using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Entities.Images;
using BL.Entities.Params;
using Ninject;
using BL.Entities.Videos;
using BL.Services.MediaAbstraction;
using System.Runtime.Caching;

namespace BL.Services.Implementations
{
    public class PixabayVkService : IMediaService
    {
        private readonly IApi _pixabayApi;
        private readonly IApi _vkApi;

        public PixabayVkService([Named("ImagesFromGroups")]IApi vkApi,
            [Named("QualityImages")]IApi pixabayApi)
        {
            _vkApi = vkApi;
            _pixabayApi = pixabayApi;
        }

        public event Action<int ?> IncrementGroupOffset; 

        public string AccessToken
        {
            get{  return null; }  
        }

        public IEnumerable<string> AccessTokens
        {
            get
            {
                return new string[] { _vkApi.AccessToken, _pixabayApi.AccessToken };
            }
        }


        public bool IsAuthorized
        {
            get
            {
                if (_vkApi.IsAuthorized && _pixabayApi.IsAuthorized)
                    return true;
                return false;
            }
        }


        public async Task<ImageResult> SearchImagesAsync(ImageSearchParams param)
        {
            MemoryCache cache = MemoryCache.Default;
            string key = $"{param.Query}.{param.Offset}.{param.OffsetGroups}";

            var cacheImages = cache.Get(key) as ImageResult;
            if(cacheImages == null)
            {
                var qualityImages = await _pixabayApi.SearchImagesAsync(param);
                CacheItemPolicy policy = new CacheItemPolicy() { SlidingExpiration = TimeSpan.FromMinutes(30) };
                if (qualityImages?.TotalCount >= 200)
                {
                    cache.Add(key, qualityImages,policy);
                    return qualityImages;
                }

                else
                {
                    var vkImages = await _vkApi.SearchImagesAsync(param);
                    while (vkImages == null) //если не нашли картинки в первых группах 
                    {
                        //сдвигаем группы , я ищу по 4 группы и если я из 4 групп не нашел 
                        //ни одной в которой есть картинки , то я сдвигаю группы на 4
                        param.OffsetGroups += 4; 

                        //продолжаем искать картинки с новым сдвигом групп
                        vkImages = await _vkApi.SearchImagesAsync(param);
                    }

                    //OffsetGroups - текущий сдвиг групп 
                    return VkImages(param.OffsetGroups, cache, key, policy, vkImages);
                }
            }
            return cacheImages;
        }


        public ImageResult SearchImages(ImageSearchParams param)
        {
            ObjectCache cache = MemoryCache.Default;
            string key = $"{param.Query}.{param.Offset}.{param.OffsetGroups}";

            var cacheImages = cache.Get(key) as ImageResult;
            if (cacheImages == null)
            {
                var qualityImages = _pixabayApi.SearchImages(param);
                CacheItemPolicy policy = new CacheItemPolicy() { SlidingExpiration = TimeSpan.FromMinutes(30) };
                if (qualityImages?.TotalCount > 500)
                {
                    cache.Add(key, qualityImages, policy);
                    return qualityImages;
                }

                else
                {
                    var vkImages = _vkApi.SearchImages(param);
                    while (vkImages == null) 
                    {     
                        param.OffsetGroups += 4; 
                        IncrementGroupOffset(param.OffsetGroups);
                        vkImages = _vkApi.SearchImages(param);
                    }

                    return VkImages(param.OffsetGroups, cache, key, policy, vkImages);
                }
            }
            return cacheImages;
        }


        private ImageResult VkImages(int ? offsetGroups, ObjectCache cache, string key, CacheItemPolicy policy, ImageResult vkImages)
        {
            //CurrentNumberGroups - номер группы в которой мы нашли картинки от 1 до 4 (до 4 т.к мы ищем группы по 4)

            //вызывающий код будет знать, то что сдвиг групп увеличился на какое-то число для того чтобы этот сдвиг 
            //был виден на клиентской части и счёт сдвига групп уже начинался с нового значения
            IncrementGroupOffset(offsetGroups + vkImages.CurrentNumberGroups);

            cache.Add(key, vkImages, policy);
            return vkImages;
        }


        public async Task<VideoResult> SearchVideosAsync(VideosSearchParams param)
        {
            ObjectCache cache = MemoryCache.Default;
            string key = $"{param.Query}.{param.Offset}";
            var videos = cache.Get(key) as VideoResult;
            if (videos == null)
            {
                CacheItemPolicy policy = new CacheItemPolicy() { SlidingExpiration = TimeSpan.FromMinutes(5) };
                var qualityVideos = await _pixabayApi.SearchVideosAsync(param);

                if (qualityVideos?.TotalCount > 30)
                {
                    cache.Add(key, qualityVideos, policy);
                    return qualityVideos;
                } 

                else
                { 
                    //изначально сдвиг идет по страницам ,
                    //поэтому если страница 2 то сдвиг нужно умножить на кол во картинок которое мы ищем
                    param.Offset = param.Offset * param.Count;

                    var vkVideos = await _vkApi.SearchVideosAsync(param);
                    cache.Add(key, vkVideos, policy);
                    return vkVideos; 
                }
            }
            return videos;
        }

        public VideoResult SearchVideos(VideosSearchParams param)
        {
            ObjectCache cache = MemoryCache.Default;
            string key = $"{param.Query}.{param.Offset}";
            var videos = cache.Get(key) as VideoResult;
            if (videos == null)
            {
                CacheItemPolicy policy = new CacheItemPolicy() { SlidingExpiration = TimeSpan.FromMinutes(5) };
                var qualityVideos = _pixabayApi.SearchVideos(param);

                if (qualityVideos?.TotalCount > 30)
                {
                    cache.Add(key, qualityVideos, policy);
                    return qualityVideos;
                }

                else
                {
                    param.Offset = param.Offset * param.Count;

                    var vkVideos = _vkApi.SearchVideos(param);
                    cache.Add(key, vkVideos, policy);
                    return vkVideos;
                }
            }
            return videos;
        }

        public async Task<Image> SearchImageByIdAsync(long idImage, long? idGroup)
        {
            return await _vkApi.SearchImageByIdAsync(idImage, idGroup);
        }

        public Image SearchImageById(long idImage, long? idGroup)
        {
            return _vkApi.SearchImageById(idImage, idGroup);
        }
    }
}
