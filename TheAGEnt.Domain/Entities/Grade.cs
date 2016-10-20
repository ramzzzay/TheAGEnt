using System.Collections.Generic;

namespace TheAGEnt.Domain.Entities
{
    public class Grade
    {
        public int Id { get; set; }
        public int NumberOfGrade { get; set; }
        public bool Graded { get; set; }
        public virtual List<User> Users { get; set; }
        public virtual Picture Picture { get; set; }
    }
}