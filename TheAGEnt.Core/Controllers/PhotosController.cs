using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
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

        // GET api/Photos/GetAnyUserAlbums
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("GetAnyUserAlbums")]
        [HttpGet]
        [Authorize(Roles = "user")]
        public async Task<List<Album>> GetAnyUserAlbums(string userEmail) => await _photoManager.GetUserAlbumsByEmail(userEmail);

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