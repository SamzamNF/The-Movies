using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Movies.Model
{
    public class Hall
    {
        public string CinemaName { get; set; }
        public string HallNumber { get; set; }
        public List<MovieProgram> MoviePrograms { get; set; }
    }
}
