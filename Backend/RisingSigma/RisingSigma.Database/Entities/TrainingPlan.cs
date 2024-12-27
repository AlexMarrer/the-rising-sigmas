using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RisingSigma.Database.Entities
{
    public class TrainingPlan
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime StartTime { get; set; }
        public int CycleWeeks { get; set; }

        public User User { get; set; }
        public List<WeekPlan> WeekPlan { get; set; }
    }
}
