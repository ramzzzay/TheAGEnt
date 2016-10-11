using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using TheAGEnt.Domain.Abstract;
using TheAGEnt.Domain.Entities;
using TheAGEnt.Infrastructure.Concrete;

namespace TheAGEnt.Infrastructure.Infrastructure
{
    public class PhotoManager: IPhotoManager
    {
        private readonly TheAGEntContext _context;

        public PhotoManager(TheAGEntContext context)
        {
            _context = context;
        }
        public async Task<List<Album>> GetUserAlbumsById(string userId)
        {
            var response = await _context.Users.Where(u => u.Id == userId).SelectMany(u => u.Albums).ToListAsync();

            var query = _context.Users.Where(u => u.Id == userId).SelectMany(u => u.Albums).ToString(); //TODO: For debug query

            return response;
        }

        public IQueryable<string> GetUserAlbumsNameById(string userId) => _context.Users.Where(u => u.Id == userId).SelectMany(u => u.Albums).Select(a => a.Name);

        public async Task<List<Album>> GetUserAlbumsByEmail(string userEmail) => await _context.Users.Where(u => u.Email == userEmail).SelectMany(u => u.Albums).ToListAsync();

        public async Task<IdentityResult> ImageUpload(string userId, string filePath, string email, string album)
        {
            var user = _context.Users.Single(u => u.Id == userId);

            if (user.Albums.All(a => a.Name != album))
            {
                user.Albums.Add(new Album() { Name = album, Discription = "temp"});
            }

            user.Albums.Single(a=>a.Name ==album)
                ?.Pictures.Add(new Picture()
            {
                Discription = "temp",
                Label = "temp",
                PathToImage = filePath
            });

            var result = await _context.SaveChangesAsync();

            return IdentityResult.Success;
        }
    }
}