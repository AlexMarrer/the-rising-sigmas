using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Flags]
public enum DayOfWeek
{
    Sunday = 0,             // 0
    Monday = 1 << 0,        // 1
    Tuesday = 1 << 1,       // 2
    Wednesday = 1 << 2,     // 4
    Thursday = 1 << 3,      // 8
    Friday = 1 << 4,        // 16
    Saturday = 1 << 5       // 32
}

namespace RisingSigma.Database.Entities
{
    public class Exercise
    {
        [Key]
        public Guid Id { get; set; }
        public int Reps { get; set; }
        public int Sets { get; set; }
        public double RPE { get; set; }
        public DayOfWeek Day { get; set; }
        public string notes { get; set; }

        public WeekPlan WeekPlan { get; set; }
        public ExerciseTemplate ExerciseTemplate { get; set; }
    }
}
