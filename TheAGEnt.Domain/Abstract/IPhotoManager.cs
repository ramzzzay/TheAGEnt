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
        Task<List<Album>> GetUserAlbumsNameByNickName(string nickname);
        Task<List<Picture>> GetUserPhotosByNickNameAndAlbumName(string nickname,string albumName);
        Task<List<Comment>> GetCommentsToPhotoById(string nickName, string albumName, int photoId);
        Task<IdentityResult> SendCommentsToPhotoById(string nickNameOfSender, string nickNameOfPhotoOwner, string albumName, int photoId, string message);
    }
}