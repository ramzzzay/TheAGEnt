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
        Task<IdentityResult> ImageUploadAsync(string userId,string filePath,string email, string album);
        Task<List<Album>> GetUserAlbumsNameByNickNameAsync(string nickname);
        Task<List<Picture>> GetUserPhotosByNickNameAndAlbumNameAsync(string nickname,string albumName);
        Task<List<Comment>> GetCommentsToPhotoByIdAsync(string nickName, string albumName, int photoId);
        Task<IdentityResult> SendCommentsToPhotoByIdAsync(string nickNameOfSender, string photoOwnerNickname, string albumName, int photoId, string message);

        Task<int> GetGradesAverageAsync(string nickNameOfSender, string albumName, int photoId);

        Task<IdentityResult> SetGradesAsync(string nickNameOfSender, string photoOwner, string albumName, int photoId,
            int grade);
    }
}