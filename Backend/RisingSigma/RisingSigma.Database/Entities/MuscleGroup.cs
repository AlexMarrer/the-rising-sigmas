using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RisingSigma.Database.Entities
{
    public class MuscleGroup
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
