using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AngularToAPI.Models
{
    public class MovieActor
    {
        public long Id { get; set; }

        [Required, StringLength(200)]
        public string ActorName { get; set; }

        public string ActorPicture { get; set; }

        [Required]
        public long MovieId { get; set; }
        [ForeignKey("MovieId")]
        public Movie Movie { get; set; }
    }
}
