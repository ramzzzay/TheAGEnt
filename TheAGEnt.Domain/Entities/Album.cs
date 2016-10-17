using System.Collections.Generic;

namespace TheAGEnt.Domain.Entities
{
    public class Album
    {
        public Album()
        {
            Pictures = new List<Picture>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Discription { get; set; }
        public string PathToCover { get; set; }
        public virtual ICollection<Picture> Pictures { get; set; }
        public virtual User UserId { get; set; }

    }
}