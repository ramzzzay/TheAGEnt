using System.Collections.Generic;

namespace TheAGEnt.Domain.Entities
{
    public class Grade
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public virtual List<User> Users { get; set; }
        public virtual Picture Picture { get; set; }
    }
}