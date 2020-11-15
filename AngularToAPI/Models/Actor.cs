using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AngularToAPI.Models
{
    public class Actor
    {
        public int Id { get; set; }

        [Required]
        public string ActorName { get; set; }

        public string ActorPicture { get; set; }
    }
}
