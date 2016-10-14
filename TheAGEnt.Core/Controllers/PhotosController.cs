using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using TheAGEnt.Core.Models;
using TheAGEnt.Core.Util;
using TheAGEnt.Domain.Abstract;
using TheAGEnt.Domain.Entities;

namespace TheAGEnt.Core.Controllers
{
    [Authorize]
    [RoutePrefix("api/Photos")]
    public class PhotosController : ApiController
    {
        private readonly IPhotoManager _photoManager;

        public PhotosController()
        {
        }

        public PhotosController(IPhotoManager photoManager)
        {
            _photoManager = photoManager;
        }

        // GET api/Photos/GetUserAlbumsById
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("GetUserAlbumsById")]
        [HttpGet]
        [Authorize(Roles = "user")]
        public async Task<List<Album>> GetCurrentUserAlbums() => await _photoManager.GetUserAlbumsById(User.Identity.GetUserId());

        // GET api/Photos/GetUserAlbumsNameById
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("GetUserAlbumsNameById")]
        [HttpGet]
        [Authorize(Roles = "user")]
        public IQueryable<string> GetUserAlbumsNameById() => _photoManager.GetUserAlbumsNameById(User.Identity.GetUserId());

        // GET api/Photos/GetUserAlbumsNameByNickName
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("GetUserAlbumsNameByNickName")]
        [HttpGet]
        [Authorize(Roles = "user")]
        public async Task<IEnumerable<AccountViewModels.AlbumViewModel>> GetUserAlbumsNameByNickName(string nickname)
        {
            var albums = await _photoManager.GetUserAlbumsNameByNickName(nickname);
            var response = albums.Select(a => new AccountViewModels.AlbumViewModel()
            {
                Name = a.Name,
                Discription = a.Discription,
            });
            return response;
        }

        // GET api/Photos/GetUserPhotosByNickNameAndAlbumName
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("GetUserPhotosByNickNameAndAlbumName")]
        [HttpGet]
        [Authorize(Roles = "user")]
        public async Task<IEnumerable<AccountViewModels.PictureViewModel>> GetUserPhotosByNickName(string nickname, string albumName)
        {
            var pictures = await _photoManager.GetUserPhotosByNickNameAndAlbumName(nickname,albumName);
            var response = pictures.Select(p => new AccountViewModels.PictureViewModel()
            {
                Id = p.Id,
                Label = p.Label,
                Discription = p.Discription,
                PathToImage = p.PathToImage
            });
            return response;
        }

        // GET api/Photos/GetCommentsToPhotoById
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("GetCommentsToPhotoById")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IEnumerable<AccountViewModels.CommentViewModel>> GetCommentsToPhotoById(string nickName,string albumName,int photoId)
        {
            var comments = await _photoManager.GetCommentsToPhotoById(nickName,albumName,photoId);

            var response = comments.Select(p => new AccountViewModels.CommentViewModel()
            {
                NickName = p.UserId.NickName,
                Message = p.Message
            });

            return response;
        }

        // POST api/Photos/SendComment
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("SendComment")]
        [HttpPost]
        [Authorize(Roles = "user")]
        public async Task<IHttpActionResult> SendComment(AccountViewModels.CommentSendViewModel comment)
            => Ok(await _photoManager.SendCommentsToPhotoById
                (comment.NickNameOfSender, comment.NickNameOfPhotoOwner, comment.AlbumName, comment.PhotoId, comment.Message));

        // GET api/Photos/GetAnyUserAlbums
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("GetAnyUserAlbums")]
        [HttpGet]
        [Authorize(Roles = "user")]
        public async Task<List<Album>> GetAnyUserAlbums(string userEmail)
            => await _photoManager.GetUserAlbumsByEmail(userEmail);

        [Route("UploadUserImage")]
        // POST api/Photos/UploadUserImage
        [MimeMultipart]
        [Authorize(Roles = "user")]
        public async Task<IHttpActionResult> UploadUserImage()
        {
            var uploadPath = HttpContext.Current.Server.MapPath("~/Assets/imgs/ProfileImages/Images");

            var multipartFormDataStreamProvider = new UploadMultipartFormProvider(uploadPath);

            // Read the MIME multipart asynchronously 
            await Request.Content.ReadAsMultipartAsync(multipartFormDataStreamProvider);

            var localFileName = multipartFormDataStreamProvider
                .FileData.Select(multiPartData => multiPartData.LocalFileName).FirstOrDefault();

            var userId = User.Identity.GetUserId();
            var fileName = Path.GetFileName(localFileName);
            var email = multipartFormDataStreamProvider.FormData["email"];
            var album = multipartFormDataStreamProvider.FormData["album"];

            var response = await _photoManager.ImageUpload(userId, $"/Assets/imgs/ProfileImages/Images/{fileName}", email, album);

            return Ok(new { Msg = response.Errors, IsOk = response.Succeeded });
        }
    }
}