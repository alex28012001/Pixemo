using BL.Entities.Images;
using BL.Entities.Params;
using BL.ServiceFactory;
using BL.Services.MediaAbstraction;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


namespace ErrorChat.Controllers
{
    public class mediaController : Controller
    {
        private IMediaService _mediaService;
        private int ? _currentGroupsOfsset;

        public mediaController(IServiceFactory serviceFactory)
        {
            _mediaService = serviceFactory.CreateMediaService();
            //иницилизируем событие при изменении значения сдвига групп 
            //(возращаем текущее значения сдвига групп)
            _mediaService.IncrementGroupOffset += _mediaService_IncrementGroupOffset;
        }

        private void _mediaService_IncrementGroupOffset(int? offset)
        {
            _currentGroupsOfsset = offset;
        }

        public ActionResult main()
        {
            return View();
        }

       
        [HttpPost]
        public async Task<JsonResult> Images(string query, int? outOffsetMedia = 1, int? outOffsetGroups = 0)
        {
            var images = await _mediaService.SearchImagesAsync(new ImageSearchParams(query, count: 35, offset: outOffsetMedia, offsetGroups: outOffsetGroups));
            object[] mas = { images.Images, _currentGroupsOfsset };
            return Json(mas, JsonRequestBehavior.DenyGet);
        }


        [HttpPost]
        public async Task<JsonResult> Videos(string query, int? outOffsetMedia = 1)
        {
            var a = new VideosSearchParams(query, 20, outOffsetMedia);
            var videos = await _mediaService.SearchVideosAsync(a);
            return Json(videos.Videos, JsonRequestBehavior.DenyGet);
        }


        [HttpGet]
        public async Task<ActionResult> info(long id, string ownerId_Url)
        {
            Image imageById = null;
            try
            {
                imageById = await _mediaService.SearchImageByIdAsync(id, Convert.ToInt64(ownerId_Url));
            }
            catch (Exception ex)
            {
                string temp = "https://pixabay.com/get/" + ownerId_Url + ".jpg";
                imageById = new Image() { FullHDImageURL = temp, Id = id };
            }
            return View(imageById);
        }
    }
}