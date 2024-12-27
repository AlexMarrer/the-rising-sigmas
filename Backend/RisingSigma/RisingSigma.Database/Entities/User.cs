using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RisingSigma.Database.Entities
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        public string Role { get; set; }
    }
}
