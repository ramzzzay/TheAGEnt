using System;

namespace TheAGEnt.Core.Models
{
    public class AlbumViewModel
    {
        public string Name { get; set; }
        public string Discription { get; set; }
        public string PathToCover { get; set; }
    }

    public class PictureViewModel
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public string Discription { get; set; }
        public string PathToImage { get; set; }
    }

    public class CommentViewModel
    {
        public string Message { get; set; }
        public string NickName { get; set; }
        public DateTime PostingTime { get; set; }
    }

    public class CommentSendViewModel
    {
        public string Message { get; set; }
        public string NickNameOfSender { get; set; }
        public string NickNameOfPhotoOwner { get; set; }
        public string AlbumName { get; set; }
        public int PhotoId { get; set; }
    }
    public class GradesViewModel
    {
        public string NickNameOfSender { get; set; }
        public string photoOwner { get; set; }
        public string AlbumName { get; set; }
        public int PhotoId { get; set; }
        public int NumberOfGrade { get; set; }
        public bool Graded { get; set; }
    }
}