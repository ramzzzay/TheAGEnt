using System;
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
    public class PhotoManager : IPhotoManager
    {
        private readonly TheAGEntContext _context;

        public PhotoManager(TheAGEntContext context)
        {
            _context = context;
        }

        public async Task<List<Album>> GetUserAlbumsById(string userId)
        {
            var response = await _context
                .Users
                .Where(u => u.Id == userId)
                .SelectMany(u => u.Albums)
                .ToListAsync();

            var query = _context
                .Users
                .Where(u => u.Id == userId)
                .SelectMany(u => u.Albums)
                .ToString(); //TODO: For debug query

            return response;
        }

        public IQueryable<string> GetUserAlbumsNameById(string userId) => _context
            .Users
            .Where(u => u.Id == userId)
            .SelectMany(u => u.Albums)
            .Select(a => a.Name);

        public async Task<List<Album>> GetUserAlbumsByEmail(string userEmail) => await _context
            .Users
            .Where(u => u.Email == userEmail)
            .SelectMany(u => u.Albums)
            .ToListAsync();

        public async Task<List<Album>> GetUserAlbumsNameByNickNameAsync(string nickname) => await _context
            .Users
            .Where(u => u.NickName == nickname)
            .SelectMany(u => u.Albums)
            .ToListAsync();

        public async Task<List<Picture>> GetUserPhotosByNickNameAndAlbumNameAsync(string nickname, string albumName)
            => await _context
                .Users
                .Where(u => u.NickName == nickname)
                .SelectMany(a => a.Albums).Where(a => a.Name == albumName)
                .SelectMany(p => p.Pictures)
                .ToListAsync();

        public async Task<List<Comment>> GetCommentsToPhotoByIdAsync(string nickname, string albumName, int photoId)
            => await _context
                .Users
                .Where(u => u.NickName == nickname)
                .SelectMany(a => a.Albums).Where(a => a.Name == albumName)
                .SelectMany(p => p.Pictures).Where(p => p.Id == photoId)
                .SelectMany(c => c.Comment)
                .ToListAsync();

        public async Task<IdentityResult> SendCommentsToPhotoByIdAsync(string SenderNickname,
            string photoOwnerNickname, string albumName, int photoId, string message)
        {
            var user = _context
                .Users
                .FirstOrDefault(x => x.NickName == SenderNickname);

            _context
                .Users
                .FirstOrDefault(x => x.NickName == photoOwnerNickname)
                ?.Albums.FirstOrDefault(a => a.Name == albumName)
                ?.Pictures.FirstOrDefault(p => p.Id == photoId)
                ?.Comment
                .Add(new Comment
                {
                    Message = message,
                    UserId = user,
                    PostingTime = DateTime.UtcNow
                });

            var response = await _context
                .SaveChangesAsync();

            return response >= 1 ? new IdentityResult("OK") : new IdentityResult("Error");
        }

        public async Task<int> GetGradesAverageAsync(string photoOwner, string albumName, int photoId)
        {
            var user = await _context
                .Users
                .FirstOrDefaultAsync(x => x.NickName == photoOwner);

            var album = user
                ?.Albums
                .FirstOrDefault(a => a.Name == albumName);

            var photo = album
                ?.Pictures
                .FirstOrDefault(p => p.Id == photoId);

            var grades = photo?.Grades;

            return grades
                .Select(g => g.NumberOfGrade)
                .Sum()/grades.Count;
        }

        public async Task<IdentityResult> SetGradesAsync(string senderNickname, string photoOwner, string albumName,
            int photoId,
            int grade)
        {
            var sender = await _context
                .Users.FirstOrDefaultAsync(x => x.NickName == senderNickname);

            var user = await _context
                .Users
                .FirstOrDefaultAsync(x => x.NickName == photoOwner);

            var album = user
                ?.Albums
                .FirstOrDefault(a => a.Name == albumName);

            var photo = album
                ?.Pictures
                .FirstOrDefault(p => p.Id == photoId);

            var grObj = new Grade
            {
                NumberOfGrade = grade,
                Graded = true
            };

            photo?.Grades.Add(grObj);
            sender.Grades.Add(grObj);

            var response = await _context
                .SaveChangesAsync();

            return response >= 1 ? new IdentityResult("OK") : new IdentityResult("Error");
        }

        public async Task<User> FindByNickNameAsync(string nickname)
        {
            return await _context
                .Users
                .FirstOrDefaultAsync(u => u.NickName == nickname);
        }

        public async Task<IdentityResult> ImageUploadAsync(string userId, string filePath, string email, string album)
        {
            var user = _context
                .Users
                .Single(u => u.Id == userId);

            if (user.Albums.All(a => a.Name != album)) { 
                user
                    .Albums
                    .Add(new Album
                    {
                        Name = album,
                        Discription = "temp",
                        PathToCover = filePath
                    });
            }

            user.Albums.Single(a => a.Name == album)
                ?.Pictures.Add(new Picture
                {
                    Discription = "temp",
                    Label = "temp",
                    PathToImage = filePath
                });

            var result = await _context
                .SaveChangesAsync();

            return IdentityResult.Success;
        }
    }
}