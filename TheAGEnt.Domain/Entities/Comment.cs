using System;

namespace TheAGEnt.Domain.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime PostingTime { get; set; }
        public virtual Picture PictureId { get; set; }
        public virtual User UserId { get; set; }
    }
}