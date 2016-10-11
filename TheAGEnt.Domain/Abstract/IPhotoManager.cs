using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using TheAGEnt.Domain.Entities;

namespace TheAGEnt.Domain.Abstract
{
    public interface IPhotoManager
    {
        Task<List<Album>> GetUserAlbumsById(string userId);
        IQueryable<string> GetUserAlbumsNameById(string userId);
        Task<List<Album>> GetUserAlbumsByEmail(string userEmail);
        Task<IdentityResult> ImageUpload(string userId,string filePath,string email, string album);
    }
}