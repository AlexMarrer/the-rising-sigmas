using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RisingSigma.Database.Entities
{
    public class WeekPlan
    {
        [Key]
        public Guid Id { get; set; }
        public int WeekNumber { get; set; }
        public int Version { get; set; }

        public TrainingPlan TrainingPlan { get; set; }
        public List<Exercise> Exercise { get; set; }
    }
}
