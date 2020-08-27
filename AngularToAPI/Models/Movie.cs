using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AngularToAPI.Models
{
    public class Movie
    {
        public long Id { get; set; }

        [Required]
        public string MovieName { get; set; }

        [Required]
        public string MovieStory { get; set; }

        [Required]
        public string MovieTrailer { get; set; }

        [Required]
        public string MoviePost{ get; set; }

        [Required]
        public int SubCategoryId { get; set; }
        [ForeignKey("SubCategoryId")]
        public SubCategory SubCategory { get; set; }
    }
}
