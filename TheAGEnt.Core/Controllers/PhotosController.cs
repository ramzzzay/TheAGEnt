using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
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
        public async Task<List<Album>> GetCurrentUserAlbums()
            => await _photoManager.GetUserAlbumsById(User.Identity.GetUserId());

        // GET api/Photos/GetUserAlbumsNameById
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("GetUserAlbumsNameById")]
        [HttpGet]
        [AllowAnonymous]
        public IQueryable<string> GetUserAlbumsNameById()
            => _photoManager.GetUserAlbumsNameById(User.Identity.GetUserId());

        // GET api/Photos/GetUserAlbumsNameByNickNameAsync
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("GetUserAlbumsNameByNickName")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IEnumerable<AlbumViewModel>> GetUserAlbumsNameByNickName(string nickname)
        {
            var albums = await _photoManager.GetUserAlbumsNameByNickNameAsync(nickname);
            var response = albums.Select(a => new AlbumViewModel
            {
                Name = a.Name,
                Discription = a.Discription,
                PathToCover = a.PathToCover
            });
            return response;
        }

        // GET api/Photos/GetUserPhotosByNickNameAndAlbumNameAsync
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("GetUserPhotosByNickNameAndAlbumName")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IEnumerable<PictureViewModel>> GetUserPhotosByNickName(string nickname, string albumName)
        {
            var pictures = await _photoManager.GetUserPhotosByNickNameAndAlbumNameAsync(nickname, albumName);
            var response = pictures.Select(p => new PictureViewModel
            {
                Id = p.Id,
                Label = p.Label,
                Discription = p.Discription,
                PathToImage = p.PathToImage
            });
            return response;
        }

        // GET api/Photos/GetCommentsToPhotoByIdAsync
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("GetCommentsToPhotoById")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IEnumerable<CommentViewModel>> GetCommentsToPhotoById(string nickName, string albumName,
            int photoId)
        {
            var comments = await _photoManager.GetCommentsToPhotoByIdAsync(nickName, albumName, photoId);

            var response = comments.Select(p => new CommentViewModel
            {
                NickName = p.UserId.NickName,
                Message = p.Message,
                PostingTime = p.PostingTime
            });

            return response;
        }

        // POST api/Photos/SendComment
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("SendComment")]
        [HttpPost]
        [Authorize(Roles = "user")]
        public async Task<IHttpActionResult> SendComment(CommentSendViewModel comment)
            => Ok(await _photoManager.SendCommentsToPhotoByIdAsync
            (comment.NickNameOfSender, comment.NickNameOfPhotoOwner, comment.AlbumName, comment.PhotoId,
                comment.Message));

        // GET api/Photos/GetAnyUserAlbums
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("GetAnyUserAlbums")]
        [HttpGet]
        [AllowAnonymous]
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

            var response =
                await _photoManager.ImageUploadAsync(userId, $"/Assets/imgs/ProfileImages/Images/{fileName}", email, album);

            return Ok(new {Msg = response.Errors, IsOk = response.Succeeded});
        }

        // GET api/Photos/GetGrades
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("GetGrades")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<int> GetGrades(string photoOwner,string albumName,int photoId) => await
            _photoManager.GetGradesAverageAsync(photoOwner, albumName, photoId);

        // POST api/Photos/SetGradesAsync
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("SetGradesAsync")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IdentityResult> SetGradesAsync(GradesViewModel grade)
        {
            return await _photoManager.SetGradesAsync(grade.NickNameOfSender, grade.photoOwner, grade.AlbumName, grade.PhotoId,
                grade.NumberOfGrade);
        }
    }
        
}