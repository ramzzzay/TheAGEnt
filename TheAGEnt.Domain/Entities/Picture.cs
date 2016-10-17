using System.Collections.Generic;

namespace TheAGEnt.Domain.Entities
{
    public class Picture
    {
        public Picture()
        {
            Comment = new List<Comment>();
            Grades = new List<Grade>();
        }
        public int Id { get; set; }
        public string Label { get; set; }
        public string Discription { get; set; }
        public string PathToImage { get; set; }
        public virtual Album AlbumId { get; set; }
        public virtual ICollection<Comment> Comment { get; set; }
        public virtual ICollection<Grade> Grades { get; set; }
    }
}