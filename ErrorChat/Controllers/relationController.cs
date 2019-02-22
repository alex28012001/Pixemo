using BL.ServiceFactory;
using BL.Services.DbAbstraction;
using ErrorChat.Models;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using BL.Infrastructure;
using System.Collections.Specialized;
using System.Collections.Generic;

namespace ErrorChat.Controllers
{
    [Authorize]
    public class relationController : Controller
    {
        private IUserService _userService;
        private ICommentService _commentService;
        private IChatService _chatService;
        private IAdvertisingService _advertisingService;

        public relationController(IServiceFactory serviceFactory)
        {
            _userService = serviceFactory.CreateUserService();
            _commentService = serviceFactory.CreateCommentService();
            _chatService = serviceFactory.CreateChatService();
            _advertisingService = serviceFactory.CreateAdvertisingService();
        }


        [AllowAnonymous]
        public ActionResult advertisinginfo()
        {
            return View();
        }


        [AllowAnonymous]
        public ActionResult chat()
        {
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> GetMessagesByChatRoomTitle(string title)
        {
            await _chatService.CreateChatRoomAsync(title);
            int chatRoomId = _chatService.FindChatRoomIdByTitle(title);
            var messages = await _chatService.GetMessageByChatRoomIdAndRemoveAsync(chatRoomId, TimeSpan.FromHours(5));
            return Json(messages, JsonRequestBehavior.DenyGet);
        }


        [AllowAnonymous]
        [HttpPost]
        public JsonResult GetCommentsByImageID(string imageId)
        {
            var comments = _commentService.FindCommentsByImageId(imageId);
            return Json(comments, JsonRequestBehavior.DenyGet);
        }


        [HttpPost]
        public async Task<ActionResult> SendComment(CommentModel comment)
        {         
            string userID = await _userService.FindIdByUserNameAsync(User.Identity.Name);
            var commentDTO = new BL.DTO.CommentDTO()
            {
                User = new BL.DTO.UserDTO() { Id = userID, UserName = User.Identity.Name },
                Date = DateTime.Now,
                ImageID = comment.ImageID,
                Text = comment.Text
            }; 
            var sendResult = await _commentService.AddCommentAsync(commentDTO);
            return sendResult.Successed ? Json(commentDTO) : null;
        }


        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> ModeratorPanel(string imageId)
        {
            string userId = await _userService.FindIdByUserNameAsync(User.Identity.Name);
            
            if (await _userService.IsInRoleAsync(userId, "moderatorAdvertising"))
            {
                var result = _advertisingService.IsMyAd(userId, imageId);
                if (result)
                {
                    string adLink = _advertisingService.GetAdvertising(imageId).AdvertisingLink;
                    return PartialView("ModeratorPanel", adLink);
                }
            }
            if (await _userService.IsInRoleAsync(userId, "admin"))
                return PartialView();

            return new EmptyResult();
        }


        [AllowAnonymous]
        [HttpPost]
        public async Task<JsonResult> GetAdvertising(string imageId)
        {
            AdvertisingResult adResult = new AdvertisingResult();
            var ad = _advertisingService.GetAdvertising(imageId);
            
            if (ad?.ExpirationDate <= DateTime.Now)
            {
                await _advertisingService.RemoveAdvertisingAsync(ad);
                adResult.Exist = false;
            }
            else
            {
                if (ad.ImageUrl != null) 
                { 
                    adResult.Exist = true;
                    adResult.Ad = ad;  
                }
            }
           
            return Json(adResult,JsonRequestBehavior.DenyGet);
        }
 

        [HttpPost]
        public async Task<JsonResult> UploadAdvertising()
        {
            DownloadResult result = null;
            string imagesUrl = null;
            string imageId = Request.Form["imageId"];
            string userId = await _userService.FindIdByUserNameAsync(User.Identity.Name);

            List<string> tempImagesUrl = new List<string>();
            if (Request.Files.Count > 0)
            {
                string filepath = Server.MapPath("~/AdvertisingImages");
                string[] mimes = { "image/jpeg", "image/jpg", "image/png" };
                int maxSize = 2 * 1024 * 1024; //max image size(2 mb)
                
                foreach (string file in Request.Files)
                {
                    var upload = Request.Files[file];

                    if (upload.ContentLength >= maxSize)
                    {
                        result = new DownloadResult() { ErrorMessage = "Максимальный размер изображения 2 мб" };
                        break;
                    }
                    else if (!mimes.Any(p => p.Equals(upload.ContentType)))
                    {
                        result = new DownloadResult() { ErrorMessage = "Недопустимый тип изображения.Разрешенные типы: .jpeg .jpg .png" };
                        break;
                    }

                    Guid guid = Guid.NewGuid();
                    string fileUrl = $@"{filepath}\{guid}.{upload.FileName}";
                    upload.SaveAs(fileUrl);

                    string imageUrl = $"/AdvertisingImages/{guid}.{upload.FileName};";
                    tempImagesUrl.Add(imageUrl);
                    imagesUrl += imageUrl;
                    
                    await AddAdvertisingLink(Request.Form, userId, imageId);
                }

                var uploadResult = await _advertisingService.AddAdvertisingImageAsync(userId, imageId, imagesUrl);
                result = new DownloadResult()
                {ImageId = imageId, ImagesUrl = tempImagesUrl, AdLink = Request.Form["advertisingLink"], ErrorMessage = uploadResult.Message, Successed = uploadResult.Successed };
            }
            else
            {
                var addLinkResult = await AddAdvertisingLink(Request.Form, userId, imageId);
                result = new DownloadResult() { Successed = addLinkResult.Successed, AdLink = Request.Form["advertisingLink"] };
            }
            return Json(result, JsonRequestBehavior.DenyGet);
        }


        private async Task<OperationDetails> AddAdvertisingLink (NameValueCollection form,string userId,string imageId)
        {
            //если ссылка на рекламу заносится в бд первый раз то всё норм, а если нет , то мы просто игнорируем то что пользователь хотел сохранить
            //это сделано для того , чтобы пользователи не могли изменять ссылок на рекламу
            if (form["savedLink"] == "false")
            {
                string adLink = Request.Form["advertisingLink"];
                if (adLink != "")
                    return await _advertisingService.AddAdvertisingLinkAsync(userId, imageId, adLink);     
            }
            return new OperationDetails(false, "ссылка уже добавлена или она пуста", "");
        }



        public async Task<ActionResult> advertising(long id ,string ownerId_Url)
        {
            string userId = await _userService.FindIdByUserNameAsync(User.Identity.Name);
            InfoAdvertising info = new InfoAdvertising() {OwnerId_Url = ownerId_Url,UserId = userId};
            try
            {
                info.SuccessUrl = $"http://pixemo.net/media/info/{id}/{Convert.ToInt64(ownerId_Url).ToString()}/";
                info.AdId = "v" + id.ToString();
            }
            catch (Exception ex)
            {
                info.SuccessUrl += $"http://pixemo.net/media/info/{id}/{ownerId_Url}/";
                info.AdId = "p" + id.ToString();
            }
            bool isTaken = _advertisingService.IsTaken(info.AdId);
            return isTaken == true ? View("busy", Convert.ToInt64(info.AdId.Remove(0,1))) : View(info);
        }


        [HttpGet]
        public ActionResult busy(long imageId)
        {
            return View(imageId);
        }


        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Paid(string notification_type, string operation_id, string label, string datetime
        ,decimal amount, decimal withdraw_amount, string sender, string sha1_hash, string currency, bool codepro)
        {
            AppSettingsReader reader = new AppSettingsReader();
            string key = (string)reader.GetValue("secretYandexKey", typeof(string)); // секретный код 
            // проверяем хэш
            string paramString = String.Format("{0}&{1}&{2}&{3}&{4}&{5}&{6}&{7}&{8}",
                notification_type, operation_id, amount, currency, datetime, sender,
                codepro.ToString().ToLower(), key, label);
            string paramStringHash1 = GetHash(paramString);
      
            // если хэши идентичны, добавляем данные о рекламе в бд
            if (paramStringHash1.Equals(sha1_hash))
            {
                bool success = false;
                DateTime expirationDate = DateTime.Now;
                switch (Convert.ToInt32(withdraw_amount)) //определяем по оплаченной сумме сколько времени будет висеть реклама
                {
                    case 180: expirationDate = expirationDate.AddMonths(1); success = true; break;
                    case 300: expirationDate = expirationDate.AddMonths(2); success = true; break;
                    case 420: expirationDate = expirationDate.AddMonths(3); success = true; break;
                    case 660: expirationDate = expirationDate.AddMonths(6); success = true; break;
                    case 1020: expirationDate = expirationDate.AddYears(1); success = true; break;
                    default: success = false; break;
                }

                if (success)
                {
                    //label - индитификатор платежа , в нем находиться id покупателя и id рекламного места (p2589123/j3242-dfs824-fdsf24)
                    string[] infoLabel = label.Split('/');
                    string imageId = infoLabel[0];  
                    string userId = infoLabel[1];        
                    var addAdResult = await _advertisingService.AddAdvertisingAsync(userId, imageId, expirationDate); 
                }
            }
            return new EmptyResult();
        }


        private string GetHash(string val)
        {
            SHA1 sha = new SHA1CryptoServiceProvider();
            byte[] data = sha.ComputeHash(Encoding.Default.GetBytes(val));
            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
    }
}