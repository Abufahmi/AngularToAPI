using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AngularToAPI.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required, StringLength(150)]
        public string CategoryName { get; set; }
    }
}
