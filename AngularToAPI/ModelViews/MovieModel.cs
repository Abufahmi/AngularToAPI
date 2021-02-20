using AngularToAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularToAPI.ModelViews
{
    public class MovieModel
    {
        public Movie Movie { get; set; }
        public IEnumerable<MovieActor> Actors { get; set; }
        public IEnumerable<MovieLink> Links { get; set; }
    }
}
